using System;

namespace izpit14032023
{
    internal class Program
    {
        static Random rnd = new Random();
        static List<Book> books = new List<Book>();
        static void Main(string[] args)
        {
            Task task1 = new Task(AddBooksToList);
            Task task2 = new Task(FilterCheapBooks);
            Task task3 = new Task(PrintBookList);

            task1.Start();
            task1.Wait();

            task2.Start();
            Task.WaitAll(task2, task1);

            task3.Start();

            while (Console.ReadKey().Key != ConsoleKey.Spacebar) ;
        }
        static void AddBooksToList()
        {
            Console.WriteLine("Currently running task id - {0}   [AddBooksToList]", Task.CurrentId);
            for (int i = 0; i < 5; i++)
            {
                Book temp_book = new Book() { Title = "Book_" + i, Price = rnd.Next(50) };
                books.Add(temp_book);
                Console.WriteLine("[{0}] A book has been added to the list.", Task.CurrentId);
            }
        }
        static void FilterCheapBooks()
        {
            Console.WriteLine("Currently running task id - {0}   [FilterCheapBooks]", Task.CurrentId);
            for(int i = 0; i < books.Count; i++)
            {
                if (books[i].Price < 25)
                {
                    books.Remove(books[i]);
                    Console.WriteLine("[{0}] A book has been removed from the list.", Task.CurrentId);
                    i--;
                }
            }
        }

        static void PrintBookList()
        {
            Console.WriteLine("Currently running task id - {0}   [PrintBookList]", Task.CurrentId);
            foreach (Book book in books)
            {
                Console.WriteLine(book);
            }
        }
    }
    class Book
    {
        public string Title { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Title} costs {Price}";
        }
    }
}