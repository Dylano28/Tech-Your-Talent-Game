using System.Collections.Generic;
using UnityEngine;

public class ListUtility
{
    // Get new list witout certain item
    public static List<T> BlackList<T>(List<T> usedList, T blackListedItem)
    {
        var list = new List<T>(usedList);
        list.Remove(blackListedItem);

        return list;
    }
}
