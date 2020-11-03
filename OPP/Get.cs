using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace OPP
{
    public class Get : IGetRequest
    {
        string endpoint;
        List<string> args;
        public Get(string endpoint, List<string> args)
        {
            this.endpoint = endpoint;
            this.args = args;
        }
        public string SendRequest()
        {
            HttpClient client = new HttpClient(); 
            return client.GetStringAsync(endpoint + string.Concat(args)).Result;
        }
        public string GetEndpoint()
        {
            return endpoint;
        }
        public List<string> GetArgs()
        {
            return args;
        }
    }
}
