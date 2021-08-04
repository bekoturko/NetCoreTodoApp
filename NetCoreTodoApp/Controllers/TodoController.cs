using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreTodoApp.Business.Abstract;
using NetCoreTodoApp.Model;

namespace NetCoreTodoApp.Controllers
{
	[Authorize]
	public class TodoController : BaseController
	{
		#region ctor & member

		private readonly ITodoService todoService;
		private readonly IUserManagerWrapper userManagerWrapper;
		private readonly ITodoFactory todoFactory;

		public TodoController(
			ITodoService todoService,
			IUserManagerWrapper userManagerWrapper,
			ITodoFactory todoFactory)
		{
			this.todoService = todoService;
			this.userManagerWrapper = userManagerWrapper;
			this.todoFactory = todoFactory;
		}

		#endregion ctor & member

		#region index

		public IActionResult Index()
		{
			var userId = userManagerWrapper.GetUserId(User);

			var model = new TodoViewModel
			{
				Todos = todoService.GetTodoList(userId)
			};

			return View(ViewNames.Index, model);
		}

		#endregion index

		#region add

		public IActionResult Add()
		{
			var model = todoFactory.CreateAddViewModel();

			return View(ViewNames.Add, model);
		}

		[HttpPost]
		public IActionResult Add(TodoAddViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(ViewNames.Add, model);
			}

			var userId = userManagerWrapper.GetUserId(User);

			var todoItem = todoFactory.CreateTodoInsertModel(userId, model.Title, model.Description);

			var isSuccess = todoService.Add(todoItem);

			if (!isSuccess)
			{
				return View(ViewNames.Add, model);
			}

			return RedirectToAction(ViewNames.Index, ControllerNames.Todo);
		}

		#endregion add

		#region edit

		public IActionResult Edit(int id)
		{
			var userId = userManagerWrapper.GetUserId(User);

			var todo = todoService.GetItem(userId, id);

			if (todo == null)
			{
				return RedirectToAction(ViewNames.Index, ControllerNames.Todo);
			}

			var model = todoFactory.CreateEditViewModel(todo);

			return View(ViewNames.Edit, model);
		}

		[HttpPost]
		public IActionResult Edit(TodoEditViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(ViewNames.Edit, model);
			}

			var userId = userManagerWrapper.GetUserId(User);

			var todo = todoFactory.CreateTodoEditModel(model, userId);

			var isSuccess = todoService.Edit(todo);

			if (!isSuccess)
			{
				return View(ViewNames.Edit, model);
			}

			return RedirectToAction(ViewNames.Index, ControllerNames.Todo);
		}

		#endregion edit
	}
}