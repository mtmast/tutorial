using System;

namespace MyApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            int firstNum, secondNum, result;
            char opr;
            Console.WriteLine("Type a number, and then press Enter");
            firstNum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Type a number, and then press Enter");
            secondNum = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Choose an option from an following list :" +
                " \n a - Add " +
                " \n s - Substract" +
                " \n m - Multiply" +
                " \n d - Divide");
            Console.Write("Your option? ");
            opr = Convert.ToChar(Console.ReadLine()[0]); 

            switch (opr)
            {
                case 'a': result = firstNum + secondNum;
                    opr = '+';
                    break;
                case 's': result = firstNum - secondNum;
                    opr = '-';
                    break;
                case 'm': result = firstNum * secondNum;
                    opr = '*';
                    break;
                case 'd': result = firstNum / secondNum;
                    opr = '/';
                    if (secondNum != 0)
                    {
                        result = firstNum / secondNum;
                        opr = '/';
                    }
                    else
                    {
                        Console.WriteLine("Sorry, :( Cannot divide by zero");
                        return;
                    }
                    break;
                default : Console.WriteLine("Sorry, Your Option is not correct");
                    result = 0;
                    break;
            }

            Console.WriteLine("Your result :" + firstNum + " " + opr + " " + secondNum + " = " + result +""); 

        }
    }
}