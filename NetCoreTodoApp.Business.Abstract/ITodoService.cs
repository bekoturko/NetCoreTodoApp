using NetCoreTodoApp.Model;
using System.Collections.Generic;

namespace NetCoreTodoApp.Business.Abstract
{
	public interface ITodoService
	{
		ICollection<Todo> GetTodoList(string userId);

		bool Add(Todo todo);

		Todo GetItem(string userId, int todoId);

		bool Edit(Todo todo);
	}
}