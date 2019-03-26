using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guide.Common;
using Guide.Core;

namespace Guide.Application.ViewModel.CommandResponses.Product
{
    public class ProductCreateView : ViewBase<int>
    {
        public string Code { get; set; }
    }
}
