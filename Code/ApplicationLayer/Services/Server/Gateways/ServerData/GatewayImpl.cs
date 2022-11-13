using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Domain.Services.Serializer;
using Domain.Services.Server;
using UnityEngine.Assertions;
using View.Login;

namespace ApplicationLayer.Services.Server.Gateways.ServerData
{
    public abstract class GatewayImpl : IGateway, IDataPreLoaderService
    {
        private Dictionary<string, string> _rawData;
        private Dictionary<Type, string> _typeToKey;
        private Dictionary<Type, IDto> _parsedData;
        private readonly Dictionary<string, string> _dirtyData;

        private readonly ISerializerService _serializerService;
        private readonly IGetDataService _getDataService;
        private readonly ISetDataService _setDataService;

        private bool _isInitialized;

        protected GatewayImpl(ISerializerService serializerService,
            IGetDataService getDataService,
            ISetDataService setDataService)
        {
            _serializerService = serializerService;
            _getDataService = getDataService;
            _setDataService = setDataService;
            _dirtyData = new Dictionary<string, string>();
        }

        protected abstract void InitializeTypeToKey(out Dictionary<Type, string> _typeToKey);

        public async UniTask PreLoad()
        {
            InitializeTypeToKey(out _typeToKey);

            var optional = await _getDataService.Get(new List<string>());
            var containsAllKeys = _typeToKey.Select(typeToKey => optional.Get.Data.ContainsKey(typeToKey.Value)).All(hasKey => hasKey);
            
            while (!containsAllKeys)
            {
                LoginMessageLogger.instance.CreatingUser();
                await UniTask.Delay(2000);
                optional = await _getDataService.Get(new List<string>());
                containsAllKeys = _typeToKey.Select(typeToKey => optional.Get.Data.ContainsKey(typeToKey.Value)).All(hasKey => hasKey);
            }

            optional
                .IfPresent(result =>
                {
                    _rawData = result.Data;
                    _parsedData = new Dictionary<Type, IDto>(_rawData.Count);
                    _isInitialized = true;
                })
                .OrElseThrow(new Exception("Error initializing gateway data"));
        }

        public T Get<T>() where T : IDto
        {
            Assert.IsTrue(_isInitialized, "Gateway is not initialized");

            var type = typeof(T);

            if (_parsedData.TryGetValue(type, out var result))
            {
                return (T) result;
            }

            var key = _typeToKey[type];
            var data = _rawData[key];

            var dto = _serializerService.Deserialize<T>(data);
            _parsedData.Add(type, dto);

            return dto;
        }

        public bool Contains<T>() where T : IDto
        {
            Assert.IsTrue(_isInitialized, "Gateway is not initialized");

            var type = typeof(T);
            var key = _typeToKey[type];
            return _rawData.ContainsKey(key);
        }

        public void Set<T>(T data) where T : IDto
        {
            Assert.IsTrue(_isInitialized, "Gateway is not initialized");

            var type = typeof(T);
            var key = _typeToKey[type];
            if (_dirtyData.ContainsKey(key))
            {
                throw new Exception($"This key {type} is already dirty");
            }

            var serializedData = _serializerService.Serialize(data);
            SetRawData(key, serializedData);
            SetParsedData(data, type);
            _dirtyData.Add(key, serializedData);
        }

        private void SetRawData(string key, string serializedData)
        {
            if (_rawData.ContainsKey(key))
            {
                _rawData[key] = serializedData;
            }
            else
            {
                _rawData.Add(key, serializedData);
            }
        }

        private void SetParsedData<T>(T data, Type type) where T : IDto
        {
            if (!_parsedData.ContainsKey(type))
            {
                _parsedData.Add(type, data);
            }
            else
            {
                _parsedData[type] = data;
            }
        }

        public UniTask Save()
        {
            Assert.IsTrue(_isInitialized, "Gateway is not initialized");

            var task = _setDataService.Set(_dirtyData);
            _dirtyData.Clear();
            return task;
        }
    }
}