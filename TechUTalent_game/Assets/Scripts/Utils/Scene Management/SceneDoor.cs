using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneDoor : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private int sceneId = 0;
    [SerializeField] private string doorId = "door";
    [SerializeField] private string nextDoorId = "door";
    public string DoorId => doorId;

    [SerializeField] private Vector2 enterPosition; 
    public Vector2 EnterPosition => (Vector3)enterPosition + transform.position;

    private Color GIZMO_COLOR = Color.green;
    private const float GIZMO_RADIUS = 0.25f;


    private void Start()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            SceneSetter.instance.NextDoor = nextDoorId;
            SceneManager.LoadScene(sceneId);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = GIZMO_COLOR;
        Gizmos.DrawWireSphere(EnterPosition, GIZMO_RADIUS);
    }
}
