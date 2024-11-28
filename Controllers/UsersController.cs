using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class UsersController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly UserManager<ApplicationUser> _userManager;

	public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
	{
		_context = context;
		_userManager = userManager;
	}

	[Authorize]
	public async Task<IActionResult> Index()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return RedirectToAction("Login", "Account");
		}
		var posts = await _context.Posts
			.Where(p => p.UserId == user.Id)
			.OrderByDescending(p => p.CreatedAt)
			.ToListAsync();
		var profileViewModel = new ProfileViewModel
		{
			User = user,
			Posts = posts
		};
		return View(profileViewModel);
	}

	// GET: Profile/Edit
	public async Task<IActionResult> Edit()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var user = _context.Users.Find(userId);

		if (user == null)
		{
			return NotFound();
		}
		var viewModel = new ProfileViewModel
		{
			Bio = user.Bio
		};
		return View(viewModel);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(ProfileViewModel model)
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return NotFound();
		}
		user.Bio = model.User.Bio;

		var result = await _userManager.UpdateAsync(user);
		if (result.Succeeded)
		{
			return RedirectToAction("Index", "Users");
		}
		foreach (var error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}
		return View(model);
	}
}
