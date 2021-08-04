using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace NetCoreTodoApp.Model
{
	public class AppUser : IdentityUser
	{
		public virtual ICollection<Todo> ToDos { get; set; }
	}
}