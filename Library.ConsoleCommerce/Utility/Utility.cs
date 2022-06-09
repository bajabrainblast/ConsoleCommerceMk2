using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.ConsoleCommerce.Models;
using Library.ConsoleCommerce.Imports;

namespace Library.ConsoleCommerce.Utility
{
    public static class Utilities
    {
        // takes valid integer input
        public static int TakeInput(int min, int max)
        {
            int input = int.Parse(Console.ReadLine() ?? "-1");

            while (input < min || input > max)
            {
                Console.WriteLine("Invalid input. Please try again -> ");
                input = int.Parse(Console.ReadLine() ?? "-1");
            }

            return input;
        }


        // returns a false when the user is done navigating
        public static bool NavigateList<T>(IEnumerable<T> list)
        {
            ListNavigator<T> nav = new ListNavigator<T>(list);

            while (true)
            {
                // actually display
                foreach (var i in nav.GetCurrentPage())
                    Console.WriteLine($"{i.Value}");

                // choose Q to go back, E to go to next, W to stop
                Console.WriteLine("\nPlease enter Q to go back, E to go forward, or W to stop scrolling.");
                char input = Console.ReadLine().ToUpper()[0];
                switch (input)
                {
                    case 'Q':
                        if (nav.HasPreviousPage)
                            nav.GoBackward();
                        break;

                    case 'E':
                        if (nav.HasNextPage)
                            nav.GoForward();
                        break;

                    case 'W':
                        return false;

                    default:
                        break;
                }
            }
        }
        
    }
}
