using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
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
        Task<List<EventViewModel>> Search(); //TODO: Search params

        Task<List<Category>> GetAllCategories();
    }

    public class EventService : IEventService
    {
        private readonly ILogger<EventService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        public EventService(ApplicationContext context, ILogger<EventService> logger, IMapper mapper)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
        }

        public async Task<EventViewModel> Create(CreateEventBody createEventBody)
        {
            var host = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == createEventBody.HostId);

            if (host == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }
            
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
                Thumbnail = createEventBody.Thumbnail != null ? Mapper.Map<Image>(createEventBody.Thumbnail): new Image(),
                EventCategories = createEventBody.Categories != null ? createEventBody.Categories.Select(c => new EventCategory(){CategoryId = c.Id}).ToList() : new List<EventCategory>(),
                SocialLinks = createEventBody.SocialLinks != null ? createEventBody.SocialLinks.Select(x => Mapper.Map<SocialLink>(x)).ToList() : new List<SocialLink>(),
                Tickets = Enumerable.Repeat(new Ticket(),createEventBody.NumberOfTickets).ToList()
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
            var events = await ApplicationContext.Events.Include(x => x.Tickets).Where(x => x.Host.Id == hostId).Select(x => new EventHostModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Address = x.Address,
                Categories = x.EventCategories.Select(c => Mapper.Map<CategoryViewModel>(c.Category)).ToList(),
                Price = x.Price,
                Thumbnail = x.Thumbnail,
                Images = x.Images.Select(i => Mapper.Map<ImageViewModel>(i)).ToList(),
                Tickets = x.Tickets.Select(t => Mapper.Map<TicketViewModel>(t)).ToList(),
                EndLocal = x.EndLocal,
                IsOnline = x.IsOnline,
                SocialLinks = x.SocialLinks.Select(s => Mapper.Map<SocialLinkViewModel>(s)).ToList(),
                StartLocal = x.StartLocal,
                TicketsLeft = x.TicketsLeft,
                EndUTC = x.EndUTC,
                StartUTC = x.StartUTC
            }).AsNoTracking().ToListAsync();
            return events;
        }

        public async Task<EventDetailModel> GetForPublic(Guid id)
        {
            var e = await ApplicationContext.Events.Include(x => x.Tickets).Select(x => new EventDetailModel()
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
                TicketsLeft = x.TicketsLeft,
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

        public Task<List<EventViewModel>> Search()
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetAllCategories()
        {
            return ApplicationContext.Categories.ToListAsync();
        }
    }
}