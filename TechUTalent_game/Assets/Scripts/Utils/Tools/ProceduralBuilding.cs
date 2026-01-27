using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralBuilding : MonoBehaviour
{
    [SerializeField] private float topMargin = 1f;
    [SerializeField] private float bottomMargin = 1f;
    [SerializeField] private Sprite topPart;
    [SerializeField] private Sprite middlePart;
    [SerializeField] private Sprite bottomPart;

    [SerializeField][Range(0, 1000)] public float size;
    [SerializeField] private float width = 1f;

    [SerializeField] private int zOrder = 0;

    private SpriteRenderer _topSprite;
    private SpriteRenderer _middleSprite;
    private SpriteRenderer _bottomSprite;
    private const int SPRITE_CHILDREN = 3;


    private void Start()
    {
        var hasSprite = 0;
        for (int index = 0; index < transform.childCount; index++) if (transform.GetChild(index).GetComponent<SpriteRenderer>()) hasSprite++;
        if (hasSprite >= SPRITE_CHILDREN)
        {
            var renderers = transform.GetComponentsInChildren<SpriteRenderer>();

            _topSprite = renderers[0];
            _middleSprite = renderers[1];
            _bottomSprite = renderers[2];

            CalculateSize();
            return;
        }

        // Upon first time
        _topSprite = new GameObject().AddComponent<SpriteRenderer>();
        _middleSprite = new GameObject().AddComponent<SpriteRenderer>();
        _bottomSprite = new GameObject().AddComponent<SpriteRenderer>();

        _topSprite.transform.SetParent(transform);
        _middleSprite.transform.SetParent(transform);
        _bottomSprite.transform.SetParent(transform);

        _middleSprite.drawMode = SpriteDrawMode.Tiled;
        CalculateSize();
    }

    private void OnValidate()
    {
        if (!_topSprite || !_middleSprite || !_bottomSprite) return;
        CalculateSize();
    }

    private void CalculateSize()
    {
        _topSprite.sprite = topPart;
        _middleSprite.sprite = middlePart;
        _bottomSprite.sprite = bottomPart;

        _topSprite.sortingOrder = zOrder + 1;
        _middleSprite.sortingOrder = zOrder;
        _bottomSprite.sortingOrder = zOrder;

        _middleSprite.transform.localPosition = Vector2.zero;
        _middleSprite.size = new Vector2(width, size);

        var middleExtents = _middleSprite.bounds.extents;
        var middleY = middleExtents.y;

        _topSprite.transform.localPosition = new Vector2(0, middleY + topMargin);
        _bottomSprite.transform.localPosition = new Vector2(0, -(middleY - bottomMargin));
    }
}
