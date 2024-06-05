using System;

namespace MyApp
{
    internal class Program
    {
        public static void getCenturyAndLeapYear(int year)
        {
            int leap, century;
            if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
            {
                leap = 1;
            }
            else
            {
                leap = -1;
            }
            century = (year / 100) + 1;
            Console.WriteLine("Output : "+ century +", "+ leap );
        }
        static void Main(string[] args)
        {
            int year;
            Console.Write("Input Year : ");
            year = Convert.ToInt32(Console.ReadLine());
            getCenturyAndLeapYear(year);
        }
    }
}