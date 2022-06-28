using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models
{
    public class Response
    {
        public Response()
        {
            Messages = new Dictionary<string, string>();
            Warnings = new Dictionary<int, string>();
        }
        public int Code { get; set; }
        public int WarningCode { get; set; } = 0;
        public Dictionary<string, string> Messages { get; set; }
        public Dictionary<int, string> Warnings { get; set; }
        private string Error { get; set; }
        public void FormatError(string key, params string[] list)
        {
            for (int i = 0; i < list.Length; i += 2)
            {
                Error = Error.Replace(list[i], list[i + 1]);
            }
            this.Messages.Add(key, Error);
        }

        public void FormatError(string err, string key, params string[] list)
        {
            Error = err;
            for (int i = 0; i < list.Length; i += 2)
            {
                Error = Error.Replace(list[i], list[i + 1]);
            }
            if (!this.Messages.ContainsKey(key))
            {
                this.Messages.Add(key, Error);
            }
            
            Error = "";
        }

        public void FormatWarnings(string warn, int key)
        {
            if (!this.Warnings.ContainsKey(key))
            {
                this.Warnings.Add(key, warn);
            }
        }

    }
}
