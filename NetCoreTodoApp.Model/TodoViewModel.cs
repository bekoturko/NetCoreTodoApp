using System.Collections.Generic;
using System.Linq;

namespace NetCoreTodoApp.Model
{
	public class TodoViewModel
	{
		public ICollection<Todo> Todos { get; set; }

		public bool HasTodo
		{
			get
			{
				return Todos.Any();
			}
		}
	}
}