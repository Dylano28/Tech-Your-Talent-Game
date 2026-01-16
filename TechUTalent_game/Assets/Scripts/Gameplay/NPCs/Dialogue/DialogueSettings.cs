using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueSettings
{
    public enum nextType
    {
        none,
        skip,
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
    [Range(0, MAX_NUMBER)] public int skipTo;

    public UnityEvent onStart;

    [Header("Collectable Settings")]
    public DialogueContainer notCollectedDialogue;
    [Tooltip("Make sure to always put next message AFTER this segment")] public Collectable collectable;

    public UnityEvent onCashIn;

    [HideInInspector] public bool hasBeenCashedIn;
}
