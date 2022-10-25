using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson19.CrossLib.Keys
{
    public class JwtOptions : IJwtOptions
    {
        public byte[] Key => Encoding.UTF8.GetBytes("Key-Must-Be-at-least-32-bytes-in-length!");

        public string Issuer => nameof(JwtOptions);

        public string Audience => nameof(JwtOptions);
    }
}
