using NetCoreTodoApp.Business.Abstract;
using NetCoreTodoApp.Data.Abstract;
using NetCoreTodoApp.Model;
using System.Collections.Generic;

namespace NetCoreTodoApp.Business
{
	public class TodoService : ITodoService
	{
		#region ctor & member

		private readonly ITodoRepository todoRepository;

		public TodoService(ITodoRepository todoRepository)
		{
			this.todoRepository = todoRepository;
		}

		#endregion ctor & member

		public ICollection<Todo> GetTodoList(string userId)
		{
			return todoRepository.GetTodoList(userId);
		}

		public Todo GetItem(string userId, int todoId)
		{
			return todoRepository.GetItem(userId, todoId);
		}

		public bool Add(Todo todo)
		{
			var rowCount = todoRepository.Add(todo);

			return IsSuccess(rowCount);
		}

		public bool Edit(Todo todo)
		{
			var rowCount = todoRepository.Edit(todo);

			return IsSuccess(rowCount);
		}

		private static bool IsSuccess(int rowCount)
		{
			if (rowCount <= ((int)DbContextResult.Fail))
			{
				return false;
			}

			return true;
		}
	}
}