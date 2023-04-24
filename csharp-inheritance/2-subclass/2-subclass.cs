using System;

static class Obj
{
	public static bool IsOnlyASubclass(Type derivedType, Type baseType)
	{
		return derivedType.BaseType == baseType;
	}
}
