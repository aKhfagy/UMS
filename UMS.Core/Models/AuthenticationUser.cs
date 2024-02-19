using Microsoft.AspNetCore.Identity;

namespace UMS.Core.Models;

public class AuthenticationUser : IdentityUser
{
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
}
