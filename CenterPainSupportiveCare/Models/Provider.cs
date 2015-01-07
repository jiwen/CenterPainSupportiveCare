using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CenterPainSupportiveCare.Models
{
    public class Provider
    {
        public Provider()
        {

        }

        public Provider(CenterPainSupportiveCareModels.Provider provider)
        {
            Id = provider.ProviderId.ToString();
            ProviderName = provider.ProviderName;
        }

        public string Id { get; set; }
        public string ProviderName { get; set; }
    }
}