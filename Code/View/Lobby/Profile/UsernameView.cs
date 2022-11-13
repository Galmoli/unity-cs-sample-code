using InterfaceAdapters.Lobby.Profile.Username;
using TMPro;
using UniRx;
using UnityEngine;

namespace View.Lobby.Profile
{
    public class UsernameView : ViewBase
    {
        [SerializeField] private TMP_Text usernameText;
        [SerializeField] private TMP_Text editedUsernameText;
        [SerializeField] private TMP_Text placeholderText;
        private UsernameViewModel _model;
        private UsernameController _controller;

        public void SetModel(UsernameViewModel model)
        {
            _model = model;
            _model.Username.Subscribe(UpdateUsername).AddTo(_disposables);
        }

        public void SetController(UsernameController controller)
        {
            _controller = controller;
        }

        public void SetUsername()
        {
            _controller.SetUsername(editedUsernameText.text);
        }

        private void UpdateUsername(string username)
        {
            usernameText.text = username;
            placeholderText.text = username;
        }
    }
}