using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODOs_API.Data
{
    public interface IJsonWebTokenAuthenticationManager
    {
        public string Authenticate(string username);
    }
}
