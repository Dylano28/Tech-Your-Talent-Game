using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [Tooltip("For creating repeated interactable speakers")][SerializeField] private Interactable dialogueInteractable;
    [SerializeField] private List<DialogueContainer> dialogueContainer;

    private int dialogueIndex = 0;
    const float TIMEOUT = 0.2f;

    private void Start()
    {
        if (dialogueInteractable) DialoguePlayer.instance.onDialogueEnd.AddListener(() => dialogueInteractable.ResetInteractable());
    }

    public void StartDialogue(InteractorController interactor = null)
    {
        StopAllCoroutines();

        if (interactor)
        {
            interactor.AdjacentController.lockMovement = true;
            DialoguePlayer.instance.onDialogueEnd.AddListener(() => StartCoroutine(DialogueInputTimeout(interactor)));
        }

        DialoguePlayer.instance.PlayDialogue(dialogueContainer[dialogueIndex]);
        if (dialogueIndex + 1 < dialogueContainer.Count) dialogueIndex++;
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
