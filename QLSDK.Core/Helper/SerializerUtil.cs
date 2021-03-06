﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSDK.Core
{
    /// <summary>
    /// 序列化处理辅助类
    /// </summary>
    class SerializerUtil
    {
        /// <summary>
        /// 序列化成Json数据
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string SerializeJson(object val)
        {
            return JsonConvert.SerializeObject(val, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local
            });
        }

        /// <summary>
        /// DeSerialize String to Data
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="str">Encrypt String</param>
        /// <returns>Type Object</returns>
        public static T DeSerializeJson<T>(string str)
        {
            return (T)DeSerializeJson(typeof(T), str);
        }

        public static object DeSerializeJson(Type type, string str)
        {
            var scriptSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local
            });
            return JsonConvert.DeserializeObject(str, type);
        }
    }
}
