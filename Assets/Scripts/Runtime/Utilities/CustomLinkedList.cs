using System;
using System.Collections.Generic;

public class CustomLinkedList<T> : LinkedList<T>
{
    public T Find(Predicate<T> predicate)
    {
        foreach (T item in this)
        {
            if (predicate(item) == true)
            {
                return item;
            }
        }
        return default;
    }

    public T Remove(Predicate<T> predicate)
    {
        foreach (T item in this)
        {
            if (predicate(item) == true)
            {
                this.Remove(item);
                return item;
            }
        }
        return default;
    }

    public void FirstToLast()
    {
        LinkedListNode<T> first = this.First;
        if (first == null)
            return;

        RemoveFirst();
        this.AddLast(first);
    }
}