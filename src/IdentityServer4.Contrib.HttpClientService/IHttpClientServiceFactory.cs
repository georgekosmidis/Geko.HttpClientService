namespace IdentityServer4.Contrib.HttpClientService
{
    /// <summary>
    /// An abstraction of the factory that creates <see cref="HttpClientService"/> instances for a given logical name.
    /// </summary>
    public interface IHttpClientServiceFactory
    {
        /// <summary>
        /// Creates new <see cref="HttpClientService"/> instances. 
        /// </summary>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        HttpClientService CreateHttpClientService();

    }
}