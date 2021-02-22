using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.Recommendation;
using Serilog;

namespace OpenEvent.Web.Services
{
    public enum Influence
    {
        PageView = 1,
        Search = 0,
        Purchase = 1,
        Verify = 1
    }

    public interface IRecommendationService
    {
        void Influence(Guid? userId, Guid eventId, Influence influence);
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
                        Weight = 1
                    };
                    user.RecommendationScores.Add(recommendationScore);
                }
                else
                {
                    recommendationScore.Weight *= (int) influence;
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
                        recommendationScore.Weight *= (int) Services.Influence.Search;
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
                        recommendationScore.Weight *= (int) influence;
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