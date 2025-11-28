using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private bool interactOnce;
    [SerializeField] private bool destroyOnInteract;
    [SerializeField] private bool autoInteract;
    public bool AutoInteract => autoInteract;

    [SerializeField] public UnityEvent<InteractorController> onInteract;

    private bool interacted;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Interact(InteractorController interactor)
    {
        if (interacted) return;
        if (interactOnce == true) interacted = true;

        onInteract.Invoke(interactor);

        if (destroyOnInteract) Destroy(gameObject);
    }

    public void ResetInteractable() => interacted = false; // Used to reset if set to InteractOnce

    public void DisableInteract() => interacted = true; // Disable interaction
}
