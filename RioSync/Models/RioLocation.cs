using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RioSync.Models
{
    public class RioLocation
    {
        [JsonProperty("did")]
        public string Domain { get; set; }
        [JsonProperty("address_1")]
        public string Address1 { get; set; }
        [JsonProperty("address_2")]
        public string Address2 { get; set; }
        [JsonProperty("suite")]
        public string Suite { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("fid")]
        public string StoreNumber { get; set; }
        [JsonProperty("from_email")]
        public string FromEmail { get; set; }
        [JsonProperty("lat")]
        public string Latitude { get; set; }
        [JsonProperty("lng")]
        public string Longitude { get; set; }
        [JsonProperty("local_fax")]
        public string LocalFax { get; set; }
        [JsonProperty("local_phone")]
        public string LocalPhone { get; set; }
        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        [JsonProperty("post_code")]
        public string PostalCode { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("tls_fax")]
        public string TlsFax { get; set; }
        [JsonProperty("tls_phone")]
        public string TldPhone { get; set; }
        [JsonProperty("tls_temp_phone")]
        public string TlsTempPhone { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("isDisabled")]
        public string IsDisabledRaw { get; set; }
        [JsonIgnore]
        public bool IsDisabled
        {
            get
            {
                //we want to default to active
                if (string.IsNullOrWhiteSpace(IsDisabledRaw)) return false;

                return string.Equals(IsDisabledRaw, "1");
            }
        }


    }

}
