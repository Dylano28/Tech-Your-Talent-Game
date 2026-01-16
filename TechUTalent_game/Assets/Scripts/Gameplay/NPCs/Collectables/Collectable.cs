using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectable", menuName = "Custom Objects/Data/Collectables/Collectable", order = 1)]
public class Collectable : ScriptableObject
{
    [SerializeField] private string collectableId;
    public string ID => collectableId;
}
