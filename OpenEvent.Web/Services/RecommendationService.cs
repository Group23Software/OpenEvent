using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Recommendation;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class RecommendationService : IRecommendationService
    {
        private readonly ILogger<RecommendationService> Logger;
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IPopularityService PopularityService;
        private readonly IWorkQueue WorkQueue;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="workQueue"></param>
        /// <param name="popularityService"></param>
        public RecommendationService(ILogger<RecommendationService> logger, IServiceScopeFactory serviceScopeFactory,
            IWorkQueue workQueue, IPopularityService popularityService)
        {
            Logger = logger;
            ScopeFactory = serviceScopeFactory;
            WorkQueue = workQueue;
            PopularityService = popularityService;
        }
        
        /// <inheritdoc />
        public async Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, Guid eventId,
            Influence influence, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Influencing");
                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var categories = context.Categories.Where(x => x.Events.Any(e => e.EventId == eventId)).ToList();
                var user = await context.Users
                    .Include(x => x.RecommendationScores).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    Logger.LogInformation("Couldn't find user when influencing");
                    return;
                }

                UpdateRecommendations(user, categories, context);

                await SaveAsync(context);

                Logger.LogInformation("Influenced");
            }
        }
        
        /// <inheritdoc />
        public async Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, string keyword,
            List<SearchFilter> searchFilters, DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Influencing");

                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var user = await context.Users
                    .Include(x => x.RecommendationScores).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                var searchCategories = searchFilters
                    .Where(x => x.Key == SearchParam.Category)
                    .Select(x => Guid.Parse(x.Value)).ToList();

                if (!searchCategories.Any()) return;

                var categories = await context.Categories.Where(x => searchCategories.Contains(x.Id)).ToListAsync();

                foreach (var category in categories)
                {
                    WorkQueue.QueueWork(token =>
                        PopularityService.PopulariseCategory(token, category.Id, DateTime.Now));
                }

                if (user == null)
                {
                    Logger.LogInformation("Couldn't find user when influencing");
                    return;
                }

                UpdateRecommendations(user, categories, context);

                await SaveAsync(context);

                Logger.LogInformation("Influenced");
            }
        }

        // calculates the new recommendation score weight
        private double CalculateWeight(double weight, Influence influence)
        {
            double multiplier = ((double) influence / 1000) + 1;
            weight *= multiplier;
            Logger.LogInformation("Updated weight {Weight}, {Multiplier}", weight, multiplier);
            return weight;
        }

        // Updates all users recommendation scores for categories in the list
        private void UpdateRecommendations(User user, List<Category> categories, ApplicationContext context)
        {
            if (user.RecommendationScores == null) return;

            categories.ForEach(c =>
            {
                var recommendationScore = user.RecommendationScores.FirstOrDefault(x => x.Category.Id == c.Id);

                if (recommendationScore != null)
                    recommendationScore.Weight =
                        CalculateWeight(recommendationScore.Weight, Influence.Search);
            });
        }

        // save the tracked context changes to database
        private async Task SaveAsync(ApplicationContext context)
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
            }
            finally
            {
                context = default;
            }
        }
    }
}