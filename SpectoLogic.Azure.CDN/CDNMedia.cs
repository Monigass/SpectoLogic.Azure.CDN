/**
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
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SpectoLogic.Azure.CDN.Security;

namespace SpectoLogic.Azure.CDN
{
    public class CDNMedia : IMediaUrlProvider
    {
        private MediaConfig _config = null;
        private HttpContext _currentContext = null;
        public CDNMedia(IOptions<MediaConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _currentContext = httpContextAccessor.HttpContext;
        }

        public string Token(string policyName = null)
        {
            MediaConfigPolicy policy = _config.GetPolicyByName(policyName);
            if (policy == null) policy = new MediaConfigPolicy(); // Create a default policy
            TokenBuilder tokenBuilder = new TokenBuilder();
            if (policy.ExpirationTimeSpan == null) policy.ExpirationTimeSpan = new TimeSpan(0, 5, 0);

            string clientIPAddress = null;
            if (policy.RestrictIPAddress == MediaConfigPolicy.RestrictIPAddressMode.Request)
            {
                clientIPAddress = _currentContext.Connection.RemoteIpAddress.ToString();
            }
            return tokenBuilder.EncryptV3(_config.Key, policy.ExpirationTimeSpan.Value, clientIPAddress, policy.AllowedCountries,
                policy.DeniedCountries, policy.AllowedReferers, policy.DeniedReferers, policy.AllowedProtocol, policy.DeniedProtocol, policy.AllowedUrls);
        }

        public string Url(string partialUrl)

        {
            return this.Url(partialUrl, null);
        }

        public string Url(string partialUrl, string policyName)
        {
            string token = this.Token(policyName);
            if (!_config.BaseUrl.EndsWith("/")) _config.BaseUrl += "/";
            if (partialUrl.StartsWith("/")) partialUrl = partialUrl.Substring(1);
            string url = _config.BaseUrl+partialUrl;
            url += "?" + token;
            Uri result = new Uri(url);
            return result.ToString();
        }
    }
}
