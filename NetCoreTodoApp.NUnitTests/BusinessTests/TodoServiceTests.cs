using FluentAssertions;
using NetCoreTodoApp.Business;
using NetCoreTodoApp.Data.Abstract;
using NetCoreTodoApp.Model;
using NetCoreTodoApp.NUnitTests.Helpers.Fixtures;
using NetCoreTodoApp.NUnitTests.Helpers.Mock;
using NUnit.Framework;
using System.Collections.Generic;

namespace NetCoreTodoApp.NUnitTests.ControllerTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.Fixtures)]
	public class TodoServiceTests : BaseTestFixture
	{
		#region members & setup

		private StrictMock<ITodoRepository> repository;

		private TodoService service;

		[SetUp]
		public void Initialize()
		{
			repository = new StrictMock<ITodoRepository>();

			service = new TodoService(repository.Object);
		}

		#endregion members & setup

		#region verify mocks

		protected override void VerifyMocks()
		{
		}

		#endregion verify mocks

		#region get user todo list

		[Test]
		public void GetTodoList_TodoListEmpty_ReturnEmptyList()
		{
			// Arrange
			var userId = "456";
			var todos = new List<Todo>();

			repository.Setup(x => x.GetTodoList(userId)).Returns(todos);

			// Act
			var result = service.GetTodoList(userId);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeEmpty();
		}

		[Test]
		public void GetTodoList_NoCondition_ReturnTodoList()
		{
			// Arrange
			var userId = "456";
			var todos = new List<Todo>
			{
				new Todo()
			};

			repository.Setup(x => x.GetTodoList(userId)).Returns(todos);

			// Act
			var result = service.GetTodoList(userId);

			// Assert
			result.Should().NotBeNullOrEmpty();
			result.Should().HaveCount(todos.Count);
		}

		#endregion get user todo list

		#region get item

		[Test]
		public void GetItem_NotFound_ReturnNull()
		{
			// Arrange
			var userId = "456";
			var todoId = 5;
			Todo todo = null;

			repository.Setup(x => x.GetItem(userId, todoId)).Returns(todo);

			// Act
			var result = service.GetItem(userId, todoId);

			// Assert
			result.Should().BeNull();
		}

		[Test]
		public void GetItem_NoCondition_ReturnTodoItem()
		{
			// Arrange
			var userId = "456";
			var todoId = 5;
			var todo = new Todo
			{
				TodoId = todoId,
				UserId = userId,
				Title = "todo 1"
			};

			repository.Setup(x => x.GetItem(userId, todoId)).Returns(todo);

			// Act
			var result = service.GetItem(userId, todoId);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeSameAs(todo);
		}

		#endregion get item

		#region add

		[Test]
		public void Add_InsertFailed_ReturnFalse()
		{
			// Arrange
			var todo = new Todo();
			var rowCount = 0;

			repository.Setup(x => x.Add(todo)).Returns(rowCount);

			// Act
			var result = service.Add(todo);

			// Assert
			result.Should().BeFalse();
		}

		[Test]
		public void Add_NoCondition_ReturnTrue()
		{
			// Arrange
			var userId = "987";
			var todoId = 23;
			var todo = new Todo
			{
				TodoId = todoId,
				UserId = userId,
				Title = "todo 1",
				Description = "todo detail"
			};
			var rowCount = 1;

			repository.Setup(x => x.Add(todo)).Returns(rowCount);

			// Act
			var result = service.Add(todo);

			// Assert
			result.Should().BeTrue();
		}

		#endregion add

		#region edit

		[Test]
		public void Edit_UpdateFailed_ReturnFalse()
		{
			// Arrange
			var todo = new Todo();
			var rowCount = 0;

			repository.Setup(x => x.Edit(todo)).Returns(rowCount);

			// Act
			var result = service.Edit(todo);

			// Assert
			result.Should().BeFalse();
		}

		[Test]
		public void Edit_NoCondition_ReturnTrue()
		{
			// Arrange
			var userId = "258";
			var todoId = 2;
			var todo = new Todo
			{
				TodoId = todoId,
				UserId = userId,
				Title = "todo 1",
				Description = "todo detail"
			};
			var rowCount = 1;

			repository.Setup(x => x.Edit(todo)).Returns(rowCount);

			// Act
			var result = service.Edit(todo);

			// Assert
			result.Should().BeTrue();
		}

		#endregion edit
	}
}