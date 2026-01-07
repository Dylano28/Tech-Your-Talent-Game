using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDialogue", menuName = "Custom Objects/Data/New Dialogue", order = 1)]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] private string speakerName = "Persoon";
    public string SpeakerName => speakerName;

    [SerializeField] private List<DialogueSegment> dialogueSegments;
    public List<DialogueSegment> Segments => dialogueSegments;
}
