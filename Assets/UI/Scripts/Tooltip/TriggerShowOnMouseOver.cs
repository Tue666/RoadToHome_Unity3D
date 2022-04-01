using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerShowOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Multiline()]
    public string content = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.ShowTooltip(content, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
