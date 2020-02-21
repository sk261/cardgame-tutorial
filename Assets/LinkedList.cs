using System.Collections;
using System.Collections.Generic;

public class LinkedList<Type>
{
    public class ListNode<Type>
    {
        public ListNode<Type> next;
        public Type value;
        public ListNode(Type data, ListNode<Type> nextNode = null)
        {
            this.value = data;
            this.next = nextNode;
        }
    }
    // Start is called before the first frame update
    ListNode<Type> front;
    ListNode<Type> back;
    int size;
    LinkedList()
    {
        front = null;
        back = null;
        size = 0;
    }

    public void AddFront(Type data)
    {
        front = new ListNode<Type>(data, front);
        if (size == 0)
            back = front;
        size++;
    }

    public void AddBack(Type data)
    {
        if (size == 0)
            AddFront(data);
        else
        {
            back.next = new ListNode<Type>(data, null);
            back = back.next;
            size++;
        }
    }

    public Type getFront()
    {
        if (size == 0)
            return null;
        return front.value;
    }
}