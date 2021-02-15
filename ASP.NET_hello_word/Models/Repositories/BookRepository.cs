using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_hello_word.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {

        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book{Id = 1 , Title = "C# Programming" , Description="No description"},
                new Book{Id = 2 , Title = "java Programming" , Description="No description"},
                new Book{Id = 3 , Title = "PHP Programming" , Description="No description"},
            };

        }

        public void Add(Book book)
        {
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
        }

        public void Delete(int id)
        {
            books.Remove(Find(id));
        }

        public Book Find(int id)
        {
            var book = books.Find(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update( int id ,  Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;

        }
    }
}
