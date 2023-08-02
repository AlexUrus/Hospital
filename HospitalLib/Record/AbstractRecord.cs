using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Record
{
    [Serializable]
    public abstract class AbstractRecord<T> : IRecord where T : class
    {
        [JsonProperty(PropertyName = "Type")]
        public TypeRecord Type { get; }
        [JsonProperty(PropertyName = "Value")]
        public virtual T Value { get; set; }

        public AbstractRecord(TypeRecord type, T value)
        {
            Type = type;
            Value = value;
        }
    }
}
