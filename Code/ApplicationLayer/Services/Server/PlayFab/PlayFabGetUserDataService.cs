using System;
using System.Collections.Generic;
using ApplicationLayer.Services.Server.Gateways.ServerData;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using SystemUtilities;

namespace ApplicationLayer.Services.Server.PlayFab
{
    public class PlayFabGetUserDataService : IGetDataService
    {
        public UniTask<Optional<DataResult>> Get(List<string> keys)
        {
            var taskCompletionSource = new UniTaskCompletionSource<Optional<DataResult>>();

            RequestUserData(keys, taskCompletionSource);

            return UniTask.Create(() => taskCompletionSource.Task);
        }

        private void OnSuccess(GetUserDataResult result, UniTaskCompletionSource<Optional<DataResult>> t)
        {
            var resultData = new Dictionary<string, string>(result.Data.Count);
            foreach (var userDataRecord in result.Data)
            {
                resultData.Add(userDataRecord.Key, userDataRecord.Value.Value);
            }

            t.TrySetResult(new Optional<DataResult>(new DataResult(resultData)));
        }

        private void OnError(PlayFabError error, UniTaskCompletionSource<Optional<DataResult>> t)
        {
            t.TrySetResult(null);
            throw new Exception(error.ErrorMessage);
        }

        private void RequestUserData(List<string> keys, UniTaskCompletionSource<Optional<DataResult>> taskCompletionSource)
        {
            var request = new GetUserDataRequest {Keys = keys};
            PlayFabClientAPI.GetUserData(request,
                result => OnSuccess(result, taskCompletionSource),
                error => OnError(error, taskCompletionSource));
        }
    }
}