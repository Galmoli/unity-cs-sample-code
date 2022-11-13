using Domain.DataAccess;

namespace Domain.UseCases.Lobby.Profile
{
    public class GetPlayerUsernameUseCase : IGetPlayerUsername
    {
        private readonly IUserDataAccess _userRepository;
        private readonly IUsernameOutput _usernameOutput;

        public GetPlayerUsernameUseCase(IUserDataAccess userRepository, IUsernameOutput usernameOutput)
        {
            _userRepository = userRepository;
            _usernameOutput = usernameOutput;
        }
        
        public void GetUsername()
        {
            _usernameOutput.UpdateUsername(_userRepository.GetLocalUser().Username);
        }
    }
}