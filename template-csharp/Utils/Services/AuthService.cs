using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using template_csharp.Utils.Services.Interfaces;

namespace template_csharp.Utils.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
    }

    public async Task<dynamic> LogInAsync(string username, string password)
    {
        SignInResult result = await this.signInManager.PasswordSignInAsync(username, password, true, false);
        if (result.Succeeded)
        {
            IdentityUser user = await this.userManager.FindByNameAsync(username);
            if (user == null) return false;
            return user;
        }

        return false;
    }

    public async Task<dynamic> RegisterAsync(string email, string username, string password)
    {
        IdentityUser user = new IdentityUser()
        {
            Email = email,
            UserName = username
        };
        
        IdentityResult result = await this.userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            if (await this.RoleExistsAsync("Member"))
            {
                if (await this.AddUserToRolesAsync(user, new List<string>() { "Member" }))
                {
                    if (await this.AddClaimAsync(user, new List<Claim>()
                        {
                            new Claim("ID", user.Id),
                            new Claim("Email", user.Email),
                        }))
                    {
                        return true;
                    };
                }
                
            }
        }

        return false;
    }

    public async Task<dynamic> FindUserByIdAsync(dynamic id)
    {
        IdentityUser user = await this.userManager.FindByIdAsync(id);
        if (user == null) return false;
        return user;
    }

    public async Task<dynamic> FindUserByEmailAsync(string email)
    {        
        IdentityUser user = await this.userManager.FindByEmailAsync(email);
        if (user == null) return false;
        return user;
    }

    public async Task<dynamic> FindUserByUserAsync(string username)
    {
        IdentityUser user = await this.userManager.FindByNameAsync(username);
        if (user == null) return false;
        return user;
    }

    public async Task<bool> AddRoleAsync(string role)
    {
        var result = await roleManager.CreateAsync(new()
        {
            Name = role
        });
        if (result.Succeeded) return true;
        return false;
    }

    public async Task<bool> AddUserToRolesAsync(IdentityUser user, List<string> roles)
    {
        foreach (string role in roles)
        {
            if (!(await this.RoleExistsAsync(role)))
            {
                await this.AddRoleAsync(role);
            }
            IdentityResult result = await this.userManager.AddToRoleAsync(user, role);
            if(!result.Succeeded) return false;            
        }

        return true;
    }

    public async Task<bool> RoleExistsAsync(string role)
    {
        if (await this.roleManager.RoleExistsAsync(role))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> AddClaimAsync(IdentityUser user, List<Claim> claims)
    {
        IdentityResult result = await this.userManager.AddClaimsAsync(user, claims);
        if (result.Succeeded) return true;
        return false;
    }
}