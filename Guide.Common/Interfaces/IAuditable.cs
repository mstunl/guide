using System;

namespace Guide.Common.Interfaces
{
    public interface IAuditable
    {
        int? CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
