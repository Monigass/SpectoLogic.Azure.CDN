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
using System.Collections.Generic;
using System.Linq;

namespace SpectoLogic.Azure.CDN
{
    public class MediaConfig
    {
        public const string MediaConfigSectionName = "mediaConfig";

        public string BaseUrl { get; set; }
        public string Key { get; set; }

        private List<MediaConfigPolicy> _policies = new List<MediaConfigPolicy>();

        public List<MediaConfigPolicy> Policies
        {
            get { return _policies; }
            set { _policies = value; }
        }

        public MediaConfigPolicy GetPolicyByName(string name)
        {
            return Policies.Where(p => p.Name == name).FirstOrDefault();
        }
    }
}
