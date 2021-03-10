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
    public enum Influence
    {
        PageView = 200,
        Search = 100,
        Purchase = 500,
        Verify = 400,
        DownVote = -100
    }

    public interface IRecommendationService
    {
        Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, Guid eventId, Influence influence,
            DateTime created);

        Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, string keyword,
            List<SearchFilter> searchFilters, DateTime created);
    }

    public class RecommendationService : IRecommendationService
    {
        private readonly ILogger<RecommendationService> Logger;
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IPopularityService PopularityService;
        private readonly IWorkQueue WorkQueue;

        public RecommendationService(ILogger<RecommendationService> logger, IServiceScopeFactory serviceScopeFactory,
            IWorkQueue workQueue, IPopularityService popularityService)
        {
            Logger = logger;
            ScopeFactory = serviceScopeFactory;
            WorkQueue = workQueue;
            PopularityService = popularityService;
        }

        public async Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, Guid eventId,
            Influence influence,
            DateTime created)
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

        public async Task InfluenceAsync(CancellationToken cancellationToken, Guid userId, string keyword,
            List<SearchFilter> searchFilters,
            DateTime created)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogInformation("Influencing");

                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var user = await context.Users
                    .Include(x => x.RecommendationScores).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    Logger.LogInformation("Couldn't find user when influencing");
                    return;
                }

                var searchCategories = searchFilters
                    .Where(x => x.Key == SearchParam.Category)
                    .Select(x => Guid.Parse(x.Value)).ToList();

                if (!searchCategories.Any()) return;

                var categories = await context.Categories.Where(x => searchCategories.Contains(x.Id)).ToListAsync();

                UpdateRecommendations(user, categories, context);

                await SaveAsync(context);

                Logger.LogInformation("Influenced");
            }
        }

        private double CalculateWeight(double weight, Influence influence)
        {
            double multiplier = ((double) influence / 1000) + 1;
            weight *= multiplier;
            Logger.LogInformation($"Updated weight {weight}, {multiplier}");
            return weight;
        }

        private void UpdateRecommendations(User user, List<Category> categories, ApplicationContext context)
        {
            if (user.RecommendationScores == null) user.RecommendationScores = new List<RecommendationScore>();
            categories.ForEach(c =>
            {
                WorkQueue.QueueWork(token => PopularityService.PopulariseCategory(token, c.Id, DateTime.Now));

                var recommendationScore = user.RecommendationScores.FirstOrDefault(x => x.Category.Id == c.Id);

                if (recommendationScore == null)
                {
                    recommendationScore = new RecommendationScore()
                    {
                        User = user,
                        Category = c,
                        Weight = 0
                    };
                    context.RecommendationScores.Add(recommendationScore);
                }
                else
                {
                    recommendationScore.Weight =
                        CalculateWeight(recommendationScore.Weight, Services.Influence.Search);
                }
            });
        }

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