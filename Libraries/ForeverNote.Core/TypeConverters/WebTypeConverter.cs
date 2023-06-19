//using ForeverNote.Core.ComponentModel;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Security;
using ForeverNote.Core.TypeConverters.Converter;
using System.Collections.Generic;
using System.ComponentModel;

namespace ForeverNote.Core.TypeConverters
{
    public class WebTypeConverter : ITypeConverter
    {
        public void Register()
        {
            _ = TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            _ = TypeDescriptor.AddAttributes(typeof(List<double>), new TypeConverterAttribute(typeof(GenericListTypeConverter<double>)));
            _ = TypeDescriptor.AddAttributes(typeof(List<string>), new TypeConverterAttribute(typeof(GenericListTypeConverter<string>)));
            _ = TypeDescriptor.AddAttributes(typeof(bool), new TypeConverterAttribute(typeof(BoolTypeConverter)));

            //dictionaries
            _ = TypeDescriptor.AddAttributes(typeof(Dictionary<int, int>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<int, int>)));
            _ = TypeDescriptor.AddAttributes(typeof(Dictionary<string, bool>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<string, bool>)));

            //generic attributes
            _ = TypeDescriptor.AddAttributes(typeof(List<GenericAttribute>), new TypeConverterAttribute(typeof(GenericAttributeListTypeConverter)));
            _ = TypeDescriptor.AddAttributes(typeof(IList<GenericAttribute>), new TypeConverterAttribute(typeof(GenericAttributeListTypeConverter)));

            _ = TypeDescriptor.AddAttributes(typeof(RefreshToken), new TypeConverterAttribute(typeof(RefreshTokenTypeConverter)));
        }

        public int Order => 0;
    }
}
