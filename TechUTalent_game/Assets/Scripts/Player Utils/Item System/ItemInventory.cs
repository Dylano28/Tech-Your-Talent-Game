using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ItemInventory : MonoBehaviour
{
    [SerializeField][Tooltip("The initial GameObject where changes will be made from to child componnents and self.")] private Transform rootSearchedGameobject;
    private List<Item> heldItems = new List<Item>();
    public List<Item> Items => heldItems;

    public void AddItem(Item newItem, bool holdItem = true)
    {
        if (heldItems.Contains(newItem)) return;
        if (holdItem) heldItems.Add(newItem);

        for (int i = 0; i < newItem.changes.Count; i++)
        {
            var change = newItem.changes[i];
            var isEmpty = false;
            for (int count = 0; count < change.GetType().GetProperties().Length; count++)
            {
                var property = change.GetType().GetProperties()[count].GetValue(change);
                if (property is string && string.IsNullOrEmpty((string)property) == true)
                {
                    Debug.LogError("Change " + i.ToString() + " on " + newItem.name + " contains not enough data");
                    isEmpty = true;
                    break;
                }
            }
            if (isEmpty) continue;

            var type = Type.GetType(change.Name);
            var component = GetComponentFromRoot(type);
            var target = type.GetField(change.TargetValue);
            if (target == null)
            {
                Debug.LogError("Field " + change.TargetValue + " is most likely private! (" + i.ToString() + " on item " + newItem.name + ")");
                continue;
            }

            var typeOfValue = target.GetValue(component).GetType();
            target.SetValue(component, Convert.ChangeType(change.Value, typeOfValue));
        }
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
