using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Patient : MonoBehaviour
{
    [SerializeField] private List<PatientHelpSequence> sequences;
    [SerializeField] private float helpTimeout = 6;

    [SerializeField] public UnityEvent onHelp;
    [SerializeField] public UnityEvent onActive;

    private bool _needHelp;


    private void Start()
    {
        var interactable = GetComponent<Interactable>();
        interactable.onInteract.AddListener(Help);
    }


    public void Activate() => StartCoroutine(HelpLoop());

    private IEnumerator HelpLoop()
    {
        yield return new WaitForSeconds(helpTimeout);
        onActive.Invoke();
        _needHelp = true;

        yield break;
    }

    public void Help(InteractorController interactor)
    {
        if (_needHelp == false) return;

        onHelp.Invoke();
        _needHelp = false;

        var playerController = interactor.AdjacentController ? interactor.AdjacentController : null;
        var randomSequence = sequences[Random.Range(0, sequences.Count)];
        var sequenceObject = Instantiate(randomSequence.gameObject);
        var sequence = sequenceObject.GetComponent<PatientHelpSequence>();

        if (playerController)
        {
            playerController.lockMovement = true;
            sequence.onComplete.AddListener(() => playerController.lockMovement = false);
        }
        sequence.onComplete.AddListener(() => StartCoroutine(HelpLoop()));
    }

    public void StopHelpLoop()
    {
        StopAllCoroutines();
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
