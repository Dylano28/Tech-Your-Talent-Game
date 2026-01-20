using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    [SerializeField][Tooltip("The initial GameObject where changes will be made from to child componnents and self.")] private Transform rootSearchedGameobject;
    private List<Item> heldItems = new List<Item>();
    public List<Item> Items => heldItems;

    public void AddItem(Item newItem, bool holdItem = true)
    {
        if (heldItems.Contains(newItem)) return;

        Item heldItem = null;
        if (holdItem)
        {
            heldItem = Instantiate(newItem);
            heldItems.Add(heldItem);
        }

        for (int i = 0; i < newItem.changes.Count; i++)
        {
            var change = ApplyChanges(newItem.changes[i], i, newItem.name);

            if (heldItem && newItem.IsPermanent == false)
            {
                heldItem.original.Add(change);
            }
        }
    }

    public ComponnentDataHolder ApplyChanges(ComponnentDataHolder data, int count, string itemName = "")
    {
        var isEmpty = false;
        for (int prop = 0; prop < data.GetType().GetProperties().Length; prop++)
        {
            var property = data.GetType().GetProperties()[prop].GetValue(data);
            if (property is string && string.IsNullOrEmpty((string)property) == true)
            {
                Debug.LogError("Change " + count.ToString() + " on " + itemName + " contains not enough data");
                isEmpty = true;
                break;
            }
        }
        if (isEmpty) return null;

        var type = Type.GetType(data.Name);
        var component = GetComponentFromRoot(type);
        var target = type.GetField(data.TargetValue);
        if (target == null)
        {
            Debug.LogError("Field " + data.TargetValue + " is most likely private! (" + count.ToString() + " on " + itemName + ")");
            return null;
        }

        var originalValue = target.GetValue(component);
        var originalData = new ComponnentDataHolder(data.Name, data.TargetValue, originalValue.ToString());
        var typeOfValue = originalValue.GetType();
        target.SetValue(component, Convert.ChangeType(data.Value, typeOfValue));

        return originalData;
    }

    public void RemoveItem(int itemIndex)
    {
        if (itemIndex > heldItems.Count) return;

        var item = heldItems[itemIndex];
        if (item.original.Count == 0) return;

        for (int i = 0; i < item.original.Count; i++)
        {
            ApplyChanges(item.original[i], i, "(original stats)" + item.name);
        }

        heldItems.Remove(item);
    }

    private Component GetComponentFromRoot(Type usedType)
    {
        var result = rootSearchedGameobject.GetComponent(usedType);
        if (result == null) result = rootSearchedGameobject.GetComponentInChildren(usedType);

        return result;
    }


    public void CreateAndAddItem(List<ComponnentDataHolder> dataSets, bool hold = true)
    {
        var newItem = new Item(dataSets);
        AddItem(newItem, hold);
    }
}
