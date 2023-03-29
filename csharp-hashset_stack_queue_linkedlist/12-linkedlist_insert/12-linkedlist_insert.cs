using System.Collections.Generic;

class LList
{
	public static LinkedListNode<int> Insert(LinkedList<int> myLList, int n)
	{
		LinkedListNode<int> newNode = new LinkedListNode<int>(n);
		foreach (int value in myLList)
		{
			if (value > newNode.Value)
			{
				myLList.AddBefore(myLList.Find(value), newNode);
				break;
			}
		}
		return newNode;
	}
}
