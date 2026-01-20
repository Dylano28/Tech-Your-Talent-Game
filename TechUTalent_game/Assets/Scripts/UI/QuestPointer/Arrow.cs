using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Transform target;

    private void Start()
    {
        ObjectiveManager.Instance.OnAllCollected += ActivateArrow;
        gameObject.SetActive(false);
    }

    private void ActivateArrow()
    {
        target = ObjectiveManager.Instance.GetFinalTarget();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (target != null) return;

        Vector3 direction = target.position - player.position;
        direction.y = 0; 

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
