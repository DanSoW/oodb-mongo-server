using oodb_mongo_server.models;
using oodb_mongo_server.models.data;
using System.Reflection;

namespace oodb_mongo_server.utils
{
    public class ReflectionUtil
    {
        public static List<object> getFields<T>(T element) where T : IdModel
        {
            List<object> fields = new List<object>();

            BindingFlags bindingFlags = BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static;

            if (element.Id != null)
            {
                fields.Add(element.Id);
            }

            foreach (FieldInfo field in element.GetType().GetFields(bindingFlags))
            {
                var value = field.GetValue(element);
                
                if(value != null)
                {
                    fields.Add(value);
                }
            }

            return fields;
        }

        public static List<object> getDataFields<T>(T element) where T : IdDataModel
        {
            List<object> fields = new List<object>();

            BindingFlags bindingFlags = BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static;

            if (element.Id != null)
            {
                fields.Add(element.Id);
            }

            foreach (FieldInfo field in element.GetType().GetFields(bindingFlags))
            {
                var value = field.GetValue(element);

                if (value != null)
                {
                    fields.Add(value);
                }
            }

            return fields;
        }
    }
}
