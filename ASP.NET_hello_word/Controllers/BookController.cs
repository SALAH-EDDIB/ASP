using ASP.NET_hello_word.Models;
using ASP.NET_hello_word.Models.Repositories;
using ASP.NET_hello_word.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_hello_word.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController( IBookStoreRepository<Book> bookRepository , IBookStoreRepository<Author> authorRepository , IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }

        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
          var book =  bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel {
                Authors = FillSelectList()
            };

            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    string filename = "";
                    if(model.File != null)
                    {
                        string uploads = Path.Combine(hosting.WebRootPath , "uploads");
                        filename = model.File.FileName;
                        string fullPath = Path.Combine(uploads ,filename);
                        model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }


                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please Select An Author From the List ";
                      
                        return View(GetViewModel());
                    }

                    var book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = authorRepository.Find(model.AuthorId),
                        ImageUrl = filename 

                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all the fields");
          
            return View(GetViewModel());


        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);

            var authorId = book.Author == null ? 0 : book.Author.Id;
            var model = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl
            };

            return View(model);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model)
        {
            try
            {

                string filename = bookRepository.Find(model.BookId).ImageUrl;

                

                if (model.File != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    filename = model.File.FileName;
                    string fullPath = Path.Combine(uploads, filename);
                    string oldFile = bookRepository.Find(model.BookId).ImageUrl;
                    System.IO.File.Delete(Path.Combine(uploads, oldFile));
                    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }



                var book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = authorRepository.Find(model.AuthorId),
                    ImageUrl = filename

                };

                bookRepository.Update(id, book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {

            var book = bookRepository.Find(id);

            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Select an Author --- " });
            return authors;
        }

        BookAuthorViewModel GetViewModel()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };

            return model;
        }

    }
}
