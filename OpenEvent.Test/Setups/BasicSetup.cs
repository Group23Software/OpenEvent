using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.User;
using System.Threading;
using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Services;

namespace OpenEvent.Test.Setups
{
    public class BasicSetup
    {
        internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestDbAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestDbAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestDbAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute(expression));
            }

            public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute<TResult>(expression));
            }
        }

        internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
        {
            public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            {
            }

            public TestDbAsyncEnumerable(Expression expression)
                : base(expression)
            {
            }

            public IDbAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
            {
                return GetAsyncEnumerator();
            }

            IQueryProvider IQueryable.Provider
            {
                get { return new TestDbAsyncQueryProvider<T>(this); }
            }
        }

        internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestDbAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public void Dispose()
            {
                _inner.Dispose();
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_inner.MoveNext());
            }

            public T Current
            {
                get { return _inner.Current; }
            }

            object IDbAsyncEnumerator.Current
            {
                get { return Current; }
            }
        }

        public async Task<DbContextMock<ApplicationContext>> Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "openEvent").Options;
            
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );


            List<User> seedUserList = new List<User>
            {
                new User
                {
                    Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                    Email = "exists@email.co.uk",
                    UserName = "ExistingUser",
                    FirstName = "exists",
                    LastName = "already",
                    PhoneNumber = "0000000000",
                    Avatar = new Byte[] {1, 1, 1, 1}
                }
            };

            seedUserList[0].Password = hasher.HashPassword(seedUserList[0], "Password");

                
            IQueryable<User> seedUsers = seedUserList.AsQueryable();

            
            var mockContext = new DbContextMock<ApplicationContext>(dbContextOptions);
            var userDbSetMock = mockContext.CreateDbSetMock(x => x.Users, seedUsers);

            // var mockSet = new Mock<DbSet<User>>();
            //
            // mockSet.As<IDbAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator())
            //     .Returns(new TestDbAsyncEnumerator<User>(seedUsers.GetEnumerator()));
            //
            // mockSet.As<IQueryable<User>>().Setup(m => m.Provider)
            //     .Returns(new TestDbAsyncQueryProvider<User>(seedUsers.Provider));
            //
            // mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(seedUsers.Expression);
            // mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(seedUsers.ElementType);
            // mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(seedUsers.GetEnumerator());
            //
            // var mockContext = new Mock<ApplicationContext>(dbContextOptions);
            // mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            return mockContext;

            // await using var context = new DbContextMock<ApplicationContext>(dbContextOptions);
            // Console.WriteLine("Starting data seed");
            // await BasicSeed.SeedAsync(context);
            // return new ApplicationContext(dbContextOptions);
        }
    }
}