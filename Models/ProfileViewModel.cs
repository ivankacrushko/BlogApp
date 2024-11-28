using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Models
{
	public class ProfileViewModel
	{
		public ApplicationUser User { get; set; }
		public List<Post> Posts { get; set; }
	}
}
