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

        var dialogue = dialogueParts[dialogueIndex];

        // Check if collectable
        var usedItem = false;
        var nextDialogue = 0;
 
        if (dialogue.type == DialogueSettings.nextType.collectable)
        {
            if (dialogue.skipTo > dialogueParts.Count)
            {
                Debug.LogWarning("dialogue SKipTo value on index " + dialogueIndex + " on speaker " + gameObject.name + " is above total amount of dialogue parts.");
                dialogueIndex = 0;
            }

            dialogueIndex = dialogue.skipTo;
        }
            
        for (int i = 0; i < dialogueParts.Count; i++)
        {
            var checkedDialogue = dialogueParts[i];
            if (checkedDialogue.type == DialogueSettings.nextType.collectable)
            {
                if (CollectableHolder.instance.HasId(checkedDialogue.collectable.ID) == false) continue;
                if (checkedDialogue.hasBeenCashedIn) continue;

                checkedDialogue.hasBeenCashedIn = true;
                checkedDialogue.onCashIn.Invoke();

                usedItem = true;
                nextDialogue = checkedDialogue.nextCollectableDialogue;

                dialogueIndex = i;

                break;
            }
        }

        // Play dialogue
        if (interactor)
        {
            interactor.AdjacentController.lockMovement = true;
            DialoguePlayer.instance.onDialogueEnd.AddListener(() => StartCoroutine(DialogueInputTimeout(interactor)));
        }

        var currentDialogue = dialogueParts[dialogueIndex];
        DialoguePlayer.instance.PlayDialogue(currentDialogue.dialogue);

        // Set next dialogue
        switch (currentDialogue.type)
        {
            case DialogueSettings.nextType.none:
                break;
            case DialogueSettings.nextType.reset:
                dialogueIndex = 0;
                break;
            case DialogueSettings.nextType.repeat:
                if (repeatTimes < currentDialogue.repeat - 1)
                {
                    repeatTimes++;
                    return;
                }
                break;
            case DialogueSettings.nextType.collectable:
                if (usedItem && nextDialogue < dialogueParts.Count)
                {
                    dialogueIndex = nextDialogue;
                    return;
                }
                break;
        }

        if (dialogueIndex + 1 < dialogueParts.Count)
        {
            repeatTimes = 0;
            dialogueIndex++;
        }
    }

    private IEnumerator DialogueInputTimeout(InteractorController interactor)
    {
        yield return new WaitForSeconds(TIMEOUT);
        interactor.AdjacentController.lockMovement = false;

        yield break;
    }

    private void OnApplicationQuit() => StopAllCoroutines();

    private void OnDestroy() => StopAllCoroutines();
}
