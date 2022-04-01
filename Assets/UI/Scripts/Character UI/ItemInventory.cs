using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image potIcon;

    // Start is called before the first frame update
    void Start()
    {
        potIcon = gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = potIcon.color;
        color.a = 0.8f;
        potIcon.color = color;
        AudioManager.Instance.PlayEffect("PLAYER", "Hover Item Iventory");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color color = potIcon.color;
        color.a = 1f;
        potIcon.color = color;
    }
}
