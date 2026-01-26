using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StickerBook : Singleton<StickerBook>
{
    private List<StickerData> dataSet = new List<StickerData>();
    public List<StickerData> DataSet => dataSet;

    [SerializeField][Tooltip("Debug to add stickers for testing")] private List<Sticker> starterStickers;


    private void Start()
    {
        CheckDebug();
    }

    private void CheckDebug()
    {
        if (starterStickers.Count == 0) return;
        foreach (var sticker in starterStickers) AddSticker(sticker);
    }


    public void AddSticker(Sticker newSticker)
    {
        var newData = new StickerData(false, Vector2.zero, newSticker);
        dataSet.Add(newData);
    }

    public bool HasSticker(string checkId)
    {
        foreach (var data in dataSet) if (data.sticker.ID == checkId) return true;
        return false;
    }

    public void RemoveSticker(string checkId)
    {
        foreach (var data in dataSet)
        {
            if (data.sticker.ID == checkId)
            {
                dataSet.Remove(data);
                return;
            }
        }
    }

    public void RemoveSticker(Sticker checkSticker)
    {
        foreach (var data in dataSet)
        {
            if (data.sticker == checkSticker)
            {
                dataSet.Remove(data);
                return;
            }
        }
    }

    public void changeStickerPosition(int stickerIndex, Vector2 newPosition)
    {
        if (stickerIndex > dataSet.Count)
        {
            var listSize = dataSet.Count;
            stickerIndex = listSize;
            Debug.LogWarning("Given sticker index of " + stickerIndex.ToString() + " is above sticker list size of " + listSize.ToString() + ". Setting stickerposition of last sticker in list");
        }

        var stickerData = dataSet[stickerIndex];
        stickerData.stickerPosition = newPosition;
        stickerData.hasSetSticker = true;
    }
}
