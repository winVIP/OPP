using System;
using System.Collections.Generic;
using System.Text;

namespace OPP
{
    public interface IGetRequest
    {
        string SendRequest();
        string GetEndpoint();
        List<string> GetArgs();
    }
}
