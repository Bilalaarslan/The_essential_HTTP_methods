using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        //In memory data sets were prepared to perform the necessary API tests.
        public static List<Book> Books { get; set; } = new List<Book>()

            {
                new Book(){Id=1, Title="The Great Gatsby", Price=75},
                new Book(){Id=2, Title="Pride and Prejudice", Price=60},
                new Book(){Id=3, Title="The Catcher in the Rye", Price=85},
            };

    }
}
