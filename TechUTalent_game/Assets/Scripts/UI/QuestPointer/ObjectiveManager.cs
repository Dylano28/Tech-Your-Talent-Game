using System;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [SerializeField] private int totalCollectable;
    private int collected;

    [SerializeField] private Transform finalTarget;

    public event Action OnAllCollected;

    private void Awake()
    {
        Instance = this;
    }

    public void Collect()
    {
        collected++;

        if (collected >= totalCollectable)
        {
            OnAllCollected?.Invoke();
        }
    }

    public Transform GetFinalTarget()
    {
        return finalTarget; 
    }
}
