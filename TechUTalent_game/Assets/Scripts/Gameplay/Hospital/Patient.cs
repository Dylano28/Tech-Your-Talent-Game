using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class Patient : MonoBehaviour
{
    [SerializeField] private List<PatientHelpSequence> sequences;

    [SerializeField] public UnityEvent onHelp;
    [SerializeField] public UnityEvent onActive;
    [SerializeField] public UnityEvent onStop;

    private bool _needHelp;
    public bool NeedHelp => _needHelp;

    private Interactable _interactable;


    private void Start()
    {
        _interactable = GetComponent<Interactable>();
        _interactable.onInteract.AddListener(Help);
    }


    public void Activate()
    {
        if (_needHelp == true) return;

        onActive.Invoke();
        _interactable.ResetInteractable();
        _needHelp = true;
    }

    public void Deactivate()
    {
        onStop.Invoke();
        _interactable.DisableInteract();
        _needHelp = false;
    }

    public void Help(InteractorController interactor)
    {
        if (_needHelp == false) return;

        var playerController = interactor.AdjacentController ? interactor.AdjacentController : null;
        var randomSequence = sequences[Random.Range(0, sequences.Count)];
        var sequenceObject = Instantiate(randomSequence.gameObject);
        var sequence = sequenceObject.GetComponent<PatientHelpSequence>();

        if (playerController)
        {
            playerController.lockMovement = true;
            sequence.onComplete.AddListener(() => playerController.lockMovement = false);
        }
        sequence.onComplete.AddListener(() => onHelp.Invoke());
        sequence.onComplete.AddListener(() => _needHelp = false);
    }
}
