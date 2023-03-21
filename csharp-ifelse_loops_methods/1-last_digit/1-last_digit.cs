using System;

class Program
{
    static void Main(string[] args)
    {
        Random rndm = new Random();
        int number = rndm.Next(-10000, 10000);
        int lastDigit = number % 10;
        System.Console.Write($"The last digit of {number} is {lastDigit}");
        if (lastDigit > 5)
            System.Console.WriteLine(" and is greater than 5");
        else if (lastDigit == 0)
            System.Console.WriteLine(" and is 0");
        else
            System.Console.WriteLine(" and is less than 6 and not 0");
    }
}