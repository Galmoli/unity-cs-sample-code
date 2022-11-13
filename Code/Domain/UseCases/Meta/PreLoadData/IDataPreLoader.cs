using Cysharp.Threading.Tasks;

namespace Domain.UseCases.Meta.PreLoadData
{
    public interface IDataPreLoader
    {
        UniTask PreLoad();
    }
}