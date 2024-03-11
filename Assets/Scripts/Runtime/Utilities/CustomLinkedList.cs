using System;
using System.Collections.Generic;

public class CustomLinkedList<T> : List<T>
{
    public T First
    {
        get
        {
            return Count == 0 ? default : this[0];
        }
    }

    public T Last
    {
        get
        {
            return Count == 0 ? default : this[Count - 1];
        }
    }

    public void RemoveFirst()
    {
        if (Count > 0)
        {
            RemoveAt(0);
        }
    }

    public void RemoveLast()
    {
        if (Count > 0)
        {
            RemoveAt(Count - 1);
        }
    }

    public void AddLast(T item)
    {
        this.Add(item);
    }

    public void AddFirst(T item)
    {
        this.Insert(0, item);
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
        T first = this.First;
        if (first == null)
            return;

        RemoveFirst();
        this.AddLast(first);
    }

    public void LastToFirst()
    {
        T last = this.Last;
        if (last == null)
            return;

        RemoveLast();
        this.AddFirst(last);
    }
}