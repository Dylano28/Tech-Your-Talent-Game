using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private float typeCooldown = 1f;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI speechLabel;

    private bool _isPlaying;
    private float _currentSpeedMod;
    private string _remainingText;
    private string _currentText;
    private float _lastUpdate;
    private DialoguePlayer _player;


    private void Start()
    {
        _player = DialoguePlayer.instance;
        _player.onNewDialogue.AddListener(PlaySegment);
        _player.onDialogueEnd.AddListener(StopDisplay);

        panel.SetActive(false);
    }

    private void PlaySegment(string speakerName, string speech, float speed)
    {
        _isPlaying = true;
        _currentSpeedMod = speed;
        _remainingText = speech;
        _currentText = string.Empty;

        nameLabel.text = speakerName;

        panel.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (_isPlaying == false) return;

        if (Time.time - _lastUpdate < typeCooldown * _currentSpeedMod) return;
        _lastUpdate = Time.time;

        var length = _remainingText.Length;
        if (length == 0)
        {
            _isPlaying = false;
            _player.canSkip = true;
            return;
        }

        _currentText += _remainingText.ToCharArray()[0];
        _remainingText = _remainingText.Remove(0, 1);

        speechLabel.text = _currentText;
    }

    private void StopDisplay()
    {
        panel.SetActive(false);
    }
}
