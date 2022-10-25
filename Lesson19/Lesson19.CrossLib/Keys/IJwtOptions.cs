using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lesson19.CrossLib.Keys
{
    public interface IJwtOptions
    {
        public byte[] Key { get; }
        public string Issuer { get; }
        public string Audience { get; }
    }
}
