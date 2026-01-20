using UnityEngine;
using UnityEngine.Events;

public class PatientTracker : Singleton<PatientTracker>
{
    [SerializeField][Tooltip("Leave at 0 to disable limit")] private int patientLimit = 12;
    public int PatientLimit => patientLimit;

    [SerializeField] public UnityEvent<int> onClear;
    [SerializeField] public UnityEvent<int> onAdd;
    [SerializeField] public UnityEvent onReset;

    private int _helpedPatients;

    public void AddPatient(int patientAmount = 1)
    {
        _helpedPatients += patientAmount;

        onAdd.Invoke(_helpedPatients);
        if (HasLimit() && _helpedPatients > patientLimit) onClear.Invoke(_helpedPatients);
    }

    public void ResetPatients()
    {
        _helpedPatients = 0;
        onReset.Invoke();
    }

    public bool HasLimit() => patientLimit > 0;
}
