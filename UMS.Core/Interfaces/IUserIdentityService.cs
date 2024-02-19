using UMS.Core.Dtos;

namespace UMS.Core.Interfaces
{
	public interface IUserIdentityService
	{
		public Task<ResultDto<bool>> RegisterUser(RegisterIdentityDto input);
		public Task<ResultDto<bool>> RegisterAdmin(RegisterIdentityDto input);
		public Task<ResultDto<TokenDto>> Authenticate(string username, string password);
		public Task<ResultDto<TokenDto>> RefreshToken(TokenDto token);
		public Task<ResultDto<bool>> Revoke(string username);
		public Task<ResultDto<bool>> RevokeAll();
	}
}
