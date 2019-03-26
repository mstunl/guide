using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Guide.Common;
using Guide.Common.Interfaces;

namespace Guide.Core
{
    public class AuditableEntityBase<TKey> : EntityBase<TKey>, IAuditable
    {
        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        [ConcurrencyCheck]
        public DateTime? ModifiedOn { get; set; }
    }
}
