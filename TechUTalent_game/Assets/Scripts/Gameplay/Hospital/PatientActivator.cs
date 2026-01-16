using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PatientActivator : MonoBehaviour
{
    [SerializeField] private List<Patient> patients = new List<Patient>();
    [SerializeField] private float helpCooldown = 2f;

    private List<Patient> _usedPatients;
    private Patient _lastPatient;
    private Coroutine _currentCoroutine;

    private void Start()
    {
        _usedPatients = patients;
    }


    public void StartLoop() => _currentCoroutine = StartCoroutine(HelpLoop()); 

    public void RestartLoop()
    {
        var thisCoroutine = _currentCoroutine;
        StartLoop();
        StopCoroutine(thisCoroutine);
    }

    private IEnumerator HelpLoop()
    {
        yield return new WaitForSeconds(helpCooldown);
        var nextPatient = _usedPatients[Random.Range(0, _usedPatients.Count)];

        if (_usedPatients.Count != 0)
        {
            _usedPatients = ListUtility.BlackList(patients, nextPatient);
        }
        else _usedPatients = ListUtility.BlackList(patients, _lastPatient);
        _lastPatient = nextPatient;

        nextPatient.Activate();
        RestartLoop();
    }


    public void StopLoop()
    {
        StopAllCoroutines();
        foreach (var patient in patients) patient.Deactivate();
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
