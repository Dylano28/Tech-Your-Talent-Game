using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class StickerBookDisplay : MonoBehaviour
{
    [SerializeField] private Vector3 defaultStickerPosition;
    public Vector3 DefaultStickerPosition => defaultStickerPosition;

    [SerializeField] private Vector2 stickerMin;
    [SerializeField] private Vector2 stickerMax;

    [SerializeField] private RectTransform rootStickerParent;
    [SerializeField] private GameObject rootStickerBook;

    private StickerBook _stickerBook;
    private const float STICKERSCALE = 0.026f;


    private void Start()
    {
        _stickerBook = StickerBook.instance;
        rootStickerBook.SetActive(false);
    }



    public void SetStickerField()
    {
        rootStickerBook.SetActive(true);

        var dataSet = _stickerBook.DataSet;
        for (int dataIndex = 0; dataIndex < dataSet.Count; dataIndex++)
        {
            var data = dataSet[dataIndex];
            var sticker = data.sticker;
            RectTransform stickerTransform = null;

            if (dataIndex + 1 > rootStickerParent.transform.childCount)
            {
                var newGameObject = new GameObject();
                stickerTransform = newGameObject.AddComponent<RectTransform>();
                stickerTransform.localScale = Vector3.one * STICKERSCALE;

                var imageComponnent = newGameObject.AddComponent<Image>();
                newGameObject.transform.SetParent(rootStickerParent);
                imageComponnent.sprite = sticker.StickerImage;

                var moveSticker = newGameObject.AddComponent<MoveSticker>();
                moveSticker.Setup(this, stickerTransform);
            }

            stickerTransform = stickerTransform ? stickerTransform : rootStickerParent.GetChild(dataIndex).GetComponent<RectTransform>();
            if (data.hasSetSticker == false)
            {
                stickerTransform.localPosition = defaultStickerPosition;
                continue;
            }
            stickerTransform.localPosition = new Vector3(data.stickerPosition.x, data.stickerPosition.y, defaultStickerPosition.z);
        }
    }

    public void ChangeStickerPosition(RectTransform stickerTransform, Vector2 newPosition)
    {
        var stickerIndex = 0;
        for (stickerIndex = 0; stickerIndex < rootStickerParent.childCount; stickerIndex++)
        {
            if (rootStickerParent.GetChild(stickerIndex).GetComponent<RectTransform>() == stickerTransform) break;
        }

        var fitMax = newPosition.x < stickerMax.x && newPosition.y < stickerMax.y;
        var fitMin = newPosition.x > stickerMin.x && newPosition.y > stickerMin.y;
        if (fitMax && fitMin)
        {
            _stickerBook.changeStickerPosition(stickerIndex, newPosition);
            return;
        }

        var lastPosition = _stickerBook.DataSet[stickerIndex].stickerPosition;
        if (lastPosition != Vector2.zero)
        {
            stickerTransform.localPosition = lastPosition;
            return;
        }
        stickerTransform.localPosition = defaultStickerPosition;
    }


    public void SetToFirst(RectTransform stickerTransform)
    {
        var stickerIndex = 0;
        for (stickerIndex = 0; stickerIndex < rootStickerParent.childCount; stickerIndex++)
        {
            if (rootStickerParent.GetChild(stickerIndex).GetComponent<RectTransform>() == stickerTransform) break;
        }
        _stickerBook.PushToBack(stickerIndex);
        stickerTransform.SetAsLastSibling();
    }
}
