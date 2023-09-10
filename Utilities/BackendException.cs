using System;

namespace CartApi.Utilities
{
    public class BackendException : Exception
    {
        public override string Message { get; }
        public int Code { get; }
        public BackendException(string message, int code)
        {
            Message = message;
            Code = code;
            
        }
    }
}
