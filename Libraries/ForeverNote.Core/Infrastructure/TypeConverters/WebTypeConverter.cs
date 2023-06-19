using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Security;
using ForeverNote.Core.Infrastructure.TypeConverters.Converter;
using System.Collections.Generic;
using System.ComponentModel;

namespace ForeverNote.Core.Infrastructure.TypeConverters
{
    public class WebTypeConverter : ITypeConverter
    {
        public void Register()
        {
            TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            TypeDescriptor.AddAttributes(typeof(List<double>), new TypeConverterAttribute(typeof(GenericListTypeConverter<double>)));
            TypeDescriptor.AddAttributes(typeof(List<string>), new TypeConverterAttribute(typeof(GenericListTypeConverter<string>)));
            TypeDescriptor.AddAttributes(typeof(bool), new TypeConverterAttribute(typeof(BoolTypeConverter)));

            //dictionaries
            TypeDescriptor.AddAttributes(typeof(Dictionary<int, int>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<int, int>)));
            TypeDescriptor.AddAttributes(typeof(Dictionary<string, bool>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<string, bool>)));

            //custom attributes
            TypeDescriptor.AddAttributes(typeof(List<GenericAttribute>), new TypeConverterAttribute(typeof(GenericAttributeListTypeConverter)));
            TypeDescriptor.AddAttributes(typeof(IList<GenericAttribute>), new TypeConverterAttribute(typeof(GenericAttributeListTypeConverter)));

            TypeDescriptor.AddAttributes(typeof(RefreshToken), new TypeConverterAttribute(typeof(RefreshTokenTypeConverter)));


        }

        public int Order => 0;
    }
}
