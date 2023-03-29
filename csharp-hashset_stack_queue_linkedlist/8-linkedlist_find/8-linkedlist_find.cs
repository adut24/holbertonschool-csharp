using System.Collections.Generic;
using System.Linq;

class LList
{
	public static int FindNode(LinkedList<int> myLList, int value)
	{
		int index = myLList.TakeWhile(node => value != node).Count();

		if (index == myLList.Count)
			return -1;
		return index;
	}
}
