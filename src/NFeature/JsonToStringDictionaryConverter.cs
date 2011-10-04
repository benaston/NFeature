namespace NFeature
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using Newtonsoft.Json;

    public sealed class JsonToStringDictionaryConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>((string) data);
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
        {
            ValidateType(value, typeof (Dictionary<string, string>));
            var dictionary = value as Dictionary<string, string>;
            if (dictionary != null)
            {
                return JsonConvert.SerializeObject(dictionary);
            }

            return null;
        }

        private static void ValidateType(object value, Type expected)
        {
            if ((value != null) && (value.GetType() != expected))
            {
                throw new ArgumentException(string.Format("Converter unsupported value type {0}", new {expected.Name}));
            }
        }
    }
}