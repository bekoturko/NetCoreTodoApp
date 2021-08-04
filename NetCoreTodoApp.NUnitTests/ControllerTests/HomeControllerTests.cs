using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NetCoreTodoApp.Controllers;
using NetCoreTodoApp.Model;
using NetCoreTodoApp.NUnitTests.Helpers.Fixtures;
using NUnit.Framework;

namespace NetCoreTodoApp.NUnitTests.ControllerTests
{
	[TestFixture]
	[Parallelizable(ParallelScope.Fixtures)]
	public class HomeControllerTests : BaseTestFixture
	{
		#region members & setup

		private HomeController controller;

		[SetUp]
		public void Initialize()
		{
			controller = new HomeController();
		}

		#endregion members & setup

		#region verify mocks

		protected override void VerifyMocks()
		{
		}

		#endregion verify mocks

		[Test]
		public void Index_NoCondition_ReturnViewResult()
		{
			// Arrange

			// Act
			var result = controller.Index();

			// Assert
			result.Should().BeViewResult().WithViewName(ViewNames.Index);
		}
	}
}