using Domain.UseCases.Lobby.Profile;

namespace InterfaceAdapters.Lobby.Profile.Username
{
    public class UsernameController
    {
        private ISetPlayerUsername _setPlayerUsernameUseCase;

        public void InjectSetPlayerUsernameUseCase(ISetPlayerUsername setPlayerUsernameUseCase)
        {
            _setPlayerUsernameUseCase = setPlayerUsernameUseCase;
        }

        public void SetUsername(string username)
        {
            _setPlayerUsernameUseCase.SetUsername(username);
        }
    }
}