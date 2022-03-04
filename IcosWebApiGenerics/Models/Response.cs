using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApi.Models
{
    public class Response
    {
        public Response()
        {
            Messages = new Dictionary<string, string>();
        }
        public int Code { get; set; }
        //public string Message { get; set; }
        public Dictionary<string, string> Messages { get; set; }
    }
}
