using UnityEngine;

public class DialogueInput : MonoBehaviour
{
    [SerializeField] private string skipAction = "Jump";

    private bool _recievingInput;


    private void Start()
    {
        var player = DialoguePlayer.instance;
        player.onNewSegment.AddListener((arg) => _recievingInput = true);
        player.onDialogueEnd.AddListener(() => _recievingInput = false);
    }

    private void Update()
    {
        if (_recievingInput && Input.GetButtonDown(skipAction)) DialoguePlayer.instance.NextSegment();
    }
}
