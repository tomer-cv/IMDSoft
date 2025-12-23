using System;
using System.Data.SqlClient;

namespace PubsBookManager
{
    class DatabaseOperations
    {
        private readonly string connectionString;

        public DatabaseOperations(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void ListAuthors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
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

        public void FindBooksByPrice(string min, string max)
        {
            try
            {
                // ISSUE: No validation - what if user enters "abc"?
                // Validate input format (will throw FormatException if invalid)
                decimal.Parse(min);
                decimal.Parse(max);

                using (SqlConnection conn = new SqlConnection(connectionString))
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

        public void AddSale(string storeId, string orderNum, string titleId, string qty)
        {
            try
            {
                // ISSUE: No validation - empty strings, invalid formats accepted
                // Validate quantity format (will throw FormatException if invalid)
                int.Parse(qty);

                using (SqlConnection conn = new SqlConnection(connectionString))
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

        public void UpdateBookPrice(string titleId, string price)
        {
            try
            {
                // ISSUE: No validation
                // Validate price format (will throw FormatException if invalid)
                decimal.Parse(price);

                using (SqlConnection c = new SqlConnection(connectionString))
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

        public void GenerateReport()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
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
