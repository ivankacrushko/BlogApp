using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
	public class Post
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Content is required.")]
		public string Content { get; set; }

		public DateTime CreatedAt { get; set; }

		// UserId to identyfikator użytkownika
		[Required(ErrorMessage = "UserId is required.")]
		public string UserId { get; set; }

		// Powiązanie z użytkownikiem
		public ApplicationUser User { get; set; }
	}

}
