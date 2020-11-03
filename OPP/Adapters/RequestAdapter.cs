using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using OPP;

namespace OPP
{
    public class RequestAdapter : IPostRequest, IGetRequest  // Class adapter (two-ways)
    {
        IPostRequest postRequest;
        IGetRequest getRequest;
        public RequestAdapter(IPostRequest postRequest)
        {
            this.postRequest = postRequest;
        }

        public RequestAdapter(IGetRequest getRequest)
        {
            this.getRequest = getRequest;
        }

        public List<string> GetArgs()
        {
            if (getRequest != null)
            {
                return getRequest.GetArgs();
            }
            else return null;
        }

        public string GetContent()
        {
            if (postRequest != null)
            {
                return postRequest.GetContent();
            }
            else return null;
        }

        public string GetContentType()
        {
            if (postRequest != null)
            {
                return postRequest.GetContentType();
            }
            else return null;
        }

        public string GetEndpoint()
        {
            if(postRequest == null)
            {
                return getRequest.GetEndpoint();
            }
            else
            {
                return postRequest.GetEndpoint();
            }
        }

        public string SendRequest()
        {
            HttpClient client = new HttpClient();
            if (postRequest == null) // Jeigu adaptuojam Get Requesta
            {
                var args = getRequest.GetArgs();
                var content = "{";
                args.ForEach((a) =>
                {
                    content += '"' + args.IndexOf(a) + '"' + ':' + '"' + a + '"' + ',';
                });
                content = content.Remove(content.Length-1);
                content += "}";
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                return client.PostAsync(getRequest.GetEndpoint(), stringContent).ToString();
            }
            else if (getRequest == null) // Jeigu adaptuojam Post Requesta
            {
                var content = postRequest.GetContent();
                content = content.Replace('{', ' ');
                content = content.Replace('}', ' ');
                content = content.Replace('"', ' ');
                content = content.Replace(':', ',');
                var clean = content.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
                List<string> args = new List<string>();
                for (int i = 0; i < clean.Length; i++)
                {
                    if (i % 2 == 0) args.Add(clean[i]);
                }
                return client.GetStringAsync(postRequest.GetEndpoint() + string.Concat(args)).Result;
            }
            else
            {
                return "error";
            }
        }
    }
}
