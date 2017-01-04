using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Presentation.Web.Services
{
    public class JSON
    {
        /// <summary>
        /// Convert class object to JSON string. USAGE: Output.SerializeJSON(test);
        /// </summary>
        /// <typeparam name="T">The type of the class</typeparam>
        /// <param name="obj">The class object</param>
        /// <returns>String</returns>
        public static string SerializeJSON<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        /// <summary>
        /// Convert JSON string to class object. USAGE: TestClass test = Output.DeserializeJSON&lt;TestClass&gt;(xml);
        /// </summary>
        /// <typeparam name="T">The type of the class</typeparam>
        /// <param name="json">The JSON string</param>
        /// <returns>Object</returns>
        public static T DeserializeJSON<T>(string json)
        {
            // First, convert to base64
            //string encoded = Statics.Base64Encode(json);
            //string decoded = Statics.Base64Decode(encoded);
            string decoded = Utf8Encoder.GetString(Utf8Encoder.GetBytes(json));
            T obj = Activator.CreateInstance<T>();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(decoded)))
            {
                obj = (T)serializer.ReadObject(ms);
            }
            return obj;
        }
        public static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
            "UTF-8",
            new EncoderReplacementFallback(string.Empty),
            new DecoderExceptionFallback()
        );
    }
}