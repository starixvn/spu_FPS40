using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Auth
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}
