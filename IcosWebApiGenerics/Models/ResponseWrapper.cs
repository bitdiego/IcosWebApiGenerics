using IcosWebApiGenerics.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcosWebApiGenerics.Models
{
    public class ResponseWrapper
    {
        private static Response response = null;
        private static string Err = "";
        private static int errorCode = 0;
        private static string Ecosystem { get; set; }

        
        public static void SetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            //return response;
        }

        public static void WrapErrorCodeAndMessages(int errCode, string key, params string[] list)
        {
            SetResponse();
            response.Code += errCode;
            response.FormatError(ErrorCodes.GeneralErrors[errCode], key, list);
        }

        public static Response GetResponse()
        {
            if (response == null)
            {
                response = new Response();
            }
            return response;
        }
    }
}
