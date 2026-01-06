using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    private Vector3 _origin;
    [SerializeField] private float zPos = -5;


    private void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            return;
        }

        if (Input.GetButton("Fire3") == false) return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var  pos = mousePos - _origin;
        var finalPos = new Vector3(pos.x, pos.y, zPos);
        transform.position = finalPos;
    }
}
