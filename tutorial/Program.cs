using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i, j, r= 6; // r = 6 is the half of diamondLength
            for (i = 0; i <= r; i++)
            {
                for (j = 1; j <= r - i; j++)
                {
                    Console.Write(" ");
                }
                for (j = 1; j <= 2 * i - 1; j++)
                {
                    Console.Write("*");
                }
                Console.Write("\n"); 
            }

           
            for (i = r - 1; i >= 1; i--)
            {
                for (j = 1; j <= r - i; j++)
                {
                    Console.Write(" ");
                }
                for (j = 1; j <= 2 * i - 1; j++)
                {
                    Console.Write("*");
                }
                Console.Write("\n");
            }
        }
    }
}