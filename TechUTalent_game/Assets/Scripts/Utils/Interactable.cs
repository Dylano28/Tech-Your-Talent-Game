using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private bool interactOnce;
    [SerializeField] private bool autoInteract;
    public bool AutoInteract => autoInteract;

    [SerializeField] private UnityEvent onInteract;

    private bool interacted;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Interact()
    {
        if (interacted) return;
        if (interactOnce == true) interacted = true;

        onInteract.Invoke();
    }
}
