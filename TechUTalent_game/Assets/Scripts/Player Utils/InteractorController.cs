using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractorController : MonoBehaviour
{
    [SerializeField] private UnityEvent onInRange;
    [SerializeField] private UnityEvent leaveRange;

    [SerializeField] private AdvancedPlatformerController adjacentController;
    public AdvancedPlatformerController AdjacentController => adjacentController;

    [SerializeField] private ItemInventory adjacentInventory;
    public ItemInventory AdjacentInventory => adjacentInventory;

    private bool isInRange;
    private Interactable currentInteractable;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>() == false) return;

        isInRange = true;
        currentInteractable = collision.GetComponent<Interactable>();
        if (currentInteractable.AutoInteract) return;

        onInRange.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>() != currentInteractable) return;

        isInRange = false;
        if (currentInteractable.AutoInteract) return;

        leaveRange.Invoke();
    }

    private void Update()
    {
        if (isInRange == false) return;
        if (Input.GetButtonDown("Interact") || currentInteractable.AutoInteract) currentInteractable.Interact(this);
    }
}
