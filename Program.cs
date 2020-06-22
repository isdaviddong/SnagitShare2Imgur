using System;

namespace SnagitShare2Imgur
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                Console.WriteLine($"\n{item}");
            }
            Console.ReadKey();
        }
    }
}
