// ReSharper disable InconsistentNaming
namespace NFeature
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;

    public sealed class StringToEnGBDateTimeConverter : ConfigurationConverterBase
    {
        private const string DefaultDateTimeFormat = "dd/MM/yyyy:HH:mm:ss";
        // Methods
        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
        {
            return DateTime.ParseExact((string)data, DefaultDateTimeFormat, new CultureInfo("en-GB"),
                                       DateTimeStyles.None);
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
        {
            ValidateType(value, typeof (DateTime));

            return (DateTime)value;
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
// ReSharper restore InconsistentNaming