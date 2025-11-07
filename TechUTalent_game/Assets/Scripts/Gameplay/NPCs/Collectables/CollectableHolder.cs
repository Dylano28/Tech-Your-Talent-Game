using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHolder : MonoBehaviour
{
    public static CollectableHolder Instance;
    public List<string> collectableIds {  get; private set; }

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
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
