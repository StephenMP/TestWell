using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using TestWell.Environment;
using TestWell.Test.Resource.Objects;
using Xunit;

namespace TestWell.Test.Component.Environment
{
    public class TestingEnvironmentTests : IDisposable
    {
        #region Private Fields

        private bool disposedValue;
        private TestWellEnvironment testEnvironment;

        #endregion Private Fields

        #region Public Methods

        [Fact]
        public void CanNewUpTestingEnvironment()
        {
            var options = new TestWellEnvironmentBuilder();
            this.testEnvironment = options.BuildEnvironment();
            Assert.NotNull(testEnvironment);
        }

        [Fact]
        public void CanNewUpTestingEnvironmentUsingChaining()
        {
            var mongoPath = PlatformServices.Default.Application.ApplicationBasePath;
            var hasTestEnvironmentsFolder = false;
            while (!hasTestEnvironmentsFolder)
            {
                mongoPath = Path.GetFullPath(Path.Combine(mongoPath, @"..\"));
                var dirs = Directory.GetDirectories(mongoPath);

                foreach (var dir in dirs)
                {
                    if (dir.Contains("TestWell.Test.Resource.Environments"))
                    {
                        hasTestEnvironmentsFolder = true;
                        break;
                    }
                }
            }

            this.testEnvironment = new TestWellEnvironmentBuilder()
                                        .AddMongo(Path.Combine(mongoPath, @"TestWell.Test.Resource.Environments\Mongo\bin\mongod.exe"))
                                        .AddSqlContext<TestDbContext>()
                                        .AddResourceWebService<TestWell.Test.Resource.Api.Startup>()
                                        .BuildEnvironment();

            Assert.NotNull(this.testEnvironment);

            Assert.NotNull(this.testEnvironment.GetMongoClient());

            Assert.NotNull(this.testEnvironment.GetSqlDbContextOptions<TestDbContext>());
            Assert.IsType<DbContextOptions<TestDbContext>>(this.testEnvironment.GetSqlDbContextOptions<TestDbContext>());

            Assert.NotNull(this.testEnvironment.GetResourceWebService<TestWell.Test.Resource.Api.Startup>());
            Assert.NotNull(this.testEnvironment.GetResourceWebService<TestWell.Test.Resource.Api.Startup>().BaseUri);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.testEnvironment?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
