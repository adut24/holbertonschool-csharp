using System;
using System.Reflection;

static class Obj
{
	public static void Print(object myObj)
	{
		Type objectType = myObj.GetType();
		Console.WriteLine($"{objectType.Name} Properties:");
        foreach (PropertyInfo item in objectType.GetTypeInfo().DeclaredProperties)
        {
			Console.WriteLine(item.Name);
        }
		Console.WriteLine($"{objectType.Name} Methods:");
		foreach (MethodInfo item in objectType.GetTypeInfo().DeclaredMethods)
		{
			Console.WriteLine(item.Name);
		}
	}
}
