using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace Zaabee.StackExchangeRedis
{
    public partial class ZaabeeRedisClient
    {
        public bool SetAdd<T>(string key, T value)
        {
            return _db.SetAdd(key, (RedisValue) _serializer.Serialize(value));
        }

        public long SetAddRange<T>(string key, IEnumerable<T> values)
        {
            return _db.SetAdd(key, values.Select(value => (RedisValue) _serializer.Serialize(value)).ToArray());
        }

        public IList<T> SetCombineUnion<T>(string firstKey, string secondKey)
        {
            var values = _db.SetCombine(SetOperation.Union, firstKey, secondKey);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public IList<T> SetCombineUnion<T>(IEnumerable<string> keys)
        {
            var values = _db.SetCombine(SetOperation.Union, keys.Select(key => (RedisKey) key).ToArray());
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public IList<T> SetCombineIntersect<T>(string firstKey, string secondKey)
        {
            var values = _db.SetCombine(SetOperation.Intersect, firstKey, secondKey);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public IList<T> SetCombineIntersect<T>(IEnumerable<string> keys)
        {
            var values = _db.SetCombine(SetOperation.Intersect, keys.Select(key => (RedisKey) key).ToArray());
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public IList<T> SetCombineDifference<T>(string firstKey, string secondKey)
        {
            var values = _db.SetCombine(SetOperation.Difference, firstKey, secondKey);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public IList<T> SetCombineDifference<T>(IEnumerable<string> keys)
        {
            var values = _db.SetCombine(SetOperation.Difference, keys.Select(key => (RedisKey) key).ToArray());
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public long SetCombineAndStoreUnion<T>(string destination, string firstKey, string secondKey)
        {
            return _db.SetCombineAndStore(SetOperation.Union, destination, firstKey, secondKey);
        }

        public long SetCombineAndStoreUnion<T>(string destination, IEnumerable<string> keys)
        {
            return _db.SetCombineAndStore(SetOperation.Union, destination,
                keys.Select(key => (RedisKey) key).ToArray());
        }

        public long SetCombineAndStoreIntersect<T>(string destination, string firstKey, string secondKey)
        {
            return _db.SetCombineAndStore(SetOperation.Intersect, destination, firstKey, secondKey);
        }

        public long SetCombineAndStoreIntersect<T>(string destination, IEnumerable<string> keys)
        {
            return _db.SetCombineAndStore(SetOperation.Intersect, destination,
                keys.Select(key => (RedisKey) key).ToArray());
        }

        public long SetCombineAndStoreDifference<T>(string destination, string firstKey, string secondKey)
        {
            return _db.SetCombineAndStore(SetOperation.Difference, destination, firstKey, secondKey);
        }

        public long SetCombineAndStoreDifference<T>(string destination, IEnumerable<string> keys)
        {
            return _db.SetCombineAndStore(SetOperation.Difference, destination,
                keys.Select(key => (RedisKey) key).ToArray());
        }

        public bool SetContains<T>(string key, T value)
        {
            return _db.SetContains(key, _serializer.Serialize(value));
        }

        public long SetLength<T>(string key)
        {
            return _db.SetLength(key);
        }

        public IList<T> SetMembers<T>(string key)
        {
            return _db.SetMembers(key).Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T))
                .ToList();
        }

        public bool SetMove<T>(string source, string destination, T value)
        {
            return _db.SetMove(source, destination, _serializer.Serialize(value));
        }

        public T SetPop<T>(string key)
        {
            var value = _db.SetPop(key);
            return value.HasValue ? _serializer.Deserialize<T>(value) : default(T);
        }

        public IList<T> SetPop<T>(string key, long count)
        {
            var values = _db.SetPop(key, count);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public T SetRandomMember<T>(string key)
        {
            var value = _db.SetRandomMember(key);
            return value.HasValue ? _serializer.Deserialize<T>(value) : default(T);
        }

        public IList<T> SetRandomMembers<T>(string key, long count)
        {
            var values = _db.SetRandomMembers(key, count);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }

        public bool SetRemove<T>(string key, T value)
        {
            return _db.SetRemove(key, _serializer.Serialize(value));
        }

        public long SetRemoveRange<T>(string key, IEnumerable<T> values)
        {
            return _db.SetRemove(key, values.Select(value => (RedisValue) _serializer.Serialize(value)).ToArray());
        }

        public IList<T> SetScan<T>(string key, T pattern = default(T), int pageSize = 10, long cursor = 0,
            int pageOffset = 0)
        {
            var values = _db.SetScan(key, _serializer.Serialize(pattern), pageSize, cursor, pageOffset);
            return values.Select(value => value.HasValue ? _serializer.Deserialize<T>(value) : default(T)).ToList();
        }
    }
}