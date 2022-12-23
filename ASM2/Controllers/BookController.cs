using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM2.Data;
using ASM2.Models;
using Newtonsoft.Json;

namespace DisplayImgDemo.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BookController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;

        }


        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var employee = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Books/Create

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Name");
           
            return View();
        }
       

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model)
        {
          
            try
            {
                Console.WriteLine(model);
                var category = await _context.BookCategories.FindAsync(model.Category);
                Book book = new Book
                {
                    Title = model.Title,
                    ReleaseDate = model.ReleaseDate,
                    Price = model.Price,
                    Image = model.Image,
                    CategoryName = model.Category.Name,
                    Category = category,

                };
                await _context.Books.AddAsync(book);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return RedirectToAction("Index");

            }
            
            return RedirectToAction("Index");

        }



        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var employee = await _context.Books.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Title,ReleaseDate,Price,Category,Image,BookCatId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if ( book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        public Book getDetailBook(int id)
        {
            var book = _context.Books.Find(id);
            return book;
        }

        public IActionResult AddCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                var book = getDetailBook(id);
                List<Cart> ListCart = new List<Cart>()
                {
                    new Cart {Books = book, Quantity= 1},
                };

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
            }
            else
            {
                List<Cart> ListCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = false;
                for(int i = 0; i < ListCart.Count; i++)
                {
                    if(ListCart[i].Books.Id == id)
                    {
                        ListCart[i].Quantity++;
                        check = true;

                    }
                }

                if (check)
                {
                    ListCart.Add(new Cart
                    {
                        Books = getDetailBook(id),
                        Quantity = 1
                    });
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart() {
            var cart = HttpContext.Session.GetString("cart");
            if(cart != null)
            {
                List<Cart> listCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if(listCart.Count > 0)
                {
                    ViewBag.Carts = listCart;
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return RedirectToAction(nameof(Index));

        }

    }
}
