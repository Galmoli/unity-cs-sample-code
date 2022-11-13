using Domain.DataAccess;
using InterfaceAdapters.Lobby.Profile.Username;
using UnityEngine;
using View.Lobby.Profile;
using Domain.UseCases.Lobby.Profile;
using SystemUtilities;

namespace UnityConfigurationAdapters.Installers.Lobby.Profile
{
    public class UsernameInstaller : MonoBehaviour
    {
        [SerializeField] private UsernameView usernameView;
        public void Install()
        {
            var userRepository = ServiceLocator.Instance.GetService<IUserDataAccess>();
            
            var usernameController = new UsernameController();
            var usernameViewModel = new UsernameViewModel();
            
            usernameView.SetModel(usernameViewModel);
            usernameView.SetController(usernameController);

            var usernamePresenter = new UsernamePresenter(usernameViewModel);
            
            var getPlayerUsernameUseCase = new GetPlayerUsernameUseCase(userRepository, usernamePresenter);
            var setPlayerUsernameUseCase = new SetPlayerUsernameUseCase(userRepository, getPlayerUsernameUseCase);
            
            usernameController.InjectSetPlayerUsernameUseCase(setPlayerUsernameUseCase);
            getPlayerUsernameUseCase.GetUsername();
        }
    }
}