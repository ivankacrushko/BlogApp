using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BlogApp.Controllers
{
	[Authorize]
	public class PostsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var posts = await _context.Posts.Include(p => p.User).ToListAsync();
			return View(posts);
		}

		[AllowAnonymous]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();  // Jeśli id jest null, zwróć 404
			}

			var post = await _context.Posts
				.Include(p => p.User)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (post == null)
			{
				return NotFound();  // Jeśli nie znaleziono posta, zwróć 404
			}

			var currentUserId = _userManager.GetUserId(User);

			var model = new PostDetailsViewModel
			{
				Post = post,
				IsAuthor = post.UserId == currentUserId  // Sprawdzamy, czy użytkownik jest autorem
			};

			return View(model);  // Zwróć widok z postem
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Post post)
		{

			if (!ModelState.IsValid)
			{
				foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
				{
					Console.WriteLine("Error: " + error.ErrorMessage);
				}
			}

			var userId = _userManager.GetUserId(User);
			post.UserId = userId;
			var user = await _userManager.FindByIdAsync(userId);
			post.User = user;

			post.CreatedAt = DateTime.Now;
			Console.WriteLine("post.UserId: " + post.UserId);
			Console.WriteLine("post.User: " + post.User);
			_context.Posts.Add(post);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var post = await _context.Posts
				.FirstOrDefaultAsync(p => p.Id == id);

			if (post == null)
			{
				return NotFound();
			}

			// Sprawdzamy, czy użytkownik jest autorem posta
			var currentUserId = _userManager.GetUserId(User);
			if (post.UserId != currentUserId)
			{
				return Unauthorized();  // Jeśli nie jesteś autorem, nie masz dostępu
			}

			// Mapowanie do EditPostViewModel
			var model = new EditPostViewModel
			{
				Id = post.Id,
				Title = post.Title,
				Content = post.Content,
				UserId = post.UserId
			};

			return View(model);  // Przekazujemy model do widoku
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserId")] EditPostViewModel model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			var post = await _context.Posts.FindAsync(id);

			if (post == null)
			{
				return NotFound();
			}

			// Sprawdzamy, czy użytkownik jest autorem posta
			var currentUserId = _userManager.GetUserId(User);
			if (post.UserId != currentUserId)
			{
				return Unauthorized();
			}

			
			try
			{
				// Aktualizowanie posta w bazie danych
				post.Title = model.Title;
				post.Content = model.Content;

				_context.Update(post);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				return NotFound();
					
			}

			return RedirectToAction("Index", "Home");  // Przekierowujemy do listy postów
		}



	}
}
