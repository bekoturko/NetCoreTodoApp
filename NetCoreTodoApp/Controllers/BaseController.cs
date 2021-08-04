using Microsoft.AspNetCore.Mvc;
using NetCoreTodoApp.Filters;

namespace NetCoreTodoApp.Controllers
{
	[TypeFilter(typeof(TodoAppExceptionFilter))]
	public class BaseController : Controller
	{
		
	}
}