using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.HttpClientService.SampleProject.ProtectedResourceServices.Dto
{
    /// <summary>
    /// An object representing the result of the protected resource https://demo.identityserver.io/api/test
    /// </summary>
    public class ProtectedResourceResponseDto
    {
        /// <summary>
        /// The type property of the result
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value property of the result
        /// </summary>
        public string Value { get; set; }
    }
}
