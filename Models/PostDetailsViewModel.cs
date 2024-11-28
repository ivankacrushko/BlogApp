using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Models
{
	public class PostDetailsViewModel
	{
		public Post Post { get; set; }  // Post, który jest wyświetlany
		public bool IsAuthor { get; set; }  // Flaga wskazująca, czy użytkownik jest autorem
	}
}
