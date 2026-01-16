using System.Collections;
using UnityEngine;

public class TestSequence : PatientHelpSequence
{
    [SerializeField] private float waitTime = 4.0f;

    void Start()
    {
        StartCoroutine(TestLoop());
    }

    private IEnumerator TestLoop()
    {
        yield return new WaitForSeconds(waitTime);

        onSucces.Invoke();
        Complete();

        yield break;
    }
}
