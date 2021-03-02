using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
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
        Verify = 400
    }

    public interface IRecommendationService
    {
        void Influence(Guid? userId, Guid eventId, Influence influence);
        void Influence(User user, Event e, Influence influence);
        void Influence(Guid? userId, string keyword, List<SearchFilter> searchFilters);
    }

    public class RecommendationService : IRecommendationService
    {
        private readonly ILogger<RecommendationService> Logger;
        private readonly IServiceScopeFactory ScopeFactory;

        public RecommendationService(ILogger<RecommendationService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            Logger = logger;
            ScopeFactory = serviceScopeFactory;
        }

        private double CalculateWeight(double weight, Influence influence)
        {
            double multiplier = ((double) influence / 1000) + 1;
            weight *= multiplier;
            Logger.LogInformation($"Updated weight {weight}, {multiplier}");
            return weight;
        }

        public void Influence(Guid userId, Guid categoryId, Influence influence)
        {
            Logger.LogInformation("Influencing");
            Task.Run(async () =>
            {
                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var user = await context.Users.Include(x => x.RecommendationScores)
                    .FirstOrDefaultAsync(x => x.Id == userId);
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

                if (user == null || category == null) return;

                var recommendationScore = user.RecommendationScores.FirstOrDefault(x => x.Category.Id == categoryId);

                if (recommendationScore == null)
                {
                    recommendationScore = new RecommendationScore()
                    {
                        Category = category,
                        User = user,
                        Weight = 0.1
                    };
                    user.RecommendationScores.Add(recommendationScore);
                }
                else
                {
                    recommendationScore.Weight = CalculateWeight(recommendationScore.Weight, influence);
                }

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

                Logger.LogInformation("Influenced");
            });
        }

        public void Influence(User user, Event e, Influence influence)
        {
            throw new NotImplementedException();
        }

        public void Influence(Guid? userId, string keyword, List<SearchFilter> searchFilters)
        {
            Logger.LogInformation("Influencing");
            Task.Run(async () =>
            {
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

                if (user.RecommendationScores == null) user.RecommendationScores = new List<RecommendationScore>();
                categories.ForEach(c =>
                {
                    var recommendationScore = user.RecommendationScores.FirstOrDefault(x => x.Category.Id == c.Id);

                    if (recommendationScore == null)
                    {
                        recommendationScore = new RecommendationScore()
                        {
                            User = user,
                            Category = c,
                            Weight = 1
                        };
                        context.RecommendationScores.Add(recommendationScore);
                    }
                    else
                    {
                        recommendationScore.Weight =
                            CalculateWeight(recommendationScore.Weight, Services.Influence.Search);
                    }
                });

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

                Logger.LogInformation("Influenced");
            });
        }

        public void Influence(Guid? userId, Guid eventId, Influence influence)
        {
            Logger.LogInformation("Influencing");
            Task.Run(async () =>
            {
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

                if (user.RecommendationScores == null) user.RecommendationScores = new List<RecommendationScore>();
                categories.ForEach(c =>
                {
                    var recommendationScore = user.RecommendationScores.FirstOrDefault(x => x.Category.Id == c.Id);

                    if (recommendationScore == null)
                    {
                        recommendationScore = new RecommendationScore()
                        {
                            User = user,
                            Category = c,
                            Weight = 1
                        };
                        context.RecommendationScores.Add(recommendationScore);
                    }
                    else
                    {
                        recommendationScore.Weight = CalculateWeight(recommendationScore.Weight, influence);
                    }
                });

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

                Logger.LogInformation("Influenced");
            });
        }
    }
}