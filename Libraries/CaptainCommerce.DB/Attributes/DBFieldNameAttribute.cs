using System;

namespace CaptainCommerce.DB.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DBFieldNameAttribute : Attribute
    {
        private readonly string _name;

        public DBFieldNameAttribute(string name)
        {
            _name = name;
        }
        public virtual string Name {
            get { return _name; }
        }
    }
}
