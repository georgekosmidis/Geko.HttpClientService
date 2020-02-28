namespace IdentityServer4.HttpClientService
{
    /// <summary>
    /// An abstraction of the factory that creates <see cref="HttpClientService"/> instances for a given logical name.
    /// </summary>
    public interface IHttpClientServiceFactory
    {
        /// <summary>
        /// Creates new <see cref="HttpClientService"/> instances. 
        /// Prefer <see cref="CreateHttpClientService(string)"/> when possible, to avoid 'sockets exhaustion' issues caused by HttpClient.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </summary>
        /// <remarks>
        /// Prefer <see cref="CreateHttpClientService(string)"/> when possible, to avoid 'sockets exhaustion'.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </remarks>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        HttpClientService CreateHttpClientService();

        /// <summary>
        /// Creates new <see cref="HttpClientService"/> instances.
        /// </summary>
        /// <remarks>
        /// Prefer <see cref="CreateHttpClientService(string)"/> when possible, to avoid 'sockets exhaustion'.
        /// Read more here https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net-core
        /// </remarks>
        /// <returns>An <see cref="HttpClientService"/> instance.</returns>
        HttpClientService CreateHttpClientService(string name);
    }
}