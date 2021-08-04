using FluentAssertions;
using NetCoreTodoApp.Business;
using NetCoreTodoApp.Model;
using NetCoreTodoApp.NUnitTests.Helpers.Fixtures;
using NUnit.Framework;
using System;

namespace NetCoreTodoApp.NUnitTests.ControllerTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.Fixtures)]
	public class TodoFactoryTests : BaseTestFixture
	{
		#region members & setup

		private TodoFactory factory;

		[SetUp]
		public void Initialize()
		{
			factory = new TodoFactory();
		}

		#endregion members & setup

		#region verify mocks

		protected override void VerifyMocks()
		{
		}

		#endregion verify mocks

		[Test]
		public void CreateAddViewModel_NoCondition_ReturnViewModel()
		{
			// Arrange
			var model = new TodoAddViewModel();

			// Act
			var result = factory.CreateAddViewModel();

			// Assert
			result.Should().NotBeNull();
			result.Should().BeEquivalentTo<TodoAddViewModel>(model);
		}

		[Test]
		public void CreateTodoInsertModel_NoCondition_ReturnModel()
		{
			// Arrange
			var userId = "12312313123";
			var title = "title";
			var description = "description";
			var date = DateTime.Now;

			// Act
			var result = factory.CreateTodoInsertModel(userId, title, description);

			// Assert
			result.Should().NotBeNull();
			result.DateCreated.Should().BeSameDateAs(date);
		}

		[Test]
		public void CreateEditViewModel_TodoIsNull_ReturnEmptyViewModel()
		{
			// Arrange
			Todo todo = null;

			// Act
			var result = factory.CreateEditViewModel(todo);

			// Assert
			result.Should().NotBeNull();
		}

		[Test]
		public void CreateEditViewModel_NoCondition_ReturnViewModel()
		{
			// Arrange
			var todo = new Todo
			{
				TodoId = 5
			};

			// Act
			var result = factory.CreateEditViewModel(todo);

			// Assert
			result.Should().NotBeNull();
			result.TodoId.Should().BePositive();
		}

		[Test]
		public void CreateTodoEditModel_NoCondition_ReturnModel()
		{
			// Arrange
			var userId = "12312313123";
			var viewModel = new TodoEditViewModel
			{
				TodoId = 8
			};

			// Act
			var result = factory.CreateTodoEditModel(viewModel, userId);

			// Assert
			result.Should().NotBeNull();
			result.TodoId.Should().BePositive();
		}
	}
}