using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Error
{
        public class YapidromException : Exception
        {
        public int Code { get; }
        public int HttpStatusCode { get; }

        public YapidromException(ErrorCodes errorCodes) : base(errorCodes.Message)
        {
            Code = errorCodes.Code;
            HttpStatusCode = errorCodes.HttpStatusCode;
        }
    }
}
