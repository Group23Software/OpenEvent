using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NUnit.Framework;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Event;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Test
{
    [TestFixture]
    public class Migrations
    {
        private ApplicationContext Context;

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        private static class SeedTestData
        {
            public static void Seed(ApplicationContext context)
            {
                User user = TestData.TestUserData.FakeUser.Generate();
                Event e = TestData.TestEventData.FakeEvent.Generate();

                context.Add(user);
                context.AddAsync(e);
                context.SaveChanges();
            }
        }

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options;

            Context = new ApplicationContext(options);
        }

        [Test]
        public async Task Should_Migrate_And_Seed()
        {
            await Context.Database.EnsureDeletedAsync();
            var migrator = Context.Database.GetService<IMigrator>();
            var migrations = Context.Database.GetMigrations();
            var migrationCount = (await Context.Database.GetPendingMigrationsAsync()).Count();
            var migrationCounter = migrationCount;
            foreach (var migration in migrations)
            {
                await migrator.MigrateAsync(migration);
                (await Context.Database.GetPendingMigrationsAsync()).Count().Should().Be(-- migrationCounter);
            }
            (await Context.Database.GetAppliedMigrationsAsync()).Count().Should().Be(migrationCount);
            
            // await Context.Database.MigrateAsync();
            // var migrationsApplied = await Context.Database.GetAppliedMigrationsAsync();
            // migrationsApplied.Count().Should().BeGreaterThan(0);
            // (await Context.Database.CanConnectAsync()).Should().BeTrue();
            // SeedTestData.Seed(Context);
            // (await Context.Events.FirstAsync()).Should().BeOfType<Event>().And.NotBeNull();
            // (await Context.Users.FirstAsync()).Should().BeOfType<User>().And.NotBeNull();
        }

        // [Test]
        // public async Task Model_Should_Match_Context()
        // {
        //     await Context.Database.EnsureCreatedAsync();
        // var migrationAssembly = Context.Database.GetService<IMigrationsAssembly>();
        // var differences = Context.Database.GetService<IMigrationsModelDiffer>();
        // differences.HasDifferences(migrationAssembly.ModelSnapshot.Model.GetRelationalModel(),
        //     // Context.Model.GetRelationalModel());
        //
        //     var dependencies = Context.GetService<ProviderConventionSetBuilderDependencies>();
        //     var relationalDependencies = Context.GetService<RelationalConventionSetBuilderDependencies>();
        //
        //     var hasDifferences = false;
        //
        //     if (migrationAssembly.ModelSnapshot != null)
        //     {
        //         var typeMappingConvention = new TypeMappingConvention(dependencies);
        //         typeMappingConvention.ProcessModelFinalizing(
        //             ((IConventionModel) migrationAssembly.ModelSnapshot.Model).Builder, null);
        //
        //         var relationalModelConvention = new RelationalModelConvention(dependencies, relationalDependencies);
        //         var sourceModel =
        //             relationalModelConvention.ProcessModelFinalized(migrationAssembly.ModelSnapshot.Model);
        //
        //         hasDifferences = differences.HasDifferences(
        //             ((IMutableModel) sourceModel).FinalizeModel().GetRelationalModel(),
        //             Context.Model.GetRelationalModel());
        //
        //         var whatDifferences = differences.GetDifferences(
        //             ((IMutableModel) sourceModel).FinalizeModel().GetRelationalModel(),
        //             Context.Model.GetRelationalModel());
        //     }
        // }
    }
}