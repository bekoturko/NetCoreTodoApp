using System.Security.Claims;

namespace NetCoreTodoApp.Business.Abstract
{
	public interface IUserManagerWrapper
	{
		string GetUserId(ClaimsPrincipal currentUser);
	}
}