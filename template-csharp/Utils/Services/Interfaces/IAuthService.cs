using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace template_csharp.Utils.Services.Interfaces;

public interface IAuthService
{
    Task<dynamic> LogInAsync(string username, string password);
    Task<dynamic> RegisterAsync(string email, string username, string password);
    Task<dynamic> FindUserByIdAsync(dynamic id);
    Task<dynamic> FindUserByEmailAsync(string email);
    Task<dynamic> FindUserByUserAsync(string username);
    Task<bool> AddRoleAsync(string role);
    Task<bool> AddUserToRolesAsync(IdentityUser user, List<string> roles);
    Task<bool> RoleExistsAsync(string roles);
    Task<bool> AddClaimAsync(IdentityUser user, List<Claim> claim);
}