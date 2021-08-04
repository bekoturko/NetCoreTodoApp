using NetCoreTodoApp.Data.Abstract;
using NetCoreTodoApp.Framework.Abstract.Services;
using NetCoreTodoApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreTodoApp.Data
{
	public class TodoRepository : ITodoRepository
	{
		#region ctor & member

		private readonly ApplicationDbContext context;
		private readonly ILoggingService loggingService;

		public TodoRepository(ApplicationDbContext context, ILoggingService loggingService)
		{
			this.context = context;
			this.loggingService = loggingService;
		}

		#endregion ctor & member

		public ICollection<Todo> GetTodoList(string userId)
		{
			try
			{
				return context.ToDos.Where(x => x.UserId == userId).ToList();
			}
			catch (Exception ex)
			{
				var message = $"Yapılacaklar listesi alınırken bir hata oluştu. UserId: {userId}";

				CreateErrorLog(nameof(GetTodoList), message, ex);

				return new List<Todo>();
			}
		}

		public int Add(Todo todo)
		{
			try
			{
				context.ToDos.Add(todo);

				return context.SaveChanges();
			}
			catch (Exception ex)
			{
				var message = $"Yeni kayıt ekleme işleminde bir hata oluştu. UserId: {todo.UserId}";

				CreateErrorLog(nameof(Add), message, ex);

				return ((int)DbContextResult.Fail);
			}
		}

		public Todo GetItem(string userId, int todoId)
		{
			try
			{
				return context.ToDos.FirstOrDefault(x => x.UserId == userId && x.TodoId == todoId);
			}
			catch (Exception ex)
			{
				var message = $"Kayıt okuma işleminde bir hata oluştu. TodoId: {todoId}. UserId: {userId}";

				CreateErrorLog(nameof(GetItem), message, ex);

				return null;
			}
		}

		public int Edit(Todo todo)
		{
			try
			{
				var entity = context.ToDos.FirstOrDefault(x => x.UserId == todo.UserId && x.TodoId == todo.TodoId);

				if (entity == null)
				{
					return ((int)DbContextResult.Fail);
				}

				entity.DateUpdate = todo.DateUpdate;
				entity.Description = todo.Description;
				entity.IsDone = todo.IsDone;
				entity.Title = todo.Title;

				return context.SaveChanges();
			}
			catch (Exception ex)
			{
				var message = $"Kayıt güncelleme işleminde bir hata oluştu. TodoId: {todo.TodoId}. UserId: {todo.UserId}";

				CreateErrorLog(nameof(Edit), message, ex);

				return ((int)DbContextResult.Fail);
			}
		}

		private void CreateErrorLog(string methodName, string message, Exception exception)
		{
			var createMethodName = SetMethodNameForLogMessage(methodName);

			loggingService.LogError(createMethodName, message, exception);
		}

		private string SetMethodNameForLogMessage(string methodName)
		{
			return $"{GetType().Name}.{methodName}";
		}
	}
}