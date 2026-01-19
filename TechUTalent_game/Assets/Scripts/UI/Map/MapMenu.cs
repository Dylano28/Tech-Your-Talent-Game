using UnityEngine;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    [SerializeField] private Transform rootParent;
    [SerializeField] private MapDrawer mapDrawer;
    [SerializeField] private Toggle playerToggle;
    [SerializeField] private Button recenter;
    [SerializeField] private Color playerToggleColor = Color.blue;

    private Color NORMAL_TOGGLE = Color.white;
    private Image toggleImage;

    [HideInInspector] public bool canSetActive = true;
    private bool _isActive = true;
    private bool _drawMap = true;
    private bool _drawPlayers = true;
    private bool _drawMarkers = true;


    private void Start()
    {
        toggleImage = playerToggle.GetComponent<Image>();
        playerToggle.isOn = _drawPlayers;
        playerToggle.onValueChanged.AddListener(TogglePlayerToggle);
        recenter.onClick.AddListener(ReCenter);
        TogglePlayerToggle(_drawPlayers);

        DisplayMap();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Map")) DisplayMap();
        if (_isActive && _drawPlayers) mapDrawer.DrawPlayers();
        if (_isActive && _drawMarkers) mapDrawer.DrawMarkers();
    }

    public void DisplayMap()
    {
        if (_isActive == false && canSetActive == false) return;

        _isActive = !_isActive;
        Show(_isActive);

        if (_isActive == false) return;
        if (_drawMap) mapDrawer.DrawMap();
    }

    private void TogglePlayerToggle(bool value)
    {
        toggleImage.color = value ? playerToggleColor : NORMAL_TOGGLE;
        _drawPlayers = value;

        if (value == false) mapDrawer.RemovePlayers();
    }

    private void ReCenter()
    {
        mapDrawer.transform.localPosition = Vector3.zero;
        if (_drawPlayers == false) mapDrawer.DrawMap();
    }

    private void Show(bool showElements)
    {
        foreach (Transform t in rootParent.GetComponentsInChildren<Transform>(true))
        {
            if (t == gameObject.transform) continue;
            t.gameObject.SetActive(showElements);
        }
    }


    public void SetCanDisplay(bool value)
    {
        if (value == false) Show(false);
        canSetActive = value;
    }
}
