using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestWell.Environment.Configuration;
using Xunit;

namespace TestWell.Test.Component.Environment.Configuration
{
    public class ResourceWebServiceConfigurationSteps : IDisposable
    {
        #region Private Fields

        private bool disposedValue;
        private ResourceWebServiceConfiguration resourceWebServiceConfiguration;

        private HttpResponseMessage webServiceResponse;

        #endregion Private Fields

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void GivenIHaveAResourceWebServiceConfiguration()
        {
            this.resourceWebServiceConfiguration = new ResourceWebServiceConfiguration();
        }

        internal void GivenIHaveAWebService()
        {
            this.resourceWebServiceConfiguration.AddWebService<TestWell.Test.Resource.Api.Startup>();
        }

        internal void ThenICanVerifyICanAddAResourceWebService()
        {
            var webService = this.resourceWebServiceConfiguration.WebService<TestWell.Test.Resource.Api.Startup>();
            Assert.NotNull(webService);
            Assert.NotNull(webService.BaseUri);
        }

        internal void ThenICanVerifyICanNewUpAResourceWebServiceConfiguration()
        {
            Assert.NotNull(this.resourceWebServiceConfiguration);
        }

        internal async Task ThenICanVerifyICanUseWebServiceAsync()
        {
            Assert.NotNull(this.webServiceResponse);
            Assert.True(this.webServiceResponse.IsSuccessStatusCode);

            var content = await this.webServiceResponse.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("value1", content);
            Assert.Contains("value2", content);
        }

        internal void WhenIAddAResourceWebService()
        {
            this.resourceWebServiceConfiguration.AddWebService<TestWell.Test.Resource.Api.Startup>();
        }

        internal async Task WhenIUseWebServiceAsync()
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{resourceWebServiceConfiguration.WebService<TestWell.Test.Resource.Api.Startup>().BaseUri.AbsoluteUri.TrimEnd('/')}/api/values";
                this.webServiceResponse = await client.GetAsync(requestUri);
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.resourceWebServiceConfiguration?.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}
