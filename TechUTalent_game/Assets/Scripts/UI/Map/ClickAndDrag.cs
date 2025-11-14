using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    [SerializeField] private Transform dragObject;

    private Vector3 _origin;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            _origin = dragObject.position;
            return;
        }

        if (Input.GetKey(KeyCode.Mouse2) == false) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var  pos = mousePos - _origin;
        dragObject.position = (Vector2)pos;
    }
}
