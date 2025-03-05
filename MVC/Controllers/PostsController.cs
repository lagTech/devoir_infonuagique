// PostsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch posts
            var posts = await _context.Posts
                .OrderByDescending(o => o.Created)
                .Take(10)
                .ToListAsync();

            // Fetch comments (client-side processing for grouping/counting)
            var postIds = posts.Select(p => p.Id).ToList();

            var comments = await _context.Comments
                .Where(c => postIds.Contains(c.PostId))
                .ToListAsync();

            // Group comments by PostId and count them
            var commentCounts = comments
                .GroupBy(c => c.PostId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Combine posts with their respective comment counts
            var postsWithCommentCounts = posts.Select(post => (
                Post: post,
                CommentCount: commentCounts.ContainsKey(post.Id) ? commentCounts[post.Id] : 0
            )).ToList();

            // Pass the data to the view
            return View(postsWithCommentCounts);
        }




        public async Task<IActionResult> Details(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }


        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Category,User,Created,FileToUpload")] PostForm postForm)
        {

            //Conversion du fichier recu and IFormFile and Byte[]. L'utilisation ici du modèle d'héritage avec la propriété supplémentaire sert de passerelle.
            using (MemoryStream ms = new MemoryStream())
            {
                if (ms.Length < 40971520)
                {
                    await postForm.FileToUpload.CopyToAsync(ms);
                    postForm.Image = ms.ToArray();

                    //retrait de l'erreur du au manque de l'imnage, celle-ci fut ajouter au model de base par notre CopyToAsync.
                    ModelState.Remove("Image");
                }
                else
                {
                    //ajout d'une erreur si le fichier est trop gros
                    ModelState.AddModelError("FileToUpload", "Le fichier est trop gros.");
                }

            }

            if (ModelState.IsValid)
            {
                //le format de l'object envoyer ici va utiliser le polymorphise pour revenir a ça forme de base, ainsi il perdra le IFormFile.
                _context.Add(postForm);


                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(postForm);
        }

        // Function pour ajouter un like a un Post
        public async Task<ActionResult> Like(string id)
        {
            // Utiliation du null-forgiving operator
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            post!.IncrementLike();
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Fonction pour ajouter un dislike a un Post
        public async Task<ActionResult> Dislike(string id)
        {
            // Utiliation du null-forgiving operator
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();


            post!.IncrementDislike();
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
