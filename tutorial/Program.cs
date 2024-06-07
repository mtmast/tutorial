namespace MyApp
{
    internal class Program
    {
        public static void GetCenturyAndLeapYear(int year)
        {
            int leap = ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0) && (year >= 1000) ? 1 : -1;
            int century = (year >= 1000 ) ?  (year / 100) + 1 : -1;
            Console.WriteLine($"Output : {century}, {leap} ");
        }
        static void Main(string[] args)
        {
            int year;
            Console.Write("Input Year : ");
            year = Convert.ToInt32(Console.ReadLine());
            GetCenturyAndLeapYear(year);
        }
    }
}