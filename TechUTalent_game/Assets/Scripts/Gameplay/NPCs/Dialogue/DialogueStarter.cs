using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueContainer dialogueContainer;


    public void StartDialogue()
    {
        DialoguePlayer.instance.PlayDialogue(dialogueContainer);
    }
}
