using System;
using UnityEngine;

[Serializable]
public class ComponnentDataHolder
{
    [SerializeField] private string componnentName;
    public string Name => componnentName;

    [SerializeField] private string componnentTargetValue;
    public string TargetValue => componnentTargetValue;

    [SerializeField] private string setValue;
    public string Value => setValue;

    public ComponnentDataHolder(string newName, string newTarget, string newValue)
    {
        componnentName = newName;
        componnentTargetValue = newTarget;
        setValue = newValue;
    }
}
