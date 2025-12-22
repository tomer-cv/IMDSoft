using System;
using System.Data.SqlClient;

namespace PubsBookManager
{
    class Program
    {
        // TODO: This should be in config file
        static string connStr = @"Server=(localdb)\MSSQLLocalDB;Database=pubs;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
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

        static void ListAuthors()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = "SELECT * FROM authors";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine();
            Console.WriteLine("=== Authors ===");
            
            while (reader.Read())
            {
                // ISSUE: No null checking, magic numbers for column indices, concatenation instead of string interpolation
                Console.WriteLine(reader[1].ToString() + " " + reader[2].ToString() + " - " + reader[3].ToString());
            }

            // ISSUE: No disposal of resources - reader, cmd, conn not disposed
        }

        static void FindBooksByPrice()
        {
            Console.Write("Enter minimum price: ");
            string min = Console.ReadLine();
            Console.Write("Enter maximum price: ");
            string max = Console.ReadLine();

            // ISSUE: No validation - what if user enters "abc"?

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string sql = "SELECT title, price FROM titles WHERE price >= @MinPrice AND price <= @MaxPrice";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MinPrice", min);
            cmd.Parameters.AddWithValue("@MaxPrice", max);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine();
            Console.WriteLine("=== Books ===");

            while (reader.Read())
            {
                // ISSUE: Magic numbers, no null checking
                Console.WriteLine(reader[0] + " - $" + reader[1]);
            }

            conn.Close();
            // ISSUE: cmd and reader not disposed
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

            // ISSUE: No validation - empty strings, invalid formats accepted

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            // ISSUE: hardcoded date, magic string "Net 30"
            string sql = "INSERT INTO sales (stor_id, ord_num, ord_date, qty, payterms, title_id) VALUES (@StoreId, @OrderNum, '2024-01-01', @Qty, 'Net 30', @TitleId)";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@StoreId", storeId);
            cmd.Parameters.AddWithValue("@OrderNum", orderNum);
            cmd.Parameters.AddWithValue("@Qty", qty);
            cmd.Parameters.AddWithValue("@TitleId", titleId);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Sale added!");
            }
            catch (Exception ex)
            {
                // ISSUE: Swallowing exceptions - no logging, generic error message
                Console.WriteLine("Error");
            }

            conn.Close();
        }

        static void UpdateBookPrice()
        {
            Console.Write("Enter title ID: ");
            string titleId = Console.ReadLine();
            Console.Write("Enter new price: ");
            string price = Console.ReadLine();

            // ISSUE: No validation

            SqlConnection c = new SqlConnection(connStr);
            c.Open();

            string query = "UPDATE titles SET price = @Price WHERE title_id = @TitleId";
            SqlCommand command = new SqlCommand(query, c);
            command.Parameters.AddWithValue("@Price", price);
            command.Parameters.AddWithValue("@TitleId", titleId);
            command.ExecuteNonQuery();

            Console.WriteLine("Price updated!");

            c.Close();
            // ISSUE: No error handling at all
        }

        static void GenerateReport()
        {
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();

            // ISSUE: Old-style JOIN syntax, inefficient query without aggregation
            string sql = "SELECT s.stor_id, st.stor_name, t.title, s.qty, t.price FROM sales s, stores st, titles t WHERE s.stor_id = st.stor_id AND s.title_id = t.title_id";

            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader r = cmd.ExecuteReader();

            double total = 0;

            Console.WriteLine();
            Console.WriteLine("=== Sales Report ===");

            while (r.Read())
            {
                // ISSUE: Magic numbers, no null checking, manual calculations
                string storeName = r[1].ToString();
                string title = r[2].ToString();
                int quantity = Convert.ToInt32(r[3]);
                double price = Convert.ToDouble(r[4]);
                double subtotal = quantity * price;
                total = total + subtotal;

                // ISSUE: Poor formatting, concatenation
                Console.WriteLine(storeName + " - " + title + " - Qty: " + quantity + " - $" + subtotal);
            }

            Console.WriteLine("---");
            Console.WriteLine("Total Sales: $" + total);

            connection.Close();
        }
    }
}