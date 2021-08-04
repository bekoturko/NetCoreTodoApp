using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NetCoreTodoApp.Business.Abstract;
using NetCoreTodoApp.Controllers;
using NetCoreTodoApp.Model;
using NetCoreTodoApp.NUnitTests.Helpers.Fixtures;
using NetCoreTodoApp.NUnitTests.Helpers.Mock;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;

namespace NetCoreTodoApp.NUnitTests.ControllerTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.Fixtures)]
	public class TodoControllerTests : BaseTestFixture
	{
		#region members & setup

		private StrictMock<ITodoService> todoService;
		private StrictMock<IUserManagerWrapper> userManagerWrapper;
		private StrictMock<ITodoFactory> todoFactory;

		private TodoController controller;

		[SetUp]
		public void Initialize()
		{
			todoService = new StrictMock<ITodoService>();
			userManagerWrapper = new StrictMock<IUserManagerWrapper>();
			todoFactory = new StrictMock<ITodoFactory>();

			controller = new TodoController(
				todoService.Object,
				userManagerWrapper.Object,
				todoFactory.Object);
		}

		#endregion members & setup

		#region verify mocks

		protected override void VerifyMocks()
		{
		}

		#endregion verify mocks

		#region index

		[Test]
		public void Index_UserHasLoggedInAndTodoListEmpty_ReturnViewResult()
		{
			// Arrange
			var userId = "123";
			var user = ClaimsPrincipal.Current;
			var todos = new List<Todo>();

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoService.Setup(x => x.GetTodoList(userId)).Returns(todos);

			// Act
			var result = controller.Index();

			// Assert
			var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<TodoViewModel>();
			resultModel.HasTodo.Should().BeFalse();
			resultModel.Todos.Should().BeEmpty();
			resultModel.Todos.Should().HaveCount(todos.Count);
		}

		[Test]
		public void Index_UserHasLoggedInAndUserHasTodoList_ReturnViewResult()
		{
			// Arrange
			var userId = "456";
			var user = ClaimsPrincipal.Current;
			var todos = new List<Todo>
			{
				new Todo()
			};

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoService.Setup(x => x.GetTodoList(userId)).Returns(todos);

			// Act
			var result = controller.Index();

			// Assert
			var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Index).ModelAs<TodoViewModel>();
			resultModel.HasTodo.Should().BeTrue();
			resultModel.Todos.Should().NotBeEmpty();
			resultModel.Todos.Should().HaveCount(todos.Count);
		}

		#endregion index

		#region add

		[Test]
		public void Add_FormMethodGet_ReturnViewResult()
		{
			// Arrange
			var addViewModel = new TodoAddViewModel();

			todoFactory.Setup(x => x.CreateAddViewModel()).Returns(addViewModel);

			// Act
			var result = controller.Add();

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<TodoAddViewModel>();
		}

		[Test]
		public void Add_ModelStateHasError_ReturnViewResult()
		{
			// Arrange
			var addViewModel = new TodoAddViewModel();

			controller.ModelState.AddModelError("Title", "required field");
			controller.ModelState.AddModelError("Description", "required field");

			// Act
			var result = controller.Add(addViewModel);

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<TodoAddViewModel>();
		}

		[Test]
		public void Add_InsertFailed_ReturnViewResult()
		{
			// Arrange
			var userId = "777";
			var model = new TodoAddViewModel
			{
				UserId = userId,
				Title = "todo 1",
				Description = "first todo"
			};
			var user = ClaimsPrincipal.Current;
			var todo = new Todo
			{
				UserId = userId,
				Title = model.Title,
				Description = model.Description,
				DateCreated = model.DateCreated
			};
			var isSuccess = false;

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoFactory.Setup(x => x.CreateTodoInsertModel(userId, model.Title, model.Description)).Returns(todo);
			todoService.Setup(x => x.Add(todo)).Returns(isSuccess);

			// Act
			var result = controller.Add(model);

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Add).ModelAs<TodoAddViewModel>();
		}

		[Test]
		public void Add_NoCondition_ReturnRedirectIndexPage()
		{
			// Arrange
			var userId = "444";
			var model = new TodoAddViewModel
			{
				UserId = userId,
				Title = "todo 2",
				Description = "second todo"
			};
			var user = ClaimsPrincipal.Current;
			var todo = new Todo
			{
				UserId = userId,
				Title = model.Title,
				Description = model.Description,
				DateCreated = model.DateCreated
			};
			var isSuccess = true;

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoFactory.Setup(x => x.CreateTodoInsertModel(userId, model.Title, model.Description)).Returns(todo);
			todoService.Setup(x => x.Add(todo)).Returns(isSuccess);

			// Act
			var result = controller.Add(model);

			// Assert
			result.Should().BeRedirectToActionResult().WithActionName(ViewNames.Index).WithControllerName(ControllerNames.Todo);
		}

		#endregion add

		#region edit

		[Test]
		public void Edit_FormMethodGetAndHasFoundTodoItem_ReturnViewResult()
		{
			// Arrange
			var id = 7;
			var userId = "777";
			var user = ClaimsPrincipal.Current;
			var todo = new Todo();
			var editViewModel = new TodoEditViewModel
			{
				TodoId = id,
				UserId = userId
			};

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoService.Setup(x => x.GetItem(userId, id)).Returns(todo);
			todoFactory.Setup(x => x.CreateEditViewModel(todo)).Returns(editViewModel);

			// Act
			var result = controller.Edit(id);

			// Assert
			var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<TodoEditViewModel>();
			resultModel.Should().NotBeNull();
			resultModel.TodoId.Should().Be(id);
			resultModel.UserId.Should().Be(userId);
		}

		[Test]
		public void Edit_FormMethodGetAndHasNotFoundTodoItem_ReturnRedirectResult()
		{
			// Arrange
			var id = 8;
			var userId = "88";
			var user = ClaimsPrincipal.Current;
			Todo todo = null;

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoService.Setup(x => x.GetItem(userId, id)).Returns(todo);

			// Act
			var result = controller.Edit(id);

			// Assert
			result.Should().BeRedirectToActionResult().WithControllerName(ControllerNames.Todo).WithActionName(ViewNames.Index);
		}

		[Test]
		public void Edit_ModelStateHasError_ReturnViewResult()
		{
			// Arrange
			var editViewModel = new TodoEditViewModel();

			controller.ModelState.AddModelError("Title", "required field");
			controller.ModelState.AddModelError("Description", "required field");

			// Act
			var result = controller.Edit(editViewModel);

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<TodoEditViewModel>();
		}

		[Test]
		public void Edit_UpdateFailed_ReturnViewResult()
		{
			// Arrange
			var userId = "777";
			var model = new TodoEditViewModel
			{
				UserId = userId,
				Title = "todo 1",
				Description = "first todo"
			};
			var user = ClaimsPrincipal.Current;
			var todo = new Todo
			{
				UserId = userId,
				Title = model.Title,
				Description = model.Description,
				DateCreated = model.DateCreated
			};
			var isSuccess = false;

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoFactory.Setup(x => x.CreateTodoEditModel(model, userId)).Returns(todo);
			todoService.Setup(x => x.Edit(todo)).Returns(isSuccess);

			// Act
			var result = controller.Edit(model);

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Edit).ModelAs<TodoEditViewModel>();
		}

		[Test]
		public void Edit_NoCondition_ReturnViewResult()
		{
			// Arrange
			var userId = "777";
			var model = new TodoEditViewModel
			{
				UserId = userId,
				Title = "todo 1",
				Description = "first todo"
			};
			var user = ClaimsPrincipal.Current;
			var todo = new Todo
			{
				UserId = userId,
				Title = model.Title,
				Description = model.Description,
				DateCreated = model.DateCreated
			};
			var isSuccess = true;

			userManagerWrapper.Setup(x => x.GetUserId(user)).Returns(userId);
			todoFactory.Setup(x => x.CreateTodoEditModel(model, userId)).Returns(todo);
			todoService.Setup(x => x.Edit(todo)).Returns(isSuccess);

			// Act
			var result = controller.Edit(model);

			// Assert
			result.Should().BeRedirectToActionResult().WithActionName(ViewNames.Index).WithControllerName(ControllerNames.Todo);
		}

		#endregion edit
	}
}