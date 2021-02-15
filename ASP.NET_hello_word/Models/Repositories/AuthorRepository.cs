using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_hello_word.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {


        List<Author> Authors;
        public AuthorRepository()
        {
            Authors = new List<Author>()
            {
                new Author{Id = 1 , FullName = "salah eddib" },
                new Author{Id = 2 , FullName = "ahmed omar" },
                new Author{Id = 3 , FullName = "achraf yassine" },
            };

        }

        public void Add(Author author)
        {
            Authors.Add(author);
        
        }

        public void Delete(int id)
        {
            Authors.Remove(Find(id));
        }

        public Author Find(int id)
        {
            var Author = Authors.Find(b => b.Id == id);
            return Author;
        }

        public IList<Author> List()
        {
            return Authors;
        }

        public void Update(int id, Author newAuthor)
        {
            var Author = Find(id);
            Author.FullName = newAuthor.FullName;
            

        }
    }
}
