using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int diamondLenght = 11;
            int i, y;
             for( i = 0; i <= diamondLenght; i++)
             {
                if (i <= (diamondLenght / 2))
                {
                    for ( y = 0; y <= diamondLenght; y++)
                    { 
                        
                        Console.Write("*");
                    }    
                    Console.WriteLine();
                }
                else
                {
                   
                }  

             } 
        }
    }
}