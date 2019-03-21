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

using System;

namespace VerizonDigital.CDN.TokenProvider
{
    public class MediaConfigPolicy
    {
        public enum RestrictIPAddressMode
        {
            None,
            Request
        }

        public string Name { get; set; } = "default";

        public TimeSpan ExpirationTimeSpan { get; set; } = new TimeSpan(0, 30, 0);

        public RestrictIPAddressMode RestrictIPAddress { get; set; } = RestrictIPAddressMode.None;

        public string AllowedUrls { get; set; } = null;

        public string AllowedCountries { get; set; } = null;

        public string DeniedCountries { get; set; } = null;

        public string AllowedReferers { get; set; } = null;

        public string DeniedReferers { get; set; } = null;

        public string AllowedProtocol { get; set; } = null;

        public string DeniedProtocol { get; set; } = null;
    }
}
