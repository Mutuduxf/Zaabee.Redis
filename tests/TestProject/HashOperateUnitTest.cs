using System;
using System.Linq;
using Xunit;
using Zaabee.StackExchangeRedis;
using Zaabee.StackExchangeRedis.Abstractions;
using Zaabee.StackExchangeRedis.Protobuf;

namespace TestProject
{
    public class HashOperateUnitTest
    {
        private readonly IZaabeeRedisClient _client =
            new ZaabeeRedisClient(new RedisConfig("192.168.5.10:7001,192.168.5.10:7002,192.168.5.10:7003,192.168.5.10:7004,192.168.5.10:7005,192.168.5.10:7006,abortConnect=false,syncTimeout=3000"),
                new Serializer());

        [Fact]
        public void HashSync()
        {
            var testModel = TestModelFactory.CreateTestModel();
            Assert.True(_client.HashAdd("HashTest", testModel.Id.ToString(), testModel));
            var result = _client.HashGet<TestModel>("HashTest", testModel.Id.ToString());
            Assert.Equal(testModel, result);
            Assert.True(_client.HashDelete("HashTest", result.Id.ToString()));
        }

        [Fact]
        public async void HashAsync()
        {
            var testModel = TestModelFactory.CreateTestModel();
            Assert.True(await _client.HashAddAsync("HashAsyncTest", testModel.Id.ToString(), testModel));
            var result = await _client.HashGetAsync<TestModel>("HashAsyncTest", testModel.Id.ToString());
            Assert.Equal(testModel, result);
            Assert.True(_client.HashDelete("HashAsyncTest", result.Id.ToString()));
        }

        [Fact]
        public void HashBatchSync()
        {
            var testModels = Enumerable.Range(0, 10).Select(p => TestModelFactory.CreateTestModel()).ToList();
            _client.HashAddRange("HashBatchTest",
                testModels.Select(testModel => new Tuple<string, TestModel>(testModel.Id.ToString(), testModel))
                    .ToList());
            var results =
                _client.HashGetRange<TestModel>("HashBatchTest",
                    testModels.Select(model => model.Id.ToString()).ToList());
            Assert.True(results.All(result => testModels.Any(model => model.Equals(result))));
            Assert.Equal(results.Count,
                _client.HashDeleteRange("HashBatchTest",
                    results.Select(testModel => testModel.Id.ToString()).ToList()));
        }

        [Fact]
        public async void HashBatchAsync()
        {
            var testModels = Enumerable.Range(0, 10).Select(p => TestModelFactory.CreateTestModel()).ToList();
            _client.HashAddRangeAsync("HashBatchAsyncTest",
                testModels.Select(testModel => new Tuple<string, TestModel>(testModel.Id.ToString(), testModel))
                    .ToList()).Wait();
            var results = await _client.HashGetRangeAsync<TestModel>("HashBatchAsyncTest",
                testModels.Select(model => model.Id.ToString()).ToList());
            Assert.True(results.All(result => testModels.Any(model => model.Equals(result))));
            Assert.Equal(results.Count,
                await _client.HashDeleteRangeAsync("HashBatchAsyncTest",
                    results.Select(testModel => testModel.Id.ToString()).ToList()));
        }

        [Fact]
        public void HashAllSync()
        {
            var testModels = Enumerable.Range(0, 10).Select(p => TestModelFactory.CreateTestModel()).ToList();
            _client.HashAddRange("HashAllTest",
                testModels.Select(testModel => new Tuple<string, TestModel>(testModel.Id.ToString(), testModel))
                    .ToList());
            var results = _client.HashGet<TestModel>("HashAllTest");
            Assert.True(results.All(result => testModels.Any(model => model.Equals(result))));
            var keys = _client.HashGetAllEntityKeys("HashAllTest");
            Assert.True(keys.All(key => testModels.Any(model => model.Id.ToString() == key)));
            Assert.Equal(results.Count, _client.HashCount("HashAllTest"));
            Assert.True(_client.Delete("HashAllTest"));
        }

        [Fact]
        public async void HashAllAsync()
        {
            var testModels = Enumerable.Range(0, 10).Select(p => TestModelFactory.CreateTestModel()).ToList();
            _client.HashAddRangeAsync("HashAllAsyncTest",
                testModels.Select(testModel => new Tuple<string, TestModel>(testModel.Id.ToString(), testModel))
                    .ToList()).Wait();
            var results = await _client.HashGetAsync<TestModel>("HashAllAsyncTest");
            Assert.True(results.All(result => testModels.Any(model => model.Equals(result))));
            var keys = await _client.HashGetAllEntityKeysAsync("HashAllAsyncTest");
            Assert.True(keys.All(key => testModels.Any(model => model.Id.ToString() == key)));
            Assert.Equal(results.Count, await _client.HashCountAsync("HashAllAsyncTest"));
            Assert.True(await _client.DeleteAsync("HashAllAsyncTest"));
        }
    }
}