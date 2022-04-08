using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDropped : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image slotBackground;

    // Start is called before the first frame update
    void Start()
    {
        slotBackground = gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotBackground.color = new Color(1f, 0.5f, 0.1f, 0.8f);
        AudioManager.Instance.PlayEffect("PLAYER", "Hover Item");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotBackground.color = new Color(1f, 1f, 1f, 0.8f);
    }
}
