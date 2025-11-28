using UnityEngine;
using UnityEngine.Events;

public class PatientHelpSequence : MonoBehaviour
{
    [SerializeField] public UnityEvent onComplete;
    [SerializeField] public UnityEvent onSucces;
    [SerializeField] public UnityEvent onFail;


    private void Awake()
    {
        // Move to camera position to center sequence
        transform.position = (Vector2)Camera.main.transform.position;
    }

    protected void Complete()
    {
        PatientTracker.instance.AddPatient();
        Destroy(gameObject);
    }
}
