using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image slotBackground;

    // Start is called before the first frame update
    void Start()
    {
        slotBackground = gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = slotBackground.color;
        color.a = 0.8f;
        slotBackground.color = color;
        AudioManager.Instance.PlayEffect("PLAYER", "Hover Item");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = slotBackground.color;
        color.a = 1f;
        slotBackground.color = color;
    }
}
