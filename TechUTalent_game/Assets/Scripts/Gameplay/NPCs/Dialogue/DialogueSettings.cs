using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueSettings
{
    public enum nextType
    {
        none,
        reset,
        repeat,
        collectable
    }
    private const int MAX_NUMBER = 1000;

    // Settings
    public DialogueContainer dialogue;

    [Header("General")]
    public nextType type = nextType.none;
    [Range(2, MAX_NUMBER)] public int repeat;

    [Header("Collectable Settings")]
    public Collectable collectable;
    [Range(0, MAX_NUMBER)] public int skipTo;
    [Range(0, MAX_NUMBER)] public int nextCollectableDialogue;
    public UnityEvent onCashIn;

    [HideInInspector] public bool hasBeenCashedIn;
}
