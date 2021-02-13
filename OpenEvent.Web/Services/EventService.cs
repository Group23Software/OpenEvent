using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Helpers;
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    public interface IEventService
    {
        // Host methods
        Task<EventViewModel> Create(CreateEventBody createEventBody);
        Task Cancel(Guid id);
        Task<List<EventHostModel>> GetAllHosts(Guid hostId);

        //public methods
        Task<EventDetailModel> GetForPublic(Guid id);
        Task<List<EventViewModel>> Search(string keyword, List<SearchFilter> filters);

        Task<List<Category>> GetAllCategories();
        Task<ActionResult<EventHostModel>> GetForHost(Guid id);
        Task Update(UpdateEventBody updateEventBody);
    }

    public class EventService : IEventService
    {
        private readonly ILogger<EventService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        private readonly HttpClient HttpClient;
        private readonly AppSettings AppSettings;

        public EventService(ApplicationContext context, ILogger<EventService> logger, IMapper mapper,
            HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
            HttpClient = httpClient;
            AppSettings = appSettings.Value;
        }

        public async Task<EventViewModel> Create(CreateEventBody createEventBody)
        {
            var host = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == createEventBody.HostId);

            if (host == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            var cords = await GetAddressCords(createEventBody.Address);
            createEventBody.Address.Lat = cords.Lat;
            createEventBody.Address.Lon = cords.Lon;

            Event newEvent = new Event()
            {
                Name = createEventBody.Name,
                Description = createEventBody.Description,
                Address = createEventBody.Address,
                isCanceled = false,
                Price = createEventBody.Price,
                IsOnline = createEventBody.IsOnline,
                EndLocal = createEventBody.EndLocal,
                EndUTC = createEventBody.EndLocal.ToUniversalTime(),
                StartLocal = createEventBody.StartLocal,
                StartUTC = createEventBody.StartLocal.ToUniversalTime(),
                Host = host,
                Images = createEventBody.Images.Select(x => Mapper.Map<Image>(x)).ToList(),
                Thumbnail = createEventBody.Thumbnail != null
                    ? Mapper.Map<Image>(createEventBody.Thumbnail)
                    : new Image(),
                EventCategories = createEventBody.Categories != null
                    ? createEventBody.Categories.Select(c => new EventCategory() {CategoryId = c.Id}).ToList()
                    : new List<EventCategory>(),
                SocialLinks = createEventBody.SocialLinks != null
                    ? createEventBody.SocialLinks.Select(x => Mapper.Map<SocialLink>(x)).ToList()
                    : new List<SocialLink>(),
                Tickets = Enumerable.Repeat(new Ticket(), createEventBody.NumberOfTickets).ToList()
            };

            try
            {
                // Saving user to Db.
                await ApplicationContext.Events.AddAsync(newEvent);
                await ApplicationContext.SaveChangesAsync();
                return Mapper.Map<EventViewModel>(newEvent);
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        private async Task<CoordinateAbbreviated> GetAddressCords(Address address)
        {
            var addressAsString = $"{address.AddressLine1},{address.City},{address.CountryName},{address.PostalCode}";
            Uri uri = new Uri(
                $"https://atlas.microsoft.com/search/address/json?&subscription-key={AppSettings.AzureMapsKey}&api-version=1.0&language=en-US&query=${addressAsString}");

            var response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentStream = await response.Content.ReadAsStreamAsync();

                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                try
                {
                    var searchResponse = serializer.Deserialize<SearchAddressResponse>(jsonReader);
                    if (searchResponse == null)
                    {
                        throw new AddressNotFoundException();
                    }

                    return searchResponse.Results.First().Position;
                }
                catch (JsonReaderException)
                {
                    throw new JsonException();
                }
            }

            throw new AddressNotFoundException();
        }

        public async Task Cancel(Guid id)
        {
            var e = await GetEvent(id);

            if (e == null)
            {
                Logger.LogInformation("Event not found");
                throw new EventNotFoundException();
            }

            e.isCanceled = true;

            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("Event canceled");
            }
            catch
            {
                Logger.LogWarning("Event failed to save");
                throw;
            }
        }

        private async Task<Event> GetEvent(Guid id)
        {
            return await ApplicationContext.Events.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<EventHostModel>> GetAllHosts(Guid hostId)
        {
            var events = ApplicationContext.Events.Include(x => x.EventCategories).ThenInclude(x => x.Category).Include(x => x.Tickets).Include(x => x.Host)
                .AsSplitQuery().AsNoTracking().AsEnumerable();

            events = events.Where(x => x.Host.Id == hostId && !x.isCanceled);

            // var fullEvents = events.ToList();
            
            return events.Select(
                x => new EventHostModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Address = x.Address,
                    Categories = x.EventCategories.Any() ? x.EventCategories.Select(c => Mapper.Map<CategoryViewModel>(c.Category)).ToList() : new List<CategoryViewModel>(),
                    Price = x.Price,
                    Thumbnail = x.Thumbnail.Source != null ? Mapper.Map<ImageViewModel>(x.Thumbnail) : null,
                    Images = x.Images.Any()
                        ? x.Images.Select(i => Mapper.Map<ImageViewModel>(i)).ToList()
                        : new List<ImageViewModel>(),
                    Tickets = x.Tickets.Any()
                        ? x.Tickets.Select(t => Mapper.Map<TicketViewModel>(t)).ToList()
                        : new List<TicketViewModel>(),
                    EndLocal = x.EndLocal,
                    IsOnline = x.IsOnline,
                    SocialLinks = x.SocialLinks.Any()
                        ? x.SocialLinks.Select(s => Mapper.Map<SocialLinkViewModel>(s)).ToList()
                        : new List<SocialLinkViewModel>(),
                    StartLocal = x.StartLocal,
                    TicketsLeft = x.Tickets.Count,
                    EndUTC = x.EndUTC,
                    StartUTC = x.StartUTC
                }).ToList();
        }

        public async Task<EventDetailModel> GetForPublic(Guid id)
        {
            var e = await ApplicationContext.Events
                .Include(x => x.Address)
                .Include(x => x.EventCategories)
                .Include(x => x.Images)
                .Include(x => x.Thumbnail)
                .Include(x => x.SocialLinks)
                .Include(x => x.Tickets).Select(x =>
                    new EventDetailModel()
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Name = x.Name,
                        Description = x.Description,
                        Categories = x.EventCategories.Select(c => Mapper.Map<CategoryViewModel>(c.Category)).ToList(),
                        Images = x.Images.Select(i => Mapper.Map<ImageViewModel>(i)).ToList(),
                        Price = x.Price,
                        Thumbnail = Mapper.Map<ImageViewModel>(x.Thumbnail),
                        EndLocal = x.EndLocal,
                        IsOnline = x.IsOnline,
                        SocialLinks = x.SocialLinks.Select(s => Mapper.Map<SocialLinkViewModel>(s)).ToList(),
                        StartLocal = x.StartLocal,
                        TicketsLeft = x.Tickets.Count,
                        EndUTC = x.EndUTC,
                        StartUTC = x.StartUTC
                    }).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
            {
                Logger.LogInformation("Event not fond");
                throw new EventNotFoundException();
            }

            return e;
        }

        public async Task<List<EventViewModel>> Search(string keyword, List<SearchFilter> filters)
        {
            List<Guid> searchCategories = filters.Where(x => x.Key == SearchParam.Category)
                .Select(x => Guid.Parse(x.Value)).ToList();
            var isOnline = filters.Find(x => x.Key == SearchParam.IsOnline) != null;
            var cords = filters.Find(x => x.Key == SearchParam.Location)?.Value.Split(",").Select(double.Parse)
                .ToArray();
            var date = filters.Find(x => x.Key == SearchParam.Date)?.Value;
            // var date = isDate != null ? DateTime.Parse(isDate): (DateTime?) null;

            // TODO: this search is not explicit does it want to be?
            // TODO: order of the result?
            // var events = await ApplicationContext.Events.Where(
            //     x => (
            //              (searchCategories.Any() ? x.EventCategories.Any(c => searchCategories.Contains(c.CategoryId)) : true)) 
            //          && (keyword != null ? x.Name.Contains(keyword) : true) && (isOnline ? x.IsOnline : true) 
            //          // && (EventDistance(x,cords))
            //          // && (cords != null && x.IsOnline ? false : true)
            //          && !x.isCanceled
            //          ).Take(50).ToListAsync();

            // var events = ApplicationContext.Events.AsEnumerable().Where(x =>
            // {
            //     var containsKeyword = keyword != null ? x.Name.Contains(keyword) : true;
            //     var hasCategory = searchCategories.Any() ? x.EventCategories.Any(c => searchCategories.Contains(c.CategoryId)) : true;
            //     // var isClose = cords != null ? EventDistance(x.Address.Lat, x.Address.Lon, cords) : true;
            //     var isClose = true;
            //     var online = (isOnline ? x.IsOnline : true);
            //     return isClose && containsKeyword && hasCategory && online && !x.isCanceled;
            // }).Take(50);

            IEnumerable<Event> events = ApplicationContext.Events.Include(x => x.EventCategories).AsSplitQuery()
                .AsEnumerable();

            if (isOnline)
            {
                events = events.Where(x => x.IsOnline);
            }

            if (searchCategories.Any())
            {
                events = events.Where(x => x.EventCategories.Any(c => searchCategories.Contains(c.CategoryId)));
            }

            if (cords != null)
            {
                events = events.Where(x => !x.IsOnline && EventDistance(x.Address.Lat, x.Address.Lon, cords));
            }

            if (date != null)
            {
                var realDate = DateTime.Parse(date);
                events = events.Where(x =>
                    new DateTime(x.StartLocal.Year, x.StartLocal.Month, x.StartLocal.Day).Equals(new DateTime(
                        realDate.Year,
                        realDate.Month, realDate.Day)));
            }

            return events.Select(e => Mapper.Map<EventViewModel>(e)).Take(50).ToList();
        }

        private static bool EventDistance(double lat, double lon, double[] cords)
        {
            return CalculateDistance.Calculate(new Location()
            {
                Latitude = lat,
                Longitude = lon
            }, new Location()
            {
                Latitude = cords[0],
                Longitude = cords[1]
            }) < cords[2];
        }

        public Task<List<Category>> GetAllCategories()
        {
            return ApplicationContext.Categories.ToListAsync();
        }

        public async Task<ActionResult<EventHostModel>> GetForHost(Guid id)
        {
            var e = await ApplicationContext.Events.Include(x => x.Tickets).Select(
                x => new EventHostModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Address = x.Address,
                    Categories = x.EventCategories.Select(c => Mapper.Map<CategoryViewModel>(c.Category)).ToList(),
                    Price = x.Price,
                    Thumbnail = Mapper.Map<ImageViewModel>(x.Thumbnail),
                    Images = x.Images.Select(i => Mapper.Map<ImageViewModel>(i)).ToList(),
                    Tickets = x.Tickets.Select(t => Mapper.Map<TicketViewModel>(t)).ToList(),
                    EndLocal = x.EndLocal,
                    IsOnline = x.IsOnline,
                    SocialLinks = x.SocialLinks.Select(s => Mapper.Map<SocialLinkViewModel>(s)).ToList(),
                    StartLocal = x.StartLocal,
                    TicketsLeft = x.Tickets.Count,
                    EndUTC = x.EndUTC,
                    StartUTC = x.StartUTC
                }).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
            {
                Logger.LogInformation("Event not fond");
                throw new EventNotFoundException();
            }

            return e;
        }

        public async Task Update(UpdateEventBody updateEventBody)
        {
            var e = await ApplicationContext.Events.Include(x => x.EventCategories)
                .FirstOrDefaultAsync(x => x.Id == updateEventBody.Id);

            if (e == null)
            {
                Logger.LogInformation("Event not fond");
                throw new EventNotFoundException();
            }

            e.Name = updateEventBody.Name;
            e.Description = updateEventBody.Description;
            e.Price = updateEventBody.Price;
            e.IsOnline = updateEventBody.IsOnline;
            e.Images = updateEventBody.SocialLinks != null
                ? updateEventBody.Images.Select(x => Mapper.Map<Image>(x)).ToList()
                : null;
            e.Thumbnail = updateEventBody.Thumbnail != null
                ? Mapper.Map<Image>(updateEventBody.Thumbnail)
                : new Image();
            e.SocialLinks = updateEventBody.SocialLinks != null
                ? updateEventBody.SocialLinks.Select(x => Mapper.Map<SocialLink>(x)).ToList()
                : new List<SocialLink>();
            e.StartLocal = updateEventBody.StartLocal;
            e.EndLocal = updateEventBody.EndLocal;
            e.StartUTC = updateEventBody.StartLocal.ToUniversalTime();
            e.EndUTC = updateEventBody.EndLocal.ToUniversalTime();
            e.Address = updateEventBody.Address;
            e.EventCategories = updateEventBody.Categories != null
                ? updateEventBody.Categories.Select(c => new EventCategory() {CategoryId = c.Id}).ToList()
                : new List<EventCategory>();

            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("Event updated");
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }
    }
}