using System;
using ApplicationLayer.Services.Server.Gateways.ServerData;

namespace ApplicationLayer.Services.Server.Dtos
{
    [Serializable]
    public class UsernameDto : IDto
    {
        public string username;
    }
}