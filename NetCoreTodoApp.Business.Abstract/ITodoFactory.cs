using NetCoreTodoApp.Model;

namespace NetCoreTodoApp.Business.Abstract
{
	public interface ITodoFactory
	{
		Todo CreateTodoInsertModel(string userId, string title, string description);

		TodoAddViewModel CreateAddViewModel();

		TodoEditViewModel CreateEditViewModel(Todo todo);

		Todo CreateTodoEditModel(TodoEditViewModel model, string userId);
	}
}