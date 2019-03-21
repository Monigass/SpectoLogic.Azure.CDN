/**
 * Copyright (C) 2019 Monigass
 * 
 * Copyright (C) 2017 SpectoLogic
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using VerizonDigital.CDN.TokenProvider.Security;
using static VerizonDigital.CDN.TokenProvider.MediaConfigPolicy;

namespace VerizonDigital.CDN.TokenProvider
{
    public class CDNTokenProvider : ICDNTokenProvider
    {
        private readonly HttpContext _currentContext;
        private readonly MediaConfig _config;

        public CDNTokenProvider(IOptions<MediaConfig> config)
        {
            _config = config.Value;
        }

        public CDNTokenProvider(IOptions<MediaConfig> config,
            IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _currentContext = httpContextAccessor.HttpContext;
        }

        public string NewToken()
        {
            return NewToken("default");
        }

        public string NewToken(string policyName)
        {
            var policy = _config.GetPolicyByName(policyName)
                ?? new MediaConfigPolicy();

            var tokenBuilder = new TokenBuilder();

            string clientIPAddress = null;

            if (policy.RestrictIPAddress == RestrictIPAddressMode.Request
                && _currentContext != null)
            {
                clientIPAddress = _currentContext.Connection.RemoteIpAddress.ToString();
            }

            return tokenBuilder.EncryptV3(_config.Key,
                policy.ExpirationTimeSpan,
                clientIPAddress, policy.AllowedCountries,
                policy.DeniedCountries, policy.AllowedReferers,
                policy.DeniedReferers, policy.AllowedProtocol,
                policy.DeniedProtocol, policy.AllowedUrls);
        }
    }
}
