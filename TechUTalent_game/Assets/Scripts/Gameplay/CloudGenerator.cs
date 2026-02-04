using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public float speed = 2;
    public float endPosX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartFloating(float speed, float endPosX)
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * speed));

        if(transform.position.x < endPosX)
        {
            Destroy(gameObject);
        }
    }
}
