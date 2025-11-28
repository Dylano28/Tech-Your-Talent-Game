using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class StopwatchDisplay : MonoBehaviour
{
    [SerializeField] private Stopwatch stopwatch;

    private TextMesh _text;
    private bool _updateTimer = false;

    private void Start()
    {
        _text = GetComponent<TextMesh>();
        _text.text = stopwatch.GetTime().ToString();

        stopwatch.OnStartTimer.AddListener(() => _updateTimer = true);
        stopwatch.OnStopTimer.AddListener(() => _updateTimer = false);
    }

    private void Update()
    {
        if (_updateTimer == false) return;
        _text.text = stopwatch.GetTime().ToString();
    }
}
