using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemWindowManager : MonoBehaviour
{
    public static SystemWindowManager Instance { get; private set; }

    [SerializeField] private GameObject systemWindow;
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject footer;
    [SerializeField] private TMP_Text content;
    [SerializeField] private LayoutElement layoutElement;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        HandlePosition("TOP-RIGHT"); 
    }

    void HandleColor(string status)
    {
        Image windowBackground = systemWindow.GetComponent<Image>();
        Image headerIcon = header.transform.GetChild(0).GetComponent<Image>();
        Image footerIcon = footer.GetComponent<Image>();
        switch (status)
        {
            case "B":
                windowBackground.color = new Color(0.6f, 0.6f, 1f, 0.8f);
                headerIcon.color = new Color(0.8f,0.8f, 1f, 1f);
                footerIcon.color = new Color(0.8f, 0.8f, 1f, 1f);
                break;
            case "A":
                windowBackground.color = new Color(0.3f, 0.6f, 0.5f, 0.8f);
                headerIcon.color = new Color(0.3f, 0.8f, 0.6f, 1f);
                footerIcon.color = new Color(0.3f, 0.8f, 0.6f, 1f);
                break;
            case "S":
                windowBackground.color = new Color(0.7f, 0.3f, 0.2f, 0.8f);
                headerIcon.color = new Color(1f, 0.5f, 0.4f, 1f);
                footerIcon.color = new Color(1f, 0.5f, 0.4f, 1f);
                break;
            case "SS":
                windowBackground.color = new Color(0.8f, 0.5f, 0.2f, 0.8f);
                headerIcon.color = new Color(0.9f, 0.7f, 0.4f, 1f);
                footerIcon.color = new Color(0.9f, 0.7f, 0.4f, 1f);
                break;
            case "SSS":
                windowBackground.color = new Color(0.8f, 0.8f, 0.1f, 0.8f);
                headerIcon.color = new Color(1f, 0.9f, 0.5f, 1f);
                footerIcon.color = new Color(1f, 0.9f, 0.5f, 1f);
                break;
            case "SUCCESS":
                windowBackground.color = new Color(0.2f, 0.9f, 0.3f, 0.8f);
                headerIcon.color = new Color(1f, 1f, 1f, 1f);
                footerIcon.color = new Color(1f, 1f, 1f, 1f);
                break;
            case "WARNING":
                windowBackground.color = new Color(1f, 0.7f, 0.1f, 0.8f);
                headerIcon.color = new Color(1f, 1f, 1f, 1f);
                footerIcon.color = new Color(1f, 1f, 1f, 1f);
                break;
            case "DANGER":
                windowBackground.color = new Color(1f, 0f, 0f, 0.8f);
                headerIcon.color = new Color(1f, 1f, 1f, 1f);
                footerIcon.color = new Color(1f, 1f, 1f, 1f);
                break;
            default:
                // Default will blue
                windowBackground.color = new Color(0f, 0.4f, 1f, 0.8f);
                headerIcon.color = new Color(1f, 1f, 1f, 1f);
                footerIcon.color = new Color(1f, 1f, 1f, 1f);
                break;
        }
    }
    void HandlePosition(string showAtPosition)
    {
        RectTransform systemWindowRectTransform = systemWindow.GetComponent<RectTransform>();
        systemWindowRectTransform.pivot = new Vector2(0.5f, 0.5f);
        systemWindowRectTransform.offsetMax = Vector2.zero;
        systemWindowRectTransform.offsetMin = Vector2.zero;
        systemWindowRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        systemWindowRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        switch (showAtPosition)
        {
            case "MOUSE":
                Vector2 mousePosition = Input.mousePosition;
                systemWindowRectTransform.pivot = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
                systemWindowRectTransform.position = mousePosition;
                break;
            case "TOP-RIGHT":
                systemWindowRectTransform.anchorMax = new Vector2(0.8f, 0.8f);
                systemWindowRectTransform.anchorMin = new Vector2(0.8f, 0.8f);
                break;
            default:
                // Default will placed at center screen (setting at the top of this function)
                break;
        }
    }

    public void ShowWindowSystem(
        string status,
        string _content,
        string showAtPosition = "CENTER",
        bool autoHide = true,
        bool isRaycasting = true,
        bool headerIcon = true,
        bool footerIcon = true
    )
    {
        AudioManager.Instance.PlayEffect("PLAYER", "System Window Show");
        HandleColor(status);

        // Enable header & footer if needed or otherwise
        header.SetActive(headerIcon);
        footer.SetActive(footerIcon);

        // Auto resize tooltip
        content.text = _content;
        layoutElement.enabled = content.preferredWidth >= layoutElement.preferredWidth;

        // Where the window will placed
        HandlePosition(showAtPosition);

        // Enable raycaster if needed or otherwise
        GraphicRaycaster raycaster = systemWindow.GetComponentInParent<GraphicRaycaster>();
        raycaster.enabled = isRaycasting;

        systemWindow.SetActive(true);

        // Enable auto hide if needed or otherwise
        if (autoHide) Invoke("HideSystemWindow", 4f);
    }

    public void HideSystemWindow()
    {
        systemWindow.SetActive(false);
    }
}
