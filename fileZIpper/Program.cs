using System;
using System.IO;
using 
public class Program
{
    public static void Main(string[] args)
    {
        string pathInput, pathOutput;
        FileZipper filer = new FileZipper();
        MainMenue menu = new MainMenue();
        menu.DisplayMenue();
        char c = Console.ReadKey().KeyChar;
        Console.WriteLine("\nChoose the path where you want to save your file: ");
        pathOutput = Console.ReadLine();
        Console.WriteLine("Enter the path of the file: ");
        pathInput = Console.ReadLine();
        using (StreamReader outputfile = new StreamReader(pathOutput))
        using (StreamWriter inputfile = new StreamWriter(pathInput))
        {
            if (c == '1')
            {
                filer.Compress(pathInput);
            }
            else if (c == '0')
            {
                filer.Decompress(outputfile, inputfile);
            }
            else
            {
                Console.WriteLine("This service isn't available");
            }
        }
    }
}

