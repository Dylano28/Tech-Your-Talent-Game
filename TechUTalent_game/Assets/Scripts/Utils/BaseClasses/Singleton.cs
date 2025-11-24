using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool keepInstance;
    [HideInInspector] public static T instance;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this as T;

        if (keepInstance) DontDestroyOnLoad(gameObject);
    }
}
