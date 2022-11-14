using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SystemUtilities;

namespace ApplicationLayer.Services.Server.Gateways.ServerData
{
    public interface IGetDataService
    {
        UniTask<Optional<DataResult>> Get(List<string> keys);
    }
}