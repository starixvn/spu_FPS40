using System;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId);
    }
}
