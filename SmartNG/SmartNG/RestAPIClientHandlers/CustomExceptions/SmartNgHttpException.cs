using System;
using System.Collections.Generic;
using System.Text;

namespace SmartNG.RestAPIClientHandlers.CustomExceptions
{
    class SmartNgHttpException : Exception
    {

        public SmartNgHttpException(string message) : base(message)
        {

        }

    }
}
