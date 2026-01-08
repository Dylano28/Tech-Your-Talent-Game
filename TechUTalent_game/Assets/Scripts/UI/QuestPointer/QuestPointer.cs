using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    private Vector3 TargetPosition;
    private RectTransform pointRectTransform;
    private void Awake()
    {
        TargetPosition = new Vector3(200, 45);
        pointRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();

    }

    private void Update()
    {
        Vector3 toPosition = TargetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
    }
}
