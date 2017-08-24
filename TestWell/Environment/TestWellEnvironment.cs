using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWell.WebService;

namespace TestWell.Environment
{
    public class TestWellEnvironment : IDisposable
    {
        #region Private Fields

        private readonly TestWellEnvironmentBuilder TestWellEnvironmentBuilder;
        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public TestWellEnvironment(TestWellEnvironmentBuilder TestWellEnvironmentBuilder)
        {
            this.TestWellEnvironmentBuilder = TestWellEnvironmentBuilder;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IMongoClient GetMongoClient()
        {
            return this.TestWellEnvironmentBuilder?.GetMongoClient();
        }

        public IResourceWebService GetResourceWebService<TStartup>() where TStartup : class
        {
            return this.TestWellEnvironmentBuilder?.GetResourceWebService<TStartup>();
        }

        public DbContextOptions<TDbContext> GetSqlDbContextOptions<TDbContext>() where TDbContext : DbContext
        {
            return this.TestWellEnvironmentBuilder?.GetSqlDbContextOptions<TDbContext>();
        }

        public async Task ImportMongoDataAsync<TEntity>(string dbName, string collectionName, IEnumerable<TEntity> data)
        {
            await this.TestWellEnvironmentBuilder?.ImportMongoDataAsync(dbName, collectionName, data);
        }

        public async Task ImportSqlDataAsync<TDbContext, TEntity>(IEnumerable<TEntity> entities) where TDbContext : DbContext where TEntity : class
        {
            await this.TestWellEnvironmentBuilder?.ImportSqlDataAsync<TDbContext, TEntity>(entities);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.TestWellEnvironmentBuilder?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
