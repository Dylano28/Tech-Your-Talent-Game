using UnityEngine;
using UnityEngine.Events;


public class DialoguePlayer : Singleton<DialoguePlayer>
{
    [SerializeField] private float typeCooldown = 0.5f;

    public UnityEvent<string> onNewSegment;
    public UnityEvent<string> onTypeUpdate;
    public UnityEvent onSegmentDone;
    public UnityEvent onDialogueEnd;

    private bool _canSkip = true;
    public bool CanSkip => _canSkip;

    private DialogueContainer _currentContainer;
    private int _currentSegment;

    private bool _isTyping;
    private float _currentSpeedMod;
    private string _currentText;
    private string _remainingText;
    private float _lastUpdate;

    public void PlayDialogue(DialogueContainer container)
    {
        _currentContainer = container;
        _currentSegment = 0;

        NextSegment();
    }

    public void NextSegment()
    {
        if (_canSkip == false) return;
        if (_currentSegment > Mathf.Clamp(_currentContainer.Segments.Count - 1, 0, _currentContainer.Segments.Count))
        {
            StopDialogue();
            return;
        }

        var segmentData = _currentContainer.Segments[_currentSegment];
        var currentName = segmentData.newPerson == string.Empty ? _currentContainer.SpeakerName : segmentData.newPerson;

        _currentText = string.Empty;
        _remainingText = segmentData.text;
        _currentSpeedMod = segmentData.textSpeed;

        // Start typing after setting typing text
        _canSkip = segmentData.isSkipable;
        _isTyping = true;

        // Signal start
        onNewSegment.Invoke(currentName);

        // Set to next segment
        _currentSegment++;
    }

    public void StopDialogue()
    {
        _isTyping = false;
        onDialogueEnd.Invoke();
    }

    private void FixedUpdate()
    {
        if (_isTyping == false) return;

        if (Time.time - _lastUpdate < typeCooldown * _currentSpeedMod) return;
        _lastUpdate = Time.time;

        var length = _remainingText.Length;
        if (length == 0)
        {
            _isTyping = false;
            _canSkip = true;

            onSegmentDone.Invoke();

            return;
        }

        _currentText += _remainingText.ToCharArray()[0];
        _remainingText = _remainingText.Remove(0, 1);

        onTypeUpdate.Invoke(_currentText);
    }
}
