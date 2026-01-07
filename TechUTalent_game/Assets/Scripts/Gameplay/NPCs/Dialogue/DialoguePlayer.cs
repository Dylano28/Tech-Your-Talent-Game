using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialoguePlayer : Singleton<DialoguePlayer>
{
    private DialogueContainer _currentContainer;
    private int _currentSegment;

    public bool canSkip;

    [HideInInspector] public UnityEvent<string, string, float> onNewDialogue;
    [HideInInspector] public UnityEvent onDialogueEnd;


    public void PlayDialogue(DialogueContainer container)
    {
        _currentContainer = container;
        _currentSegment = 0;
    }

    public void NextSegment()
    {
        if (_currentSegment > _currentContainer.Segments.Count)
        {
            StopDialogue();
            return;
        }

        _currentSegment++;

        var segmentData = _currentContainer.Segments[_currentSegment];
        var currentName = segmentData.newPerson == string.Empty ? _currentContainer.SpeakerName : segmentData.newPerson;
        onNewDialogue.Invoke(currentName, segmentData.text, segmentData.textSpeed);

        canSkip = segmentData.isSkipable;
    }

    public void StopDialogue() => onDialogueEnd.Invoke();


    private void Update()
    {
        if (canSkip == false) return;

        if (Input.GetButton("Jump"))
        {
            NextSegment();
        }
    }
}
