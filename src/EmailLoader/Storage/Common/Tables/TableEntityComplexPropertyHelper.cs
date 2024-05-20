using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EmailLoader.Storage.Common.Tables
{
    public static class TableEntityComplexPropertyHelper 
    {
        public static void Serialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> results)
        {
            var complexProperties = GetComplexProperties(entity);
            foreach (var property in complexProperties) 
            {
                results.Add(property.Name, new EntityProperty(JsonConvert.SerializeObject(property.GetValue(entity))));
            }
        }

        public static void Deserialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> properties)
        {
            var complexProperties = GetComplexProperties(entity);
            foreach (var property in complexProperties)
            {
                property.SetValue(entity, JsonConvert.DeserializeObject(properties[property.Name].StringValue, property.PropertyType));
            }
        }

        private static List<PropertyInfo> GetComplexProperties<TEntity>(TEntity entity) => entity
            .GetType()
            .GetProperties()
            .Where(x => x.GetCustomAttributes(typeof(TableEntityComplexPropertyAttribute), inherit: false).Any())
            .ToList();
    }
}
