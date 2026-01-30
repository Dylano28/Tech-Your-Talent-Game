using UnityEngine;

// DONT USE !!!

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlatformExpander : MonoBehaviour
{
    [SerializeField] private float width = 1;

    [Header("Sprite")]
    [SerializeField] private bool useSpriteHeight;

    [Header("Settings")]
    [SerializeField] private float colliderHeight = 0.1f;
    [SerializeField] private float colliderOffset= 0.45f;

    private BoxCollider2D _coll;
    private SpriteRenderer _platformSprite;

    private void Start()
    {
        _coll = GetComponent<BoxCollider2D>();
        _platformSprite = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if (_coll == null || _platformSprite == null) return;

        var spriteSize = useSpriteHeight ? new Vector2(1, width) : new Vector2(width, 1);
        _platformSprite.drawMode = SpriteDrawMode.Tiled;
        _platformSprite.size = spriteSize;

        _coll.size = new Vector2(width, colliderHeight);
        _coll.offset = new Vector2(0, colliderOffset);
    }
}
