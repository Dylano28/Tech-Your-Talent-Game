using System;
using UnityEngine;


[Serializable]
public class DialogueSegment
{
    [Tooltip("Leave epmty when speaker doesn't change")] public string newPerson;
    [TextArea] public string text;
    [Range(0.1f, 1)] public float textSpeed = 1f;
    public bool isSkipable = true;
}
