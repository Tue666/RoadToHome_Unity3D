using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject footer;
    [SerializeField] private TMP_Text content;
    [SerializeField] private LayoutElement layoutElement;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowTooltip(string _content, bool isRaycasting = true, bool headerIcon = false, bool footerIcon = false)
    {
        if (!headerIcon)
            header.SetActive(false);
        else
            header.SetActive(true);
        if (!footerIcon)
            footer.SetActive(false);
        else
            footer.SetActive(true);

        // Auto resize tooltip
        content.text = _content;
        layoutElement.enabled = content.preferredWidth >= layoutElement.preferredWidth;

        Vector2 mousePosition = Input.mousePosition;
        RectTransform tooltipRectTransform = tooltip.GetComponent<RectTransform>();
        tooltipRectTransform.pivot = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        tooltip.transform.position = mousePosition;

        // Enable raycaster if needed or otherwise
        GraphicRaycaster raycaster = tooltip.GetComponentInParent<GraphicRaycaster>();
        raycaster.enabled = isRaycasting;
        
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
