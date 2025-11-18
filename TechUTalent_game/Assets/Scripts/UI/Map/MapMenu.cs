using UnityEngine;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    [SerializeField] private Transform rootParent;
    [SerializeField] private MapDrawer mapDrawer;
    [SerializeField] private Toggle playerToggle;
    [SerializeField] private Color playerToggleColor = Color.blue;

    private Color NORMAL_TOGGLE = Color.white;
    private Image toggleImage;

    [HideInInspector] public bool canSetActive = true;
    private bool _isActive = true;
    private bool _drawMap = true;
    private bool _drawPlayers = true;


    private void Start()
    {
        toggleImage = playerToggle.GetComponent<Image>();
        playerToggle.isOn = _drawPlayers;
        playerToggle.onValueChanged.AddListener(TogglePlayerToggle);
        TogglePlayerToggle(_drawPlayers);

        DisplayMap();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Map")) DisplayMap();
        if (_isActive && _drawPlayers) mapDrawer.DrawPlayers();
    }

    public void DisplayMap()
    {
        if (_isActive == false && canSetActive == false) return;

        _isActive = !_isActive;
        foreach (Transform t in rootParent.GetComponentsInChildren<Transform>(true))
        {
            if (t == gameObject.transform) continue;
            t.gameObject.SetActive(_isActive);
        }

        if (_isActive == false) return;
        if (_drawMap) mapDrawer.DrawMap();
    }

    private void TogglePlayerToggle(bool value)
    {
        toggleImage.color = value ? playerToggleColor : NORMAL_TOGGLE;
        _drawPlayers = value;

        if (value == false) mapDrawer.RemovePlayers();
    }
}
