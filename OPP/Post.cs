using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace OPP
{
    public class Post : IPostRequest
    {
        string endpoint;
        string content;
        string type;
        public Post(string endpoint, string content, string type)
        {
            this.endpoint = endpoint;
            this.content = content;
            this.type = type;
        }
        public string SendRequest()
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            string responseString = client.PostAsync(endpoint, stringContent).ToString();
            return responseString;
        }
        public string GetEndpoint()
        {
            return endpoint;
        }
        public string GetContent()
        {
            return content;
        }

        public string GetContentType()
        {
            return type;
        }
    }
}
