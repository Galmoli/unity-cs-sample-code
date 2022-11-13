using UniRx;

namespace InterfaceAdapters.Lobby.Profile.Username
{
    public class UsernameViewModel
    {
        public ReactiveProperty<string> Username;

        public UsernameViewModel()
        {
            Username = new StringReactiveProperty();
        }
    }
}