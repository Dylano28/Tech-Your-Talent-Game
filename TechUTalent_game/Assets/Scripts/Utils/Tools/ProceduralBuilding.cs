using UnityEngine;

[ExecuteInEditMode]
public class ProceduralBuilding : MonoBehaviour
{
    [SerializeField] private float topMargin = 1f;
    [SerializeField] private Sprite topPart;
    [SerializeField] private float middleMargin = 1f;
    [SerializeField] private Sprite middlePart;
    [SerializeField] private float bottomMargin = 1f;
    [SerializeField] private Sprite bottomPart;

    [SerializeField][Range(0, 100)] private float size;

    private SpriteRenderer _topSprite;
    private SpriteRenderer _middleSprite;
    private SpriteRenderer _bottomSprite;


    private void Start()
    {
        if (_topSprite || _middleSprite || _bottomSprite) return;

        _topSprite = new GameObject().AddComponent<SpriteRenderer>();
        _middleSprite = new GameObject().AddComponent<SpriteRenderer>();
        _bottomSprite = new GameObject().AddComponent<SpriteRenderer>();

        _topSprite.transform.SetParent(transform);
        _middleSprite.transform.SetParent(transform);
        _bottomSprite.transform.SetParent(transform);

        _middleSprite.drawMode = SpriteDrawMode.Sliced;
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

        var halfTop = topMargin / 2;
        _topSprite.transform.localPosition = new Vector2(0, (middleMargin * size) - (halfTop * size) + halfTop);

        _middleSprite.transform.localPosition = Vector2.zero;
        _middleSprite.size = new Vector2(1, size);

        var halfBottom = bottomMargin / 2;
        _bottomSprite.transform.localPosition = new Vector2(0, -((middleMargin * size) - (halfBottom * size) + halfBottom));
    }
}
