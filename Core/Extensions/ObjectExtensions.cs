using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// Adds anonymous property into existing object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<string, object> AddProperty(this object obj, string name, object value)
        {
            var dictionary = obj.ToDictionary();
            dictionary.Add(name, value);
            return dictionary;
        }

        /// <summary>
        /// Gets dictionary of an anonymous object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(obj));
            }
            return result;
        }
    }
}
