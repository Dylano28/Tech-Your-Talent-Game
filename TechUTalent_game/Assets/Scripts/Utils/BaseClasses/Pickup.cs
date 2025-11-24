using UnityEngine;

[RequireComponent(typeof(Interactable))]
public abstract class Pickup : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Interactable>().onInteract.AddListener(PickUp);
    }

    protected abstract void PickUp(InteractorController interactor);
}
