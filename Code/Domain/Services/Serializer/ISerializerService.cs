﻿namespace Domain.Services.Serializer
{
    public interface ISerializerService
    {
        string Serialize<T>(T data);
        T Deserialize<T>(string data);
    }
}