using UnityEngine;

public class StickerData
{
    public bool hasSetSticker = false;
    public Vector2 stickerPosition = Vector2.zero;
    public Sticker sticker;

    public StickerData(bool newSetSticker, Vector2 newPosition, Sticker newSticker)
    {
        hasSetSticker = newSetSticker;
        stickerPosition = newPosition;
        sticker = newSticker;
    }
}
