using UnityEngine;

public class ScrollScaling : MonoBehaviour
{
    [SerializeField] private float scrollStep = 0.5f;


    private void Update()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        var scrollValue = new Vector3(scroll, scroll);
        var scrollVector = scrollValue * scrollStep;
        if (transform.localScale.x + scrollVector.x <= scrollStep) return;

        transform.localScale += scrollVector;
    }
}
