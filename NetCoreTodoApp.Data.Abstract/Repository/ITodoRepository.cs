using NetCoreTodoApp.Model;
using System.Collections.Generic;

namespace NetCoreTodoApp.Data.Abstract
{
	public interface ITodoRepository
	{
		ICollection<Todo> GetTodoList(string userId);

		int Add(Todo todo);

		Todo GetItem(string userId, int todoId);

		int Edit(Todo todo);
	}
}