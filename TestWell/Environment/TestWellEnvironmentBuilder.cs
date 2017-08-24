using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWell.Environment.Configuration;
using TestWell.WebService;

namespace TestWell.Environment
{
    public class TestWellEnvironmentBuilder : IDisposable
    {
        #region Private Fields

        private bool disposedValue;

        private MongoConfiguration mongoConfiguration;
        private ResourceWebServiceConfiguration resourceWebServiceConfiguration;
        private SqlConfiguration sqlConfiguration;

        #endregion Private Fields

        #region Public Methods

        public TestWellEnvironmentBuilder AddMongo(string pathToMongoDExe)
        {
            this.mongoConfiguration = this.mongoConfiguration ?? new MongoConfiguration(pathToMongoDExe);
            return this;
        }

        public TestWellEnvironmentBuilder AddResourceWebService<TStartup>() where TStartup : class
        {
            this.resourceWebServiceConfiguration = this.resourceWebServiceConfiguration ?? new ResourceWebServiceConfiguration();
            this.resourceWebServiceConfiguration?.AddWebService<TStartup>();
            return this;
        }

        public TestWellEnvironmentBuilder AddSqlContext<TDbContext>() where TDbContext : DbContext
        {
            this.sqlConfiguration = this.sqlConfiguration ?? new SqlConfiguration();
            this.sqlConfiguration?.AddSqlContext<TDbContext>();
            return this;
        }

        public TestWellEnvironment BuildEnvironment()
        {
            return new TestWellEnvironment(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IMongoClient GetMongoClient()
        {
            return this.mongoConfiguration?.MongoClient;
        }

        public IResourceWebService GetResourceWebService<TStartup>() where TStartup : class
        {
            return this.resourceWebServiceConfiguration?.WebService<TStartup>();
        }

        public DbContextOptions<TDbContext> GetSqlDbContextOptions<TDbContext>() where TDbContext : DbContext
        {
            return this.sqlConfiguration?.SqlDbContextOptions<TDbContext>();
        }

        public async Task ImportMongoDataAsync<TEntity>(string dbName, string collectionName, IEnumerable<TEntity> data)
        {
            await this.mongoConfiguration?.ImportDataAsync(dbName, collectionName, data);
        }

        public async Task ImportSqlDataAsync<TDbContext, TEntity>(IEnumerable<TEntity> entities) where TDbContext : DbContext where TEntity : class
        {
            await this.sqlConfiguration?.ImportDataAsync<TDbContext, TEntity>(entities);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.mongoConfiguration?.Dispose();
                    this.resourceWebServiceConfiguration?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
