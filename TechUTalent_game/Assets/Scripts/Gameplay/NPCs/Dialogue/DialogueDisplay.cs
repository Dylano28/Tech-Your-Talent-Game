using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private SnapCam snapCam;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI speechLabel;

    [SerializeField] private float panelDownPosition;
    private Vector3 _panelDefaultPosition;
    private RectTransform _panelTransform;


    private void Start()
    {
        var player = DialoguePlayer.instance;
        player.onNewSegment.AddListener(PlaySegment);
        player.onTypeUpdate.AddListener(UpdateText);
        player.onDialogueEnd.AddListener(StopDisplay);

        panel.SetActive(false);

        _panelTransform = GetComponent<RectTransform>();
        _panelDefaultPosition = _panelTransform.localPosition;
    }

    private void PlaySegment(string speakerName)
    {
        if (snapCam)
        {
            if (snapCam.Target.transform.position.y > snapCam.transform.position.y) _panelTransform.localPosition = new Vector3(_panelDefaultPosition.x, panelDownPosition, _panelDefaultPosition.z);
            else _panelTransform.localPosition = _panelDefaultPosition;
        }

        nameLabel.text = speakerName;
        panel.SetActive(true);
    }

    private void UpdateText(string text) => speechLabel.text = text;

    private void StopDisplay() => panel.SetActive(false);
}
