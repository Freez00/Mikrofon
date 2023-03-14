using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Book
{
    public string Title { get; set; }
    public double Price { get; set; }
}

public class BookList
{
    private List<Book> books = new List<Book>();

    public async Task AddBooksAsync()
    {
        await Task.Run(() =>
        {
            books.Add(new Book { Title = "Book 1", Price = 15.99 });
            books.Add(new Book { Title = "Book 2", Price = 20.50 });
            books.Add(new Book { Title = "Book 3", Price = 30.00 });
        });
        Console.WriteLine($"Task {Task.CurrentId} added books to the list.");
    }

    public async Task RemoveCheapBooksAsync()
    {
        await Task.Run(() =>
        {
            books.RemoveAll(b => b.Price < 25);
        });
        Console.WriteLine($"Task {Task.CurrentId} removed cheap books from the list.");
    }

    public async Task PrintBooksAsync()
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"Task {Task.CurrentId} printing books list:");
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title} - {book.Price}");
            }
        });
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var bookList = new BookList();
        var addTask = bookList.AddBooksAsync();
        var removeTask = bookList.RemoveCheapBooksAsync();
        await Task.WhenAll(addTask, removeTask);

        await bookList.PrintBooksAsync();
    }
}