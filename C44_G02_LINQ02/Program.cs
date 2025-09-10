using C44_G02_LINQ02.Data;
using System.Diagnostics.CodeAnalysis;
// This using statement allows direct access to the static lists
using static C44_G02_LINQ02.Data.ListGenerator;

namespace C44_G02_LINQ02
{
    // Custom comparer for the anagram grouping problem
    public class AnagramEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (x is null || y is null) return false;
            return getCanonicalString(x) == getCanonicalString(y);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return getCanonicalString(obj).GetHashCode();
        }

        private string getCanonicalString(string word)
        {
            char[] wordChars = word.ToCharArray();
            Array.Sort(wordChars);
            return new string(wordChars);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Helper method to print collections
            void Print<T>(IEnumerable<T> collection)
            {
                foreach (var item in collection)
                {
                    Console.WriteLine(item);
                }
            }

            #region LINQ - Element Operators
            Console.WriteLine("## LINQ - Element Operators ##\n");

            #region Q1: Get first Product out of Stock
            Console.WriteLine("1. First product out of stock:");
            var firstOutOfStock = ProductList.First(p => p.UnitsInStock == 0);
            Console.WriteLine(firstOutOfStock);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Return the first product whose Price > 1000, or null
            Console.WriteLine("2. First product with price > 1000 (or null):");
            var productOver1000 = ProductList.FirstOrDefault(p => p.UnitPrice > 1000M);
            Console.WriteLine(productOver1000 is null ? "No product found with price > 1000" : productOver1000.ToString());
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Retrieve the second number greater than 5
            Console.WriteLine("3. Second number greater than 5:");
            int[] Arr1 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var secondNumberGreaterThan5 = Arr1.Where(n => n > 5).Skip(1).First();
            Console.WriteLine(secondNumberGreaterThan5);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion

            #region LINQ - Aggregate Operators
            Console.WriteLine("\n## LINQ - Aggregate Operators ##\n");

            // NOTE: The dictionary file is not provided, so a sample array is created.
            string[] dictionary = { "apple", "banana", "cherry", "blueberry", "date", "grapefruit", "substantiate", "their", "receive" };

            #region Q1: Get the number of odd numbers in the array
            Console.WriteLine("1. Number of odd numbers:");
            int[] Arr2 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var oddCount = Arr2.Count(n => n % 2 != 0);
            Console.WriteLine(oddCount);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Return a list of customers and how many orders each has
            Console.WriteLine("2. List of customers and their order counts:");
            var customerOrderCounts = CustomerList.Select(c => new { c.CustomerName, OrderCount = c.Orders.Length });
            Print(customerOrderCounts);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Return a list of categories and how many products each has
            Console.WriteLine("3. List of categories and their product counts:");
            var categoryProductCounts = ProductList.GroupBy(p => p.Category)
                                                   .Select(g => new { CategoryName = g.Key, ProductCount = g.Count() });
            Print(categoryProductCounts);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q4: Get the total of the numbers in an array
            Console.WriteLine("4. Sum of numbers in the array:");
            var sumOfNumbers = Arr2.Sum();
            Console.WriteLine(sumOfNumbers);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q5: Get the total number of characters of all words in dictionary
            Console.WriteLine("5. Total characters in all dictionary words:");
            var totalChars = dictionary.Sum(w => w.Length);
            Console.WriteLine(totalChars);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q6: Get the length of the shortest word in dictionary
            Console.WriteLine("6. Length of the shortest word:");
            var shortestWordLength = dictionary.Min(w => w.Length);
            Console.WriteLine(shortestWordLength);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q7: Get the length of the longest word in dictionary
            Console.WriteLine("7. Length of the longest word:");
            var longestWordLength = dictionary.Max(w => w.Length);
            Console.WriteLine(longestWordLength);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q8: Get the average length of the words in dictionary
            Console.WriteLine("8. Average word length:");
            var averageWordLength = dictionary.Average(w => w.Length);
            Console.WriteLine(averageWordLength);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q9: Get the total units in stock for each product category
            Console.WriteLine("9. Total units in stock per category:");
            var categoryUnitTotals = ProductList.GroupBy(p => p.Category)
                                                .Select(g => new { CategoryName = g.Key, TotalUnits = g.Sum(p => p.UnitsInStock) });
            Print(categoryUnitTotals);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q10: Get the cheapest price among each category's products
            Console.WriteLine("10. Cheapest price per category:");
            var cheapestPrices = ProductList.GroupBy(p => p.Category)
                                            .Select(g => new { CategoryName = g.Key, CheapestPrice = g.Min(p => p.UnitPrice) });
            Print(cheapestPrices);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q11: Get the products with the cheapest price in each category (Use Let)
            Console.WriteLine("11. Cheapest product(s) in each category:");
            var cheapestProducts = from p in ProductList
                                   group p by p.Category into g
                                   let minPrice = g.Min(p => p.UnitPrice)
                                   select new
                                   {
                                       Category = g.Key,
                                       Products = g.Where(p => p.UnitPrice == minPrice)
                                   };
            foreach (var item in cheapestProducts)
            {
                Console.WriteLine($"Category: {item.Category}");
                Print(item.Products);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q12: Get the most expensive price among each category's products
            Console.WriteLine("12. Most expensive price per category:");
            var mostExpensivePrices = ProductList.GroupBy(p => p.Category)
                                                 .Select(g => new { CategoryName = g.Key, MostExpensivePrice = g.Max(p => p.UnitPrice) });
            Print(mostExpensivePrices);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q13: Get the products with the most expensive price in each category
            Console.WriteLine("13. Most expensive product(s) in each category:");
            var mostExpensiveProducts = ProductList.GroupBy(p => p.Category)
                                                   .Select(g => new
                                                   {
                                                       Category = g.Key,
                                                       Products = g.Where(p => p.UnitPrice == g.Max(pr => pr.UnitPrice))
                                                   });

            foreach (var item in mostExpensiveProducts)
            {
                Console.WriteLine($"Category: {item.Category}");
                Print(item.Products);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q14: Get the average price of each category's products
            Console.WriteLine("14. Average price per category:");
            var averagePrices = ProductList.GroupBy(p => p.Category)
                                           .Select(g => new { CategoryName = g.Key, AveragePrice = g.Average(p => p.UnitPrice) });
            Print(averagePrices);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion

            #region LINQ - Set Operators
            Console.WriteLine("\n## LINQ - Set Operators ##\n");

            #region Q1: Find the unique Category names from Product List
            Console.WriteLine("1. Unique category names:");
            var uniqueCategories = ProductList.Select(p => p.Category).Distinct();
            Print(uniqueCategories);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Produce a Sequence containing the unique first letter from both product and customer names
            Console.WriteLine("2. Unique first letters from product and customer names (Union):");
            var productFirstLetters = ProductList.Select(p => p.ProductName[0]);
            var customerFirstLetters = CustomerList.Select(c => c.CustomerName[0]);
            var unionFirstLetters = productFirstLetters.Union(customerFirstLetters);
            Print(unionFirstLetters);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Create one sequence that contains the common first letter from both product and customer names
            Console.WriteLine("3. Common first letters (Intersect):");
            var intersectFirstLetters = productFirstLetters.Intersect(customerFirstLetters);
            Print(intersectFirstLetters);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q4: Create one sequence that contains the first letters of product names that are not also first letters of customer names
            Console.WriteLine("4. First letters in products but not customers (Except):");
            var exceptFirstLetters = productFirstLetters.Except(customerFirstLetters);
            Print(exceptFirstLetters);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q5: Create one sequence that contains the last three characters in each name of all customers and products, including any duplicates
            Console.WriteLine("5. Last three characters of all names (with duplicates):");
            var productLastThree = ProductList.Select(p => p.ProductName.Length >= 3 ? p.ProductName.Substring(p.ProductName.Length - 3) : p.ProductName);
            var customerLastThree = CustomerList.Select(c => c.CustomerName.Length >= 3 ? c.CustomerName.Substring(c.CustomerName.Length - 3) : c.CustomerName);
            var allLastThree = productLastThree.Concat(customerLastThree);
            Print(allLastThree);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion

            #region LINQ - Partitioning Operators
            Console.WriteLine("\n## LINQ - Partitioning Operators ##\n");

            #region Q1: Get the first 3 orders from customers in Washington
            Console.WriteLine("1. First 3 orders from Washington:");
            var first3WAOrders = CustomerList.Where(c => c.Region == "WA")
                                             .SelectMany(c => c.Orders)
                                             .Take(3);
            Print(first3WAOrders);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Get all but the first 2 orders from customers in Washington
            Console.WriteLine("2. All but first 2 orders from Washington:");
            var skip2WAOrders = CustomerList.Where(c => c.Region == "WA")
                                            .SelectMany(c => c.Orders)
                                            .Skip(2);
            Print(skip2WAOrders);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Return elements until a number is hit that is less than its position
            Console.WriteLine("3. TakeWhile number is >= index:");
            int[] numbers1 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var takeWhileResult = numbers1.TakeWhile((n, index) => n >= index);
            Print(takeWhileResult);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q4: Get the elements of the array starting from the first element divisible by 3
            Console.WriteLine("4. SkipWhile number is not divisible by 3:");
            var skipWhileNotDivBy3 = numbers1.SkipWhile(n => n % 3 != 0);
            Print(skipWhileNotDivBy3);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q5: Get the elements of the array starting from the first element less than its position
            Console.WriteLine("5. SkipWhile number is >= index:");
            var skipWhileResult = numbers1.SkipWhile((n, index) => n >= index);
            Print(skipWhileResult);
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion

            #region LINQ - Quantifiers
            Console.WriteLine("\n## LINQ - Quantifiers ##\n");

            #region Q1: Determine if any of the words in the dictionary contain the substring 'ei'
            Console.WriteLine("1. Any word contains 'ei'?");
            bool hasEi = dictionary.Any(w => w.Contains("ei"));
            Console.WriteLine(hasEi ? "Yes" : "No");
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Return a grouped list of products only for categories that have at least one product that is out of stock
            Console.WriteLine("2. Categories with at least one out-of-stock product:");
            var categoriesWithOutOfStock = ProductList.GroupBy(p => p.Category)
                                                      .Where(g => g.Any(p => p.UnitsInStock == 0));
            foreach (var group in categoriesWithOutOfStock)
            {
                Console.WriteLine($"Category: {group.Key}");
                Print(group);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Return a grouped list of products only for categories that have all of their products in stock
            Console.WriteLine("3. Categories with all products in stock:");
            var categoriesAllInStock = ProductList.GroupBy(p => p.Category)
                                                  .Where(g => g.All(p => p.UnitsInStock > 0));
            foreach (var group in categoriesAllInStock)
            {
                Console.WriteLine($"Category: {group.Key}");
                Print(group);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion

            #region LINQ - Grouping Operators
            Console.WriteLine("\n## LINQ - Grouping Operators ##\n");

            #region Q1: Use group by to partition a list of numbers by their remainder when divided by 5
            Console.WriteLine("1. Group numbers by remainder when divided by 5:");
            List<int> numbers2 = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var groupedByRemainder = numbers2.GroupBy(n => n % 5);
            foreach (var group in groupedByRemainder)
            {
                Console.WriteLine($"Numbers with a remainder of {group.Key} when divided by 5:");
                Print(group);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q2: Uses group by to partition a list of words by their first letter
            Console.WriteLine("2. Group dictionary words by first letter:");
            var groupedByFirstLetter = dictionary.GroupBy(w => w[0]);
            foreach (var group in groupedByFirstLetter)
            {
                Console.WriteLine($"Words starting with '{group.Key}':");
                Print(group);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #region Q3: Use Group By with a custom comparer to find anagrams
            Console.WriteLine("3. Group words that are anagrams:");
            string[] Arr3 = { "from", "salt", "earn", "last", "near", "form" };
            var anagramGroups = Arr3.GroupBy(w => w, new AnagramEqualityComparer());
            foreach (var group in anagramGroups)
            {
                Console.WriteLine("--- Anagram Group ---");
                Print(group);
            }
            Console.WriteLine("---------------------------------\n");
            #endregion

            #endregion
        }
    }
}