using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MoveSticker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UnityEvent<RectTransform, Vector2> _onSetPosition;
    private UnityEvent<RectTransform> _setFirst;

    private RectTransform _rectTransform;
    private bool _isDragging = false;
    private bool _isActive = false;
    private float _zPos;


    public MoveSticker(StickerBookDisplay display, RectTransform newTransform)
    {
        _onSetPosition = new UnityEvent<RectTransform, Vector2>();
        _setFirst = new UnityEvent<RectTransform>();

        _onSetPosition.AddListener(display.ChangeStickerPosition);
        _setFirst.AddListener(display.SetToFirst);

        _rectTransform = newTransform;
        _zPos = -display.DefaultStickerPosition.z;
    }

    public void Setup(StickerBookDisplay display, RectTransform newTransform)
    {
        _onSetPosition = new UnityEvent<RectTransform, Vector2>();
        _setFirst = new UnityEvent<RectTransform>();

        _onSetPosition.AddListener(display.ChangeStickerPosition);
        _setFirst.AddListener(display.SetToFirst);

        _rectTransform = newTransform;
        _zPos = -display.DefaultStickerPosition.z;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0)) return;
        _isActive = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDragging) return;
        _isActive = false;
    }



    private void Update()
    {
        if (_isActive == false) return;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_isDragging == true) return;
            _isDragging = true;
            _setFirst.Invoke(_rectTransform);
        }
        else
        {
            if (_isDragging == false) return;
            _isDragging = false;
            _onSetPosition.Invoke(_rectTransform, _rectTransform.position);
        }
    }

    private void FixedUpdate()
    {
        if (_isDragging  == false) return;
        var mousePos = Input.mousePosition;
        _rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _zPos));
    }
}
