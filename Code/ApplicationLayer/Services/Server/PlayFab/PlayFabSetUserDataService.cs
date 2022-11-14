using System;
using System.Collections.Generic;
using ApplicationLayer.Services.Server.Gateways.ServerData;
using Cysharp.Threading.Tasks;
using Domain.Services.Server;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace ApplicationLayer.Services.Server.PlayFab
{
    public class PlayFabSetUserDataService : ISetDataService
    {
        public UniTask Set(Dictionary<string, string> _data)
        {
            var taskCompletionSource = new UniTaskCompletionSource<bool>();
            
            SetUserData(_data, taskCompletionSource);
            
            return UniTask.Create(() => taskCompletionSource.Task);
        }

        private void OnSuccess(UpdateUserDataResult _result, UniTaskCompletionSource<bool> _t)
        {
            _t.TrySetResult(true);
        }

        private void OnError(PlayFabError _error, UniTaskCompletionSource<bool> t)
        {
            t.TrySetResult(false);
            throw new Exception(_error.ErrorMessage);
        }

        private void SetUserData(Dictionary<string, string> _data, UniTaskCompletionSource<bool> _taskCompletionSource)
        {
            var request = new UpdateUserDataRequest{Data = _data};
            PlayFabClientAPI.UpdateUserData(request, 
                _result => OnSuccess(_result, _taskCompletionSource),
                _error => OnError(_error, _taskCompletionSource));
        }
    }
}