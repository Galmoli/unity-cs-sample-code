using Domain.UseCases.Lobby.Profile;

namespace InterfaceAdapters.Lobby.Profile.Username
{
    public class UsernamePresenter : IUsernameOutput
    {
        private readonly UsernameViewModel _model;

        public UsernamePresenter(UsernameViewModel model)
        {
            _model = model;
        }
        public void UpdateUsername(string username)
        {
            _model.Username.Value = username;
        }
    }
}