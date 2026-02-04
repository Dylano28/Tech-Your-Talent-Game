using UnityEngine;

public class Cloudgeneratorscript : MonoBehaviour
{
    [SerializeField]
    GameObject[] clouds;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;


    Vector3 StartPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPos = transform.position;

        Invoke("AttemptSpawn", spawnInterval);
    }


    void Cloudspawn(Vector3 StartPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        GameObject cloud = Instantiate(clouds[randomIndex]);

        float StartY = UnityEngine.Random.Range(StartPos.y - 1f, StartPos.y + 10f);

        cloud.transform.position = new Vector3(StartPos.x, StartY, StartPos.z);
        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        cloud.transform.localScale = new Vector2(scale, scale);
        float speed = UnityEngine.Random.Range(0.5f, 1.5f);
        cloud.GetComponent<CloudGenerator>().StartFloating(speed, endPoint.transform.position.x);


    }

    void AttemptSpawn()
    {
        Cloudspawn(StartPos);
        Invoke("AttemptSpawn", spawnInterval);
    }

    void Prewarm()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 SpawnPos = StartPos + Vector3.left * (i * 2);
            Cloudspawn(StartPos);
        }
    }
}

