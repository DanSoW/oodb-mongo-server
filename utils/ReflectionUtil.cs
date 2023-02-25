using oodb_mongo_server.models;
using oodb_mongo_server.models.data;
using System.Reflection;

/*
 * В данном файле находится класс ReflectionUtil,
 * который содержит в себе статические методы активно использующиеся
 * при необходимости использования механизмов рефлексии. Например,
 * базовый класс контроллеров API
 * **/

namespace oodb_mongo_server.utils
{
    /// <summary>
    /// Класс определяющий статические методы,
    /// активно использующиеся для упрощения задач с механизмом рефлексии
    /// </summary>
    public class ReflectionUtil
    {
        /// <summary>
        /// Получение значений всех полей объекта определённого типа (базовый тип - IdModel)
        /// </summary>
        /// <typeparam name="T">Тип целевого объекта</typeparam>
        /// <param name="element">Целевой объект</param>
        /// <returns>Список значений полей целевого объекта</returns>
        public static List<object> getFields<T>(T element) where T : IdModel
        {
            List<object> fields = new List<object>();

            // Определение флагов, по которым будет осуществляться поиск полей целевого объекта
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

        /// <summary>
        /// Получение значений всех полей объекта определённого типа (базовый тип - IdDataModel)
        /// </summary>
        /// <typeparam name="T">Тип целевого объекта</typeparam>
        /// <param name="element">Целевой объект</param>
        /// <returns>Список значений полей целевого объекта</returns>
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

        /// <summary>
        /// Получение информации о всех полях объекта определённого типа
        /// </summary>
        /// <typeparam name="T">Тип целевого объекта</typeparam>
        /// <param name="element">Целевой объект</param>
        /// <returns>Список значений полей целевого объекта</returns>
        public static List<FieldInfo> getDataFieldsInfo<T>(T element) where T : IdDataModel
        {
            BindingFlags bindingFlags = BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static;

            return element.GetType().GetFields(bindingFlags).ToList();
        }
    }
}
