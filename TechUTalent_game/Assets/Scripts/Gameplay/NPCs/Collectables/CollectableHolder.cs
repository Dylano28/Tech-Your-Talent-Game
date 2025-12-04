using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHolder : Singleton<CollectableHolder>
{
    public List<string> collectableIds {  get; private set; }

    private void Start()
    {
        if (collectableIds == null) collectableIds = new List<string>();
    }

    public void AddCollectable(Collectable collectable)
    {
        if (collectableIds.Contains(collectable.ID)) return;
        collectableIds.Add(collectable.ID);
    }

    public void RemoveCollectable(Collectable collectable)
    {
        if (collectableIds.Contains(collectable.ID) == false) return;
        collectableIds.Remove(collectable.ID);
    }

    public bool HasId(Collectable collectable)
    {
        return collectableIds.Contains(collectable.ID);
    }

    public bool HasId(string id)
    {
        return collectableIds.Contains(id);
    }
}
