﻿using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.Redis
{
    public static class DatabaseExtensions
    {
        public static void JsonSet<T>(this IDatabase redis, RedisKey key, T value, TimeSpan? expireDate)
        {
            string json = JsonConvert.SerializeObject(value);

            redis.StringSet(key, json, expireDate);
        }

        public static T JsonGet<T>(this IDatabase redis, RedisKey key)
        {
            RedisValue value = redis.StringGet(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public static void JsonHashSet<T>(this IDatabase redis, RedisKey key, RedisValue field, T value)
        {
            string json = JsonConvert.SerializeObject(value);

            redis.HashSet(key, field, json);
        }

        public static T JsonHashGet<T>(this IDatabase redis, RedisKey key, RedisValue field)
        {
            RedisValue value = redis.HashGet(key, field);

            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public static List<T> JsonHashGet<T>(this IDatabase redis, RedisKey key, RedisValue[] fields)
        {
            RedisValue[] values = redis.HashGet(key, fields.Distinct().ToArray());

            return values.Where(t => t.HasValue).Select(t => JsonConvert.DeserializeObject<T>(t)).ToList();
        }
    }
}
