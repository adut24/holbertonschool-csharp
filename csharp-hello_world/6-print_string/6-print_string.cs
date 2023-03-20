using System;

class Program
{
	static void Main(string[] args)
	{
		string str = "Holberton School";
        System.Console.WriteLine("{0}", str+str+str);
        System.Console.WriteLine("{0}", str.Substring(0, 9));
    }
}