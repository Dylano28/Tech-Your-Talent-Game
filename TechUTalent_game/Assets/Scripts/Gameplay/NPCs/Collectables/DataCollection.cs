using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDataCollection", menuName = "Custom Objects/Data/Collectables/Data Collection", order = 1)]
public class DataCollection : ScriptableObject
{
    [SerializeField] private List<Collectable> collectableCollection;
    public List<Collectable> Collectables => collectableCollection;

    [SerializeField] private List<Collectable> stickerCollection;
    public List<Collectable> Stickers => stickerCollection;

    public static DataCollection Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Another data collection was detected with the name of: " +  Instance.name);
            return;
        }
        Instance = this;
    }
}
