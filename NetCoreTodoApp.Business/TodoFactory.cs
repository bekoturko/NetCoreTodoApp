using NetCoreTodoApp.Business.Abstract;
using NetCoreTodoApp.Model;
using System;

namespace NetCoreTodoApp.Business
{
	public class TodoFactory : ITodoFactory
	{
		public TodoAddViewModel CreateAddViewModel()
		{
			return new TodoAddViewModel();
		}

		public Todo CreateTodoInsertModel(string userId, string title, string description)
		{
			return new Todo
			{
				UserId = userId,
				Title = title,
				Description = description,
				DateCreated = DateTime.Now
			};
		}

		public TodoEditViewModel CreateEditViewModel(Todo todo)
		{
			if (todo == null)
			{
				return new TodoEditViewModel();
			}

			return new TodoEditViewModel
			{
				DateCreated = todo.DateCreated,
				DateUpdate = todo.DateUpdate,
				Description = todo.Description,
				IsDone = todo.IsDone,
				Title = todo.Title,
				TodoId = todo.TodoId,
				UserId = todo.UserId
			};
		}

		public Todo CreateTodoEditModel(TodoEditViewModel model, string userId)
		{
			return new Todo
			{
				DateUpdate = DateTime.Now,
				Description = model.Description,
				IsDone = model.IsDone,
				Title = model.Title,
				TodoId = model.TodoId,
				UserId = userId
			};
		}
	}
}