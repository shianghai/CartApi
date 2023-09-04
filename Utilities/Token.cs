using Microsoft.AspNetCore.Http;
using System;

namespace CartApi.Utilities
{
    public class Token
    {
        public static string GetTokenFromRequest(HttpRequest request)
        {
            var token = request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(token) || !token.StartsWith("Bearer "))
            {
                throw new Exception("Please specify the correct token format in the header");
            }

            var validToken = token.Substring("Bearer ".Length).Trim();
            return validToken;
        }
    }
}
