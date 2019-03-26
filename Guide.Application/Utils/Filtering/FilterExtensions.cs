using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guide.Application.Utils.Filtering
{
    public static class FilterExtensions
    {
        public static List<QueryFilter> GrowFilter(this List<QueryFilter> list, string propertyName, Op operation, object value)
        {
            var filter = new QueryFilter() { PropertyName = propertyName, Operation = operation, Value = value };
            list.Add(filter);
            return list;
        }
    }
}
