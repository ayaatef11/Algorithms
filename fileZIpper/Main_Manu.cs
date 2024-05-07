using System;
namespace file_Zipper
{
    public class MainMenue
    {
        public void DisplayMenu()
        {
            Console.WriteLine("\t\t\t\t*FILE ZIPPER*");
            Console.WriteLine("How can I help you ?");
            Console.WriteLine("1-Compress a file");
            Console.WriteLine("2-Decompress a file");
            Console.Write("\t Enter choice:) ");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();
            // You can return 'c' or perform further actions based on the selected option
        }
    }
}