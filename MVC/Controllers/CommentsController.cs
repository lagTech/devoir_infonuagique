﻿// CommentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id)
        {
            return View(await _context.Comments.Where(w => w.PostId == id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create(string PostId)
        {
            ViewData["PostId"] = PostId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Commentaire,User,PostId")] Comment comment)
        {
            // Retrait de l'erreur dans le modèle pour le manque de l'object de navigation
            ModelState.Remove("Post");

            if (ModelState.IsValid)
            {
                var post = await _context.Posts.Where(w => w.Id == comment.PostId).FirstOrDefaultAsync();
                post.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // la fonction Index s'attend a un object nommé id ...
                return RedirectToAction(nameof(Index), new { id = comment.PostId });
            }

            // la fonction Index s'attend a un object nommé id ...
            return RedirectToAction(nameof(Index), new { id = comment.PostId });
        }

        // Function pour ajouter un like a un Comment
        public async Task<ActionResult> Like(string CommentId, string PostId)
        {
            // Utiliation du null-forgiving operator
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

            var comment = await _context.Comments.FindAsync(CommentId);
            comment!.IncrementLike();

            await _context.SaveChangesAsync();

            // la fonction Index s'attend a un object nommé id ...
            return RedirectToAction(nameof(Index), new { id = PostId });
        }

        // Fonction pour ajouter un dislike a un Comment
        public async Task<ActionResult> Dislike(string CommentId, string PostId)
        {
            // Utiliation du null-forgiving operator
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

            var comment = await _context.Comments.FindAsync(CommentId);
            comment!.IncrementDislike();

            await _context.SaveChangesAsync();

            // la fonction Index s'attend a un object nommé id ...
            return RedirectToAction(nameof(Index), new { id = PostId });
        }
    }
}
