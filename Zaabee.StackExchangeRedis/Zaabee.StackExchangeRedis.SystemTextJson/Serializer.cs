﻿using Zaabee.StackExchangeRedis.ISerialize;
using Zaabee.SystemTextJson;

namespace Zaabee.StackExchangeRedis.SystemTextJson
{
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T o) =>
            o == null ? new byte[0] : o.ToBytes();

        public T Deserialize<T>(byte[] bytes) =>
            bytes is null || bytes.Length == 0 ? default(T) : bytes.FromBytes<T>();
    }
}