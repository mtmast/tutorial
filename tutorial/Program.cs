using System;

namespace DiamondShape
{
    internal class Program
    {
        public static void GenerateDiamondShape(int diamondLength)
        {
            int outerLoop, innerLoop;
            for (outerLoop = 1; outerLoop <= (diamondLength / 2); outerLoop++)
            {
                for (innerLoop = 1; innerLoop <= (diamondLength / 2) - outerLoop; innerLoop++)
                {
                    Console.Write(" ");
                }
                for (innerLoop = 1; innerLoop <= (2 * outerLoop) - 1; innerLoop++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            for (outerLoop = (diamondLength / 2) - 1; outerLoop >= 1; outerLoop--)
            {
                for (innerLoop = 1; innerLoop <= (diamondLength / 2) - outerLoop; innerLoop++)
                {
                    Console.Write(" ");
                }
                for (innerLoop = 1; innerLoop <= (2 * outerLoop) - 1; innerLoop++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            int diamondLength = 11;
            GenerateDiamondShape(diamondLength);
        }
    }
}