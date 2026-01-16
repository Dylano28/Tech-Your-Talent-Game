using UnityEngine;
using UnityEngine.Events;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private bool countDown = true;
    [SerializeField] private int time = 60;

    [SerializeField] public UnityEvent OnStartTimer;
    [SerializeField] public UnityEvent OnStopTimer;

    private float _currentTime = 0;
    private bool _isActive = false;
    public bool IsActive => _isActive;


    public void StartCountdown()
    {
        _isActive = true;
        _currentTime = countDown ? time : 0;

        OnStartTimer.Invoke();
    }

    private void Update()
    {
        if (_isActive == false) return;

        if ((_currentTime <= 0 && countDown) || (_currentTime >= time && countDown == false)) StopCountdown();
        _currentTime = countDown ? _currentTime - Time.deltaTime : _currentTime + Time.deltaTime;
    }

    public int GetTime()
    {
        return Mathf.RoundToInt(_currentTime);
    }

    public void StopCountdown(bool timeout = true)
    {
        _isActive = false;
        if (timeout) OnStopTimer.Invoke();
    }
}
