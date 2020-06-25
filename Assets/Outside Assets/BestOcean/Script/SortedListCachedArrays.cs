using System.Collections.Generic;


public class SortedListCachedArrays<TKey, TValue> : SortedList<TKey, TValue>
{
    public TKey[] KeyArray = new TKey[0];
    public TValue[] ValueArray = new TValue[0];

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);

        RefreshArrays();
    }

    public new void Remove(TKey key)
    {
        base.Remove(key);

        RefreshArrays();
    }

    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);

        RefreshArrays();
    }

    void RefreshArrays()
    {
        if (Count != KeyArray.Length) KeyArray = new TKey[Count];
        if (Count != ValueArray.Length) ValueArray = new TValue[Count];

        Keys.CopyTo(KeyArray, 0);
        Values.CopyTo(ValueArray, 0);
    }
}

