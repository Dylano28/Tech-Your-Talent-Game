using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMesh))]
public class PatientDisplay : MonoBehaviour
{
    private PatientTracker _patientTracker;
    private TextMesh _display;

    private void Start()
    {
        _display = GetComponent<TextMesh>();
        _display.text = string.Empty;

        _patientTracker = PatientTracker.instance;
        _patientTracker.onAdd.AddListener(DisplayNewPatients);
        _patientTracker.onReset.AddListener(() => _display.text = string.Empty);
    }

    public void DisplayNewPatients(int patientAmount)
    {
        if (_patientTracker.HasLimit())
        {
            var patientString = patientAmount.ToString() + " / " + _patientTracker.PatientLimit.ToString();
            _display.text = patientString;
            return;
        }
        _display.text = patientAmount.ToString();
    }
}
