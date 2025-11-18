using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    [SerializeField] private Transform dragObject;

    private Vector3 _origin;


    private void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragObject.position;
            return;
        }

        if (Input.GetButton("Fire3") == false) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var  pos = mousePos - _origin;
        dragObject.position = (Vector2)pos;
    }
}
