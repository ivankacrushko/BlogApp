﻿using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }
		public ICollection<Post> Posts { get; set; }
		public string Bio { get; set; }
		public DateTime RegistrationDate { get; set; }
	}
}
