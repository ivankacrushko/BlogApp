﻿using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Models
{
	public class EditPostViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string UserId { get; set; }
	}
}
