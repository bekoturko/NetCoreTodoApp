using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreTodoApp.Model
{
	public class Todo
	{
		public int TodoId { get; set; }

		public string UserId { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public bool IsDone { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime? DateUpdate { get; set; }

		public virtual AppUser User { get; set; }
	}
}