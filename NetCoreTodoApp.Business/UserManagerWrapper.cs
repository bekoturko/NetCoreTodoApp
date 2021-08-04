using NetCoreTodoApp.Business.Abstract;
using System.Security.Claims;

namespace NetCoreTodoApp.Business
{
	public class UserManagerWrapper : IUserManagerWrapper
	{
		public string GetUserId(ClaimsPrincipal currentUser)
		{
			return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
		}
	}
}