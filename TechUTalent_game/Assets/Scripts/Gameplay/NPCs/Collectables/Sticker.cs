using UnityEngine;

[CreateAssetMenu(fileName = "NewSticker", menuName = "Custom Objects/Data/Collectables/Sticker", order = 1)]
public class Sticker : ScriptableObject
{
    [SerializeField] private string id = "default";
    public string ID => id;

    [SerializeField] private Sprite stickerImage;
    public Sprite StickerImage => stickerImage;
}
