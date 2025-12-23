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
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string sql = "SELECT * FROM authors";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine();
                            Console.WriteLine("=== Authors ===");
                            
                            while (reader.Read())
                            {
                                // ISSUE: No null checking, magic numbers for column indices, concatenation instead of string interpolation
                                string firstName = reader[1] != DBNull.Value ? reader[1].ToString() : "";
                                string lastName = reader[2] != DBNull.Value ? reader[2].ToString() : "";
                                string city = reader[3] != DBNull.Value ? reader[3].ToString() : "";
                                Console.WriteLine(firstName + " " + lastName + " - " + city);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while listing authors: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        static void FindBooksByPrice()
        {
            try
            {
                Console.Write("Enter minimum price: ");
                string min = Console.ReadLine();
                Console.Write("Enter maximum price: ");
                string max = Console.ReadLine();

                // ISSUE: No validation - what if user enters "abc"?
                decimal minPrice = decimal.Parse(min);
                decimal maxPrice = decimal.Parse(max);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // ISSUE: Critical SQL Injection vulnerability!
                    string sql = "SELECT title, price FROM titles WHERE price >= " + min + " AND price <= " + max;
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine();
                            Console.WriteLine("=== Books ===");

                            while (reader.Read())
                            {
                                // ISSUE: Magic numbers, no null checking
                                string title = reader[0] != DBNull.Value ? reader[0].ToString() : "";
                                string price = reader[1] != DBNull.Value ? reader[1].ToString() : "0";
                                Console.WriteLine(title + " - $" + price);
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid price format. Please enter valid numeric values.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while searching for books: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        static void AddSale()
        {
            try
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
                int quantity = int.Parse(qty);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // ISSUE: Multiple problems - SQL Injection, hardcoded date, magic string "Net 30"
                    string sql = "INSERT INTO sales (stor_id, ord_num, ord_date, qty, payterms, title_id) VALUES ('"
                        + storeId + "', '" + orderNum + "', '2024-01-01', " + qty + ", 'Net 30', '" + titleId + "')";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Sale added!");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid quantity format. Please enter a valid number.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while adding sale: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        static void UpdateBookPrice()
        {
            try
            {
                Console.Write("Enter title ID: ");
                string titleId = Console.ReadLine();
                Console.Write("Enter new price: ");
                string price = Console.ReadLine();

                // ISSUE: No validation
                decimal newPrice = decimal.Parse(price);

                using (SqlConnection c = new SqlConnection(connStr))
                {
                    c.Open();

                    // ISSUE: Critical SQL Injection vulnerability
                    string query = "UPDATE titles SET price = " + price + " WHERE title_id = '" + titleId + "'";
                    using (SqlCommand command = new SqlCommand(query, c))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Price updated!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid price format. Please enter a valid numeric value.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while updating price: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        static void GenerateReport()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    // ISSUE: Old-style JOIN syntax, inefficient query without aggregation
                    string sql = "SELECT s.stor_id, st.stor_name, t.title, s.qty, t.price FROM sales s, stores st, titles t WHERE s.stor_id = st.stor_id AND s.title_id = t.title_id";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            double total = 0;

                            Console.WriteLine();
                            Console.WriteLine("=== Sales Report ===");

                            while (r.Read())
                            {
                                // ISSUE: Magic numbers, no null checking, manual calculations
                                string storeName = r[1] != DBNull.Value ? r[1].ToString() : "";
                                string title = r[2] != DBNull.Value ? r[2].ToString() : "";
                                int quantity = r[3] != DBNull.Value ? Convert.ToInt32(r[3]) : 0;
                                double price = r[4] != DBNull.Value ? Convert.ToDouble(r[4]) : 0.0;
                                double subtotal = quantity * price;
                                total = total + subtotal;

                                // ISSUE: Poor formatting, concatenation
                                Console.WriteLine(storeName + " - " + title + " - Qty: " + quantity + " - $" + subtotal);
                            }

                            Console.WriteLine("---");
                            Console.WriteLine("Total Sales: $" + total);
                        }
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine("Data conversion error in report: " + ex.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while generating report: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}