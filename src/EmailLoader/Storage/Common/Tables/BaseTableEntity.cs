using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace EmailLoader.Storage.Common.Tables
{
    public abstract class BaseTableEntity : TableEntity 
    {
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            TableEntityComplexPropertyHelper.Serialize(this, results);
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            TableEntityComplexPropertyHelper.Deserialize(this, properties);
        }
    }
}
