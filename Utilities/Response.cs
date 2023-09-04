using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace CartApi.Utilities
{
    public class Response<T> where T : class
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }


    }
}
