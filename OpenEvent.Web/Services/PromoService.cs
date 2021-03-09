using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Promo;

namespace OpenEvent.Web.Services
{
    public interface IPromoService
    {
        Task<PromoViewModel> Create(CreatePromoBody createPromoBody);
        Task<PromoViewModel> Update(UpdatePromoBody updatePromoBody);
        Task Destroy(Guid id);
    }

    public class PromoService : IPromoService
    {
        private readonly ILogger<PromoService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        public PromoService(ILogger<PromoService> logger, ApplicationContext applicationContext, IMapper mapper)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
        }

        public async Task<PromoViewModel> Create(CreatePromoBody createPromoBody)
        {
            var e = await ApplicationContext.Events.FirstOrDefaultAsync(x => x.Id == createPromoBody.EventId);

            if (e == null) throw new EventNotFoundException();

            Promo promo = new Promo()
            {
                Start = createPromoBody.Start,
                End = createPromoBody.End,
                Active = createPromoBody.Active,
                Discount = createPromoBody.Discount,
                Event = e
            };

            try
            {
                await ApplicationContext.Promos.AddAsync(promo);
                await ApplicationContext.SaveChangesAsync();

                return Mapper.Map<PromoViewModel>(promo);
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Create exception {Exception}", ex.ToString());
                throw;
            }
        }

        public async Task<PromoViewModel> Update(UpdatePromoBody updatePromoBody)
        {
            var promo = await ApplicationContext.Promos.FirstOrDefaultAsync(x => x.Id == updatePromoBody.Id);

            if (promo == null) throw new PromoNotFoundException();

            promo.Active = updatePromoBody.Active;
            promo.Discount = updatePromoBody.Discount;
            promo.End = updatePromoBody.End;
            promo.Start = updatePromoBody.Start;

            try
            {
                await ApplicationContext.SaveChangesAsync();

                return Mapper.Map<PromoViewModel>(promo);
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Update exception {Exception}", ex.ToString());
                throw;
            }
        }

        public async Task Destroy(Guid id)
        {
            var promo = await ApplicationContext.Promos.FirstOrDefaultAsync(x => x.Id == id);

            if (promo == null) throw new PromoNotFoundException();

            try
            {
                ApplicationContext.Promos.Remove(promo);
                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogInformation("Destroy exception {Exception}", ex.ToString());
                throw;
            }
        }
    }
}