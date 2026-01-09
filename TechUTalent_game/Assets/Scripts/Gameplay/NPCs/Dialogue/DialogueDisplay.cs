using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI speechLabel;


    private void Start()
    {
        var player = DialoguePlayer.instance;
        player.onNewSegment.AddListener(PlaySegment);
        player.onTypeUpdate.AddListener(UpdateText);
        player.onDialogueEnd.AddListener(StopDisplay);

        panel.SetActive(false);
    }

    private void PlaySegment(string speakerName)
    {
        nameLabel.text = speakerName;
        panel.SetActive(true);
    }

    private void UpdateText(string text) => speechLabel.text = text;

    private void StopDisplay() => panel.SetActive(false);
}
