using System;

namespace PubsBookManager
{
    class Program
    {
        // TODO: This should be in config file
        static string connStr = @"Server=(localdb)\MSSQLLocalDB;Database=pubs;Integrated Security=True;TrustServerCertificate=True;";
        static DatabaseOperations dbOps;

        static void Main(string[] args)
        {
            dbOps = new DatabaseOperations(connStr);

            try
            {
                // ISSUE: Infinite loop with no proper exit handling
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("=== Pubs Book Manager ===");
                    Console.WriteLine("1. List all authors");
                    Console.WriteLine("2. Find books by price range");
                    Console.WriteLine("3. Add new sale");
                    Console.WriteLine("4. Update book price");
                    Console.WriteLine("5. Generate sales report");
                    Console.WriteLine("0. Exit");
                    Console.Write("Select option: ");

                    string choice = Console.ReadLine();

                    // ISSUE: No input validation, uses if-else chain instead of switch
                    if (choice == "1")
                        ListAuthors();
                    else if (choice == "2")
                        FindBooksByPrice();
                    else if (choice == "3")
                        AddSale();
                    else if (choice == "4")
                        UpdateBookPrice();
                    else if (choice == "5")
                        GenerateReport();
                    else if (choice == "0")
                        break;
                    // ISSUE: Invalid input is silently ignored
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        static void ListAuthors()
        {
            dbOps.ListAuthors();
        }

        static void FindBooksByPrice()
        {
            Console.Write("Enter minimum price: ");
            string min = Console.ReadLine();
            Console.Write("Enter maximum price: ");
            string max = Console.ReadLine();

            dbOps.FindBooksByPrice(min, max);
        }

        static void AddSale()
        {
            Console.Write("Store ID: ");
            string storeId = Console.ReadLine();
            Console.Write("Order Number: ");
            string orderNum = Console.ReadLine();
            Console.Write("Title ID: ");
            string titleId = Console.ReadLine();
            Console.Write("Quantity: ");
            string qty = Console.ReadLine();

            dbOps.AddSale(storeId, orderNum, titleId, qty);
        }

        static void UpdateBookPrice()
        {
            Console.Write("Enter title ID: ");
            string titleId = Console.ReadLine();
            Console.Write("Enter new price: ");
            string price = Console.ReadLine();

            dbOps.UpdateBookPrice(titleId, price);
        }

        static void GenerateReport()
        {
            dbOps.GenerateReport();
        }
    }
}