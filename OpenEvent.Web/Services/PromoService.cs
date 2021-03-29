using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Promo;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class PromoService : IPromoService
    {
        private readonly ILogger<PromoService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicationContext"></param>
        /// <param name="mapper"></param>
        public PromoService(ILogger<PromoService> logger, ApplicationContext applicationContext, IMapper mapper)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
        }

        /// <inheritdoc />
        /// <exception cref="EventNotFoundException">Thrown if the event is not found</exception>
        /// <exception cref="InvalidPromoException">Thrown if the promo dates are not valid</exception>
        public async Task<PromoViewModel> Create(CreatePromoBody createPromoBody)
        {
            var e = await ApplicationContext.Events.Include(x => x.Promos).AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == createPromoBody.EventId);

            if (e == null) throw new EventNotFoundException();

            // check if the promo conflicts with existing promos
            if (e.Promos.Any(p => p.Start >= createPromoBody.Start && createPromoBody.End <= e.EndLocal))
            {
                throw new InvalidPromoException();
            }

            Promo promo = new Promo
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

        /// <inheritdoc />
        /// <exception cref="PromoNotFoundException">Thrown if the promo is not found</exception>
        public async Task<PromoViewModel> Update(UpdatePromoBody updatePromoBody)
        {
            var promo = await ApplicationContext.Promos.FirstOrDefaultAsync(x => x.Id == updatePromoBody.Id);

            if (promo == null) throw new PromoNotFoundException();

            // sets values to new values
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

        /// <inheritdoc />
        /// <exception cref="PromoNotFoundException">Thrown if the promo is not found</exception>
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