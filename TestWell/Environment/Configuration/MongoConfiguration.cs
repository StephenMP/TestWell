using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWell.Mongo;

namespace TestWell.Environment.Configuration
{
    public class MongoConfiguration : IDisposable
    {
        #region Private Fields

        private readonly MongoTestServer mongoServer;
        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public MongoConfiguration(string pathToMongoDExe)
        {
            this.mongoServer = new MongoTestServer(TcpPortUtility.GetFreeTcpPort(), pathToMongoDExe);
            this.MongoClient = mongoServer.Database.Client;
        }

        #endregion Public Constructors

        #region Public Properties

        public IMongoClient MongoClient { get; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task ImportDataAsync<TEntity>(string dbName, string collectionName, IEnumerable<TEntity> data)
        {
            await this.MongoClient.GetDatabase(dbName).GetCollection<TEntity>(collectionName).InsertManyAsync(data);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.mongoServer?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
