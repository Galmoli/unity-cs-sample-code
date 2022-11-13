using ApplicationLayer.Services.Server.Dtos;
using ApplicationLayer.Services.Server.Gateways.ServerData;
using Domain.DataAccess;
using Domain.Entities;

namespace ApplicationLayer.DataAccess.Users
{
    public class UserRepository : IUserDataAccess
    {
        private readonly GatewayImpl userDataGateway;
        private User localUser;

        public UserRepository(GatewayImpl _userDataGateway)
        {
            userDataGateway = _userDataGateway;
        }

        public void EditUsername(string _username)
        {
            localUser.Username = _username;
            
            var userNameDto = new UsernameDto {username = _username};
            userDataGateway.Set(userNameDto);
            userDataGateway.Save();
        }
    }
}