namespace IdentityServer4.Contrib.HttpClientService.Models
{

    /// <summary>
    ///  of the client credentials options for IdentityServer4
    /// </summary>
    public class DefaultClientCredentialOptions
    {
        /// <summary>
        /// The address of the access token service.
        /// (e.g. https://demo.identityserver.io/connect/token)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The scopes.
        /// </summary>
        public string Scopes { get; set; }
    }
}
