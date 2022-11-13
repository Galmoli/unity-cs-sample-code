using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Domain.Services.Server
{
    public interface ISetDataService
    {
        UniTask Set(Dictionary<string, string> _data);
    }
}