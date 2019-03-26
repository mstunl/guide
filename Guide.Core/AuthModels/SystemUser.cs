using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Guide.Core.AuthModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public int DealerId { get; set; }
    }
    public class ApplicationRole : IdentityRole<int> { }
    public class ApplicationUserToken : IdentityUserToken<int> { }
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationRoleClaim : IdentityRoleClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    
}
