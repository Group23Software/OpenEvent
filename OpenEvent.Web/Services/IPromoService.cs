using System;
using System.Threading.Tasks;
using OpenEvent.Web.Models.Promo;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service for all promo logic
    /// </summary>
    public interface IPromoService
    {
        /// <summary>
        /// Creates a new promo for an event
        /// </summary>
        /// <param name="createPromoBody"><see cref="CreatePromoBody"/></param>
        /// <returns>
        /// Returns the new promo created
        /// </returns>
        Task<PromoViewModel> Create(CreatePromoBody createPromoBody);

        /// <summary>
        /// Updates a promo
        /// </summary>
        /// <param name="updatePromoBody"><see cref="UpdatePromoBody"/></param>
        /// <returns>
        /// Returns the updated promo
        /// </returns>
        Task<PromoViewModel> Update(UpdatePromoBody updatePromoBody);

        /// <summary>
        /// Destroys promo
        /// </summary>
        /// <param name="id">Promo id</param>
        /// <returns>Completed task when destroyed</returns>
        Task Destroy(Guid id);
    }
}