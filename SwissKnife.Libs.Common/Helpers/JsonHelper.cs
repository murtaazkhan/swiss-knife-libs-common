
using SwissKnife.Libs.Common.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace SwissKnife.Libs.Common.Helpers
{
    /// <summary>
    /// This class helps format Json Content.
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Json Content wrapper for http content
        /// </summary>
        /// /// <param name="value">json content object</param>
        public JsonContent(object value)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, HttpHeaders.APPLICATION_JSON)
        {
        }

        /// <summary>
        /// Json Content wrapper for http content with json serialization settings
        /// </summary>
        /// /// <param name="value">json content object</param>
        /// <param name="serializerSettings">json content serialization settings</param>
        public JsonContent(object value, JsonSerializerSettings serializerSettings)
            : base(JsonConvert.SerializeObject(value, serializerSettings), Encoding.UTF8, HttpHeaders.APPLICATION_JSON)
        {
        }
    }

    /// <summary>
    /// This class provides commonly used json serialization settings.
    /// </summary>
    public static class SerializationSettings
    {
        /// <summary>
        /// Serialize Json with camel casing of keys and remove null value keys
        /// </summary>
        public static readonly JsonSerializerSettings CamelCaseAndNullValueHandling = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Serialize Json with default casing of keys and remove null value keys
        /// </summary>
        public static readonly JsonSerializerSettings DefaultCaseAndNullValueHandling = new()
        {
            ContractResolver = new DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
        };
    }
}
