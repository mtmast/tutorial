namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool correctInput;
            decimal  result;
            Console.WriteLine("Type a number, and then press Enter");
            correctInput = decimal.TryParse(Console.ReadLine(), out decimal firstNum);
            Console.WriteLine( !correctInput ? "You input data is incorrect, But i insert 0 for you" : string.Empty);
            Console.WriteLine("Type a number, and then press Enter");
            correctInput = decimal.TryParse(Console.ReadLine(), out decimal secondNum);
            Console.WriteLine(!correctInput ? "You input data is incorrect, But i insert 0 for you" : string.Empty);
            Console.WriteLine("Choose an option from an following list :" +
                " \n a - Add " +
                " \n s - Substract" +
                " \n m - Multiply" +
                " \n d - Divide");
            Console.Write("Your option? ");
            correctInput = char.TryParse(Console.ReadLine(), out char opr);
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
                case 'd': 
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
            Console.WriteLine(correctInput ?"Your result :" + firstNum + " " + opr + " " + secondNum + " = " + Math.Round(result, 2) + "" : string.Empty);
        }
    }
}