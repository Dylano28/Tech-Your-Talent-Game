using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Custom Objects/Data/New Player Item", order = 1)]
public class Item : ScriptableObject
{
    [SerializeField] private List<ComponnentDataHolder> changedComponnents;
    public List<ComponnentDataHolder> changes => changedComponnents;

    [SerializeField] private bool isPermanent;
    public bool IsPermanent => isPermanent;

    [HideInInspector] public List<ComponnentDataHolder> original = new List<ComponnentDataHolder>();

    public Item(List<ComponnentDataHolder> newDataSet)
    {
        changedComponnents = newDataSet;
    }

    public void AddData(ComponnentDataHolder data)
    {
        changedComponnents.Add(data);
    }
}
