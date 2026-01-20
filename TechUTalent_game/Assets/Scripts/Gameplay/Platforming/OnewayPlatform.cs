using UnityEngine;

// DONT USE !!!

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformEffector2D))]
public class OnewayPlatform : MonoBehaviour
{
    [SerializeField] private float width = 1;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer platformSprite;
    [SerializeField] private bool useSpriteHeight;

    [Header("Settings")]
    [SerializeField] private float colliderHeight = 0.1f;
    [SerializeField] private float colliderOffset= 0.45f;

    private BoxCollider2D _coll;

    private void Start()
    {
        _coll = GetComponent<BoxCollider2D>();
        _coll.usedByEffector = true;
    }

    private void OnValidate()
    {
        var spriteSize = useSpriteHeight ? new Vector2(1, width) : new Vector2(width, 1);
        platformSprite.drawMode = SpriteDrawMode.Tiled;
        platformSprite.size = spriteSize;

        _coll.size = new Vector2(width, colliderHeight);
        _coll.offset = new Vector2(0, colliderOffset);
    }
}
