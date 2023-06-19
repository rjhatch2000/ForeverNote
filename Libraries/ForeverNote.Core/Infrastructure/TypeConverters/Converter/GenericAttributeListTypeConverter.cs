using ForeverNote.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace ForeverNote.Core.Infrastructure.TypeConverters.Converter
{
    public class GenericAttributeListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is not string valueStr) return base.ConvertFrom(context, culture, value);
            List<GenericAttribute> customAttributes = null;
            if (string.IsNullOrEmpty(valueStr)) return null;
            try
            {
                customAttributes = JsonSerializer.Deserialize<List<GenericAttribute>>(valueStr);
            }
            catch
            {
                // ignored
            }

            return customAttributes;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value is List<GenericAttribute> customAttributes ? JsonSerializer.Serialize(customAttributes) : "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
