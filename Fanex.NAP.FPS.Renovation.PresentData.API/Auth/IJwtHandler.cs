using System;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Auth
{
    public interface IJwtHandler
    {
        JsonWebToken Create(Guid userId);
    }
}
