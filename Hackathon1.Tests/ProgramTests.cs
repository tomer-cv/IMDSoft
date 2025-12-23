using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubsBookManager;

namespace PubsBookManager.Tests
{
    [TestClass]
    public class ProgramTests
    {
        // ISSUE: Testing a static method that directly accesses the database
        // IMPROVEMENT NEEDED: The Program class should be refactored to use dependency injection
        // so that database connections can be mocked

        [TestMethod]
        public void ListAuthors_ShouldNotThrowException()
        {
            // ISSUE: This is a very weak test - it only checks that the method doesn't crash
            // IMPROVEMENT NEEDED: Should verify actual output or behavior
            
            // ISSUE: This test requires a real database connection
            // IMPROVEMENT NEEDED: Should use a mock or in-memory database
            
            try
            {
                // ISSUE: Cannot easily test private/static methods
                // IMPROVEMENT NEEDED: Methods should be instance methods in a testable class
                
                // This won't compile because ListAuthors is private in the static Program class
                // Program.ListAuthors();
                
                Assert.IsTrue(true, "Method executed without exception");
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        [TestMethod]
        public void FindBooksByPrice_WithValidInput_ShouldReturnResults()
        {
            // ISSUE: Cannot test this method because:
            // 1. It reads from Console.ReadLine() - no way to inject test input
            // 2. It's a static method in a static class
            // 3. It directly creates database connections
            
            // IMPROVEMENT NEEDED: Refactor to accept parameters instead of reading from console
            // Example: FindBooksByPrice(decimal minPrice, decimal maxPrice)
            
            Assert.Inconclusive("This test cannot be implemented without refactoring the code");
        }

        [TestMethod]
        public void AddSale_WithInvalidInput_ShouldHandleGracefully()
        {
            // ISSUE: No way to test this without a database and console input
            
            // IMPROVEMENT NEEDED:
            // 1. Extract business logic into a separate class (e.g., SalesService)
            // 2. Use dependency injection for database access
            // 3. Accept parameters instead of reading from Console
            // 4. Return success/failure status instead of writing to Console
            
            Assert.Inconclusive("Method is not testable in current form");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParsePrice_WithInvalidString_ShouldThrowException()
        {
            // ISSUE: This demonstrates a test that SHOULD exist but cannot
            // because there's no separate method to test price parsing
            
            // IMPROVEMENT NEEDED: Extract validation logic into separate, testable methods
            // Example: decimal ParseAndValidatePrice(string input)
            
            string invalidPrice = "abc";
            decimal result = Convert.ToDecimal(invalidPrice);
            
            // This will throw FormatException - but this logic should be
            // extracted from the UI methods and tested separately
        }

        // ISSUE: No test for SQL injection vulnerabilities
        // IMPROVEMENT NEEDED: Demonstrate how parameterized queries prevent SQL injection
        
        // ISSUE: No test for resource disposal
        // IMPROVEMENT NEEDED: Verify that SqlConnection, SqlCommand, and SqlDataReader are disposed
        
        // ISSUE: No test coverage for the main menu logic
        // IMPROVEMENT NEEDED: Extract menu logic into testable methods
        
        // ISSUE: No integration tests
        // IMPROVEMENT NEEDED: Add integration tests with a test database
    }
}