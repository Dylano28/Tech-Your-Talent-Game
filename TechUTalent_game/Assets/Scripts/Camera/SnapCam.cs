using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SnapCam : MonoBehaviour
{
    private Vector2 snapSize;
    private const string SNAP_INFO = "Adds a margin on the size of the collider. Make sure to change the collider size to your resolution for snapping!";
    [SerializeField][Tooltip(SNAP_INFO)] private Vector2 margin = new Vector2(0.2f, 0.2f);
    [SerializeField] private int snapMod = 2;
    [SerializeField] private string callTag = "Player";

    private void Start()
    {
        var camCollider = GetComponent<BoxCollider2D>();
        snapSize = camCollider.size / 2;
        camCollider.size = camCollider.size - margin;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != callTag) return;

        var collPos = collision.transform.position;
        var snap = snapSize * snapMod;
        var x = Mathf.Round(collPos.x / snap.x);
        var y = Mathf.Round(collPos.y / snap.y);
        var gridPos = new Vector2(x, y) * snap;

        transform.position = new Vector3(gridPos.x, gridPos.y, transform.position.z);
    }
}
