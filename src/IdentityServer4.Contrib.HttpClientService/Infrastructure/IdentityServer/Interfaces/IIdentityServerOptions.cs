using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Contrib.HttpClientService.Models
{
    /// <summary>
    /// Contract that all IdentityServer options (e.g. <see cref="ClientCredentialOptions"/> or <see cref="PasswordOptions"/>) must implement.
    /// </summary>
    public interface IIdentityServerOptions
    {
    }
}
