using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that repressents a list sorted by a key.
/// </summary>
/// <typeparam name="T">Type of the elements of the list.</typeparam>
/// <typeparam name="D">Type of the keys used to sort. It must be a comparable type.</typeparam>
public class KeySortedList<T, D> where D : IComparable<D>
{
    /// <summary>
    /// List of values.
    /// </summary>
    private List<T> values;

    /// <summary>
    /// List of keys.
    /// </summary>
    private List<D> keys;

    /// <summary>
    /// Accessor to the number of element in the list.
    /// </summary>
    public int Count
    {
        get
        {
            return values.Count;
        }
    }

    /// <summary>
    /// Indexor to the element in the position index.
    /// </summary>
    /// <param name="index">Position of the element we want to access. Its value
    /// must be in the range [0, this.Count[.</param>
    /// <returns>Element at position index.</returns>
    public T this[int index]
    {
        get
        {
            return values[index];
        }
    }

    /// <summary>
    /// Base constructor that creates an empty list.
    /// </summary>
    public KeySortedList()
    {
        values = new List<T>();
        keys = new List<D>();
    }

    /// <summary>
    /// Value of the key of the element at position index.
    /// </summary>
    /// <param name="index">Position of the element we want to access. Its value
    /// must be in the range [0, this.Count[.</param>
    /// <returns>Key of the elemet at position index.</returns>
    public D KeyAt(int index)
    {
        return keys[index];
    }

    /// <summary>
    /// Key of the element in the list value.
    /// </summary>
    /// <param name="value">Element on the list whose index we want to obtain.</param>
    /// <returns>Key of the element value.</returns>
    public D KeyOf(T value)
    {
        return keys[values.IndexOf(value)];
    }

    /// <summary>
    /// Adds a new element to the list in sorted order based on the key.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    public void Add(T value, D key)
    {
        if(keys.Contains(key))
        {
            int i = keys.IndexOf(key);
            keys.RemoveAt(i);
            values.RemoveAt(i);
        }
        keys.InsertIntoSortedList(key);
        values.Insert(keys.IndexOf(key), value);
    }

    /// <summary>
    /// Removes the element at position i in the list.
    /// </summary>
    /// <param name="i">Position of the element we want to access. Its value
    /// must be in the range [0, this.Count[.</param>
    public void RemoveAt(int i)
    {
        if(i >= 0 && i < Count)
        {
            values.RemoveAt(i);
            keys.RemoveAt(i);
        }
    }

    /// <summary>
    /// Removes the element value of the list.
    /// </summary>
    /// <param name="value">Element of the list that we want to destroy.</param>
    public void Remove(T value)
    {
        while(values.Contains(value))
        {
            int i = values.IndexOf(value);
            keys.RemoveAt(i);
            values.RemoveAt(i);
        }
    }

    /// <summary>
    /// Whenever the list contains a value.
    /// </summary>
    /// <param name="value">Element that we want to know if it's in the list.</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        return values.Contains(value);
    }
}
