using UnityEngine;

public class AddSticker : MonoBehaviour
{
    [SerializeField] private Sticker newSticker;

    public void AddStickerToStickerBook()
    {
        StickerBook.instance.AddSticker(newSticker);
    }
}
