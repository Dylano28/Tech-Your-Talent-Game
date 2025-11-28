using UnityEngine;
using UnityEngine.Events;

public class PatientHelpSequence : MonoBehaviour
{
    [SerializeField] public UnityEvent onComplete;
    [SerializeField] public UnityEvent onSucces; // When minigame is succeeded
    [SerializeField] public UnityEvent onFail; // When minigame is failed


    private void Awake()
    {
        // Move to camera position to center sequence
        transform.position = (Vector2)Camera.main.transform.position;
    }

    protected void Complete()
    {
        PatientTracker.instance.AddPatient();
        onComplete.Invoke();

        Destroy(gameObject);
    }
}
