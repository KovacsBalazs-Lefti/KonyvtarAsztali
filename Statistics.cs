using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztali
{
    internal class Statistics
    {
        static List<Book> books;
        public static void Run() 
        {
            try
            {
                ReadAllBooks();
                Console.WriteLine("500 oldalnál hosszabb könyvek száma: {0}", CountLongerThan500Pages());
                Console.WriteLine("{0} 1950-nél réggebi könyv", OlderThan1950IsPresent() ? "Van" : "Nincs");
                Book longest = GetLongestBook();
                Console.WriteLine("A leghosszabb könyv:" + 
                    "\r\n\t Szerző: {0}" +
                    "\r\n\t Cím: {1}" +
                    " \r\n\t Kiadás éve: {2}" +
                    "\r\n\t Oldalszám:{3}", longest.Author, longest.Title, longest.Publish_year, longest.Page_count);
                Console.WriteLine("A legtöbb könyvvel rendelkező szerző: {0}", GetAuthorwithMostBooks());
                Console.Write("Adjon meg egy könyv címet: ");
                string title = Console.ReadLine();
                PrintAuthor(title);
                //teszt, hogy jól csináltam a dolgokat eddig:
                //books.ForEach(book => Console.WriteLine(book.Title));
                Console.ReadKey();
            }
            catch (MySqlException ex)
            {

                Console.WriteLine("Hiba történt az adatbázis kapcsolat kiépítésekor: ");
                Console.WriteLine(ex.Message);
            }
        }

        private static void PrintAuthor(string title)
        {
            int index = 0;
            while (index < books.Count && books[index].Title != title)
            {
                index++;
            }
            if (index < books.Count)
            {
                Console.WriteLine("A megadott könyv szerzője: {0}", books[index].Author);
            }
            else
            {
                Console.WriteLine("Nincs ilyen könyv");
            }
        }

        private static string GetAuthorwithMostBooks()
        {
            Dictionary<string, int> authorBookCount = new Dictionary<string, int>();
            foreach (Book book in books)
            {
                if (!authorBookCount.ContainsKey(book.Author))
                {
                    authorBookCount[book.Author] = 0;
                }
                authorBookCount[book.Author]++;
            }
            string author = null;
            foreach (KeyValuePair<string, int> item in authorBookCount)
            {
                if (author == null)
                {
                    author = item.Key;
                }
                if (item.Value > authorBookCount[author])
                {
                    author = item.Key;
                }
            }
            return author;
        }

        private static Book GetLongestBook()
        {
            Book longest = books[0];
            for (int i = 1; i < books.Count; i++)
            {
                if (books[i].Page_count > longest.Page_count)
                {
                    longest = books[i];
                }
            }
            return longest;
        }

        private static bool OlderThan1950IsPresent()
        {
            int index = 0;
            while (index< books.Count && !(books[index].Publish_year < 1950))
            {
                index++;
            }
            return index < books.Count;
        }

        private static int CountLongerThan500Pages()
        {
            int count = 0;
            foreach (Book book in books)
            {
                if (book.Page_count > 500)
                {
                    count++;
                }
            }
            return count;
        }

        private static void ReadAllBooks()
        {
            BookService bookService = new BookService();
            books = bookService.GetBooks();
        }
    }
}

