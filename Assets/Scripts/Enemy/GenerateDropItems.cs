using System.Collections.Generic;
using UnityEngine;

public class GenerateDropItems : MonoBehaviour
{
    private List<TableItem> tableItems;
    private bool isOpening = false;
    private bool isInitialized = false;

    // Update is called once per frame
    void Update()
    {
        HandleTableOpen();
        HandleTookAll();
    }

    void OnDestroy()
    {
        if (isOpening)
            UIManager.Instance.HideView("Dropped Items Table");
    }

    void HandleTookAll()
    {
        if (tableItems.Count == 0)
            Destroy(gameObject);
    }

    void HandleTableOpen()
    {
        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
        if (distance <= 1 && !isOpening)
        {
            if (isInitialized)
            {
                isOpening = true;
                DroppedItemsUI.Instance.InitializeDropTable(ref tableItems);
                UIManager.Instance.ShowView("Dropped Items Table");
            }
        }
        if (distance > 1 && isOpening)
        {
            isOpening = false;
            UIManager.Instance.HideView("Dropped Items Table");
        }
    }

    public void InitializeDropTable(DropTableSO dropTableSO)
    {
        tableItems = dropTableSO.GenerateDropTable();
        isInitialized = true;
    }
}
