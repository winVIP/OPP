using System;
using System.Collections.Generic;
using System.Text;

namespace OPP
{
    public interface IPostRequest
    {
        string SendRequest();
        string GetEndpoint();
        string GetContent();
        string GetContentType();
    }
}
