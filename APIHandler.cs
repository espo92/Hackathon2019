using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackathonCorebot
{
    public class APIHandler
    {
        public HttpClient Api { get; set; } = new HttpClient();

        public APIHandler()
        {
            Api.BaseAddress = new Uri("https://feqa-192-a1.devops.lcl/19_2-RestAPI/api/");
        }

        public async Task<Dictionary<string,string>> GetEmpById(string id)
        {
            HttpResponseMessage response = Api.GetAsync(string.Format("humanresources/employees/{0}", id)).Result;

            try
            {
                response.EnsureSuccessStatusCode();

                Dictionary<string, string> dic = new Dictionary<string, string>();
                JObject jContent = JObject.Parse(await response.Content.ReadAsStringAsync());

                foreach (JProperty prop in jContent.Properties())
                {
                    dic.Add(prop.Name, prop.Value.ToString());
                }

                return dic;
            }
            catch
            {
                return default;
            }
        }
    }
}
