using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guide.Core.AuthModels;
using Guide.Core.DomainModels;
using Microsoft.AspNetCore.Identity;

namespace Guide.Application.Data
{
    public class DbSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleMgr;
        private readonly UserManager<ApplicationUser> _userMgr;

        public DbSeeder(UserManager<ApplicationUser> userMgr, RoleManager<ApplicationRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("mustafaunlu");

            // Add User
            if (user == null)
            {
                if (!(await _roleMgr.RoleExistsAsync("Admin")))
                {
                    var role = new ApplicationRole() {Name = "Admin"};

                    //role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" });
                    await _roleMgr.CreateAsync(role);
                }

                user = new ApplicationUser()
                {
                    UserName = "mustafaunlu",
                    Name = "Mustafa Ünlü",
                    Email = "mustafa@unlu.com"
                };

                var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }

            }
        }
    }
}
