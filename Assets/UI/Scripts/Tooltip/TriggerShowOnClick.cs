using UnityEngine;
using UnityEngine.UI;

public class TriggerShowOnClick : MonoBehaviour
{
    [Multiline()]
    public string content = "";

    private Button item;

    // Start is called before the first frame update
    void Start()
    {
        item = gameObject.GetComponent<Button>();
        if (item != null) item.onClick.AddListener(() => TooltipManager.Instance.ShowTooltip(content, true, true, true));
    }
}
