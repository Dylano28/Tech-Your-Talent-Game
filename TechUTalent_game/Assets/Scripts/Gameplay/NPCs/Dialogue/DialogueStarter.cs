using NUnit.Framework.Internal.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [Tooltip("For creating repeated interactable speakers")][SerializeField] private Interactable dialogueInteractable;
    [SerializeField] private List<DialogueSettings> dialogueParts;

    private int dialogueIndex = 0;
    private int repeatTimes = 0;
    const float TIMEOUT = 0.2f;

    private void Start()
    {
        if (dialogueInteractable) DialoguePlayer.instance.onDialogueEnd.AddListener(() => dialogueInteractable.ResetInteractable());
    }



    public void StartDialogue(InteractorController interactor = null)
    {
        StopAllCoroutines();

        var currentDialogue = dialogueParts[dialogueIndex];
        var segment = currentDialogue.dialogue;

        // Check collected item
        if (currentDialogue.type == DialogueSettings.nextType.collectable)
        {
            if (CollectableHolder.instance.HasId(currentDialogue.collectable.ID) == false)
            {
                segment = currentDialogue.notCollectedDialogue;
            }
            else if (currentDialogue.hasBeenCashedIn == false)
            {
                currentDialogue.hasBeenCashedIn = true;
                currentDialogue.onCashIn.Invoke();
            }
        }

        // Play dialogue
        if (interactor)
        {
            interactor.AdjacentController.lockMovement = true;
            DialoguePlayer.instance.onDialogueEnd.AddListener(() => StartCoroutine(DialogueInputTimeout(interactor)));
        }

        DialoguePlayer.instance.PlayDialogue(segment);
        currentDialogue.onStart.Invoke();

        // Set next dialogue
        switch (currentDialogue.type)
        {
            case DialogueSettings.nextType.none:
                break;
            case DialogueSettings.nextType.skip:
                dialogueIndex = currentDialogue.skipTo - 1;
                return;
            case DialogueSettings.nextType.reset:
                dialogueIndex = 0;
                return;
            case DialogueSettings.nextType.repeat:
                if (repeatTimes < currentDialogue.repeat - 1)
                {
                    repeatTimes++;
                    return;
                }
                break;
            case DialogueSettings.nextType.collectable:
                if (currentDialogue.hasBeenCashedIn) break;
                return;
        }

        repeatTimes = 0;
        if (dialogueIndex + 1 < dialogueParts.Count) dialogueIndex++;
    }

    private IEnumerator DialogueInputTimeout(InteractorController interactor)
    {
        yield return new WaitForSeconds(TIMEOUT);
        interactor.AdjacentController.lockMovement = false;

        yield break;
    }


    public void SetToSegmnent(int newIndex)
    {
        if (newIndex < 0 || newIndex > dialogueParts.Count - 1)
        {
            Debug.LogWarning("Given index " + newIndex.ToString() + " inavlid! Please check " + gameObject.name + "'s list of dialogue.");
            return;
        }
        dialogueIndex = newIndex;
    }



    private void OnApplicationQuit() => StopAllCoroutines();

    private void OnDestroy() => StopAllCoroutines();
}
