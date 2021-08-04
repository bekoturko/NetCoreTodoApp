using Microsoft.AspNetCore.Mvc;
using NetCoreTodoApp.Model;

namespace NetCoreTodoApp.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController()
		{
		}

		public IActionResult Index()
		{
			return View(ViewNames.Index);
		}
	}
}