using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerBookDisplay : MonoBehaviour
{
    [SerializeField] private Vector2 defaultStickerPosition;
    [SerializeField] private Vector2 stickerMin;
    [SerializeField] private Vector2 stickerMax;

    [SerializeField] private RectTransform rootStickerParent;
    [SerializeField] private GameObject rootStickerBook;

    private StickerBook _stickerBook;
    private const float STICKERSCALE = 0.035f;


    private void Start()
    {
        _stickerBook = StickerBook.instance;
        rootStickerBook.SetActive(false);
    }

    // TEST, DELETE LATER
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetStickerField();
        }
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
                var imageComponnent = newGameObject.AddComponent<Image>();
                newGameObject.transform.SetParent(rootStickerParent);
                imageComponnent.sprite = sticker.StickerImage;

                stickerTransform = newGameObject.GetComponent<RectTransform>();
                stickerTransform.localScale = Vector3.one * STICKERSCALE;
            }

            stickerTransform = stickerTransform ? stickerTransform : rootStickerParent.GetChild(dataIndex).GetComponent<RectTransform>();
            if (data.hasSetSticker == false)
            {
                stickerTransform.position = (Vector3)defaultStickerPosition;
                continue;
            }
            stickerTransform.position = (Vector3)data.stickerPosition;
        }
    }

    public void ChangeStickerPosition(int changedStickerIndex, Vector2 newPosition)
    {
        var fitMax = newPosition.x < stickerMax.x && newPosition.y < stickerMax.y;
        var fitMin = newPosition.x > stickerMin.x && newPosition.y > stickerMin.y;
        if (fitMax && fitMin)
        {
            _stickerBook.changeStickerPosition(changedStickerIndex, newPosition);
            return;
        }
        var changedTransform = rootStickerParent.GetChild(changedStickerIndex).GetComponent<RectTransform>();
        changedTransform.position = defaultStickerPosition;
    }
}
