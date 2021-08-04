using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCoreTodoApp.Framework.Abstract.Services;

namespace NetCoreTodoApp.Filters
{
	public class TodoAppExceptionFilter : IExceptionFilter
	{
		#region ctor & member

		private readonly IModelMetadataProvider _modelMetadataProvider;
		private readonly ILoggingService loggingService;

		public TodoAppExceptionFilter(IModelMetadataProvider modelMetadataProvider, ILoggingService loggingService)
		{
			_modelMetadataProvider = modelMetadataProvider;
			this.loggingService = loggingService;
		}

		#endregion ctor & member

		public void OnException(ExceptionContext context)
		{
			var result = new ViewResult
			{
				ViewName = "CustomError",
				ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
			};

			var methodName = $"{nameof(TodoAppExceptionFilter)}/{nameof(OnException)}";
			var errorMessage = "An Error has Occurred.";

			loggingService.LogError(methodName, errorMessage, context.Exception);

			// Here we can pass additional detailed data via ViewData
			result.ViewData.Add("Exception", context.Exception);

			context.ExceptionHandled = true;
			context.Result = result;
		}
	}
}