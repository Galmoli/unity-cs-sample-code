using Cysharp.Threading.Tasks;

namespace ApplicationLayer.Services.Server.Gateways.ServerData
{
    public interface IGateway
    {
        T Get<T>() where T : IDto;
        bool Contains<T>() where T : IDto;
        void Set<T>(T data) where T : IDto;
        UniTask Save();
    }
}