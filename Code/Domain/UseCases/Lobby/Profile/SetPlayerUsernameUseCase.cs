using System.Collections.Generic;
using Domain.DataAccess;

namespace Domain.UseCases.Lobby.Profile
{
    public class SetPlayerUsernameUseCase : ISetPlayerUsername
    {
        private readonly IUserDataAccess _userRepository;
        private readonly IGetPlayerUsername _getPlayerUsernameUseCase;

        public SetPlayerUsernameUseCase(IUserDataAccess userRepository, IGetPlayerUsername getPlayerUsernameUseCase)
        {
            _userRepository = userRepository;
            _getPlayerUsernameUseCase = getPlayerUsernameUseCase;
        }

        public void SetUsername(string username)
        {
            _userRepository.EditUsername(username);
            _getPlayerUsernameUseCase.GetUsername();
        }
    }
}