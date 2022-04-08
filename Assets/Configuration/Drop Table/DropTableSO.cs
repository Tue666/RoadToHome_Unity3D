using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DroppedItem
{
    public ItemSO item;
    public float percent;
}

public class TableItem
{
    public ItemSO item;
    public int quantity;

    public TableItem(DroppedItem droppedItem, int quantity)
    {
        this.item = droppedItem.item;
        this.quantity = quantity;
    }
}

[CreateAssetMenu(fileName = "DropTableSO", menuName = "Configuration/DropTable")]
public class DropTableSO : ScriptableObject
{
    public int dropCount;
    public List<DroppedItem> droppedItems;

    public List<TableItem> GenerateDropTable()
    {
        List<DroppedItem> dropped = new List<DroppedItem>();
        List<DroppedItem> uniqueDropped = new List<DroppedItem>();

        int index = 0;
        while (index < dropCount)
        {
            List<DroppedItem> mainDropped = droppedItems.Where(droppedItem => !uniqueDropped.Any(uniqueItem => uniqueItem == droppedItem)).ToList();
            float totalPercent = mainDropped.Sum(item => item.percent / 100);
            float randomDrop = Random.Range(0, totalPercent);
            float currentRate;
            float nextRate = 0;
            float currentTotal = 0;
            foreach (DroppedItem droppedItem in mainDropped)
            {
                if (droppedItem.item.isUnique)
                    uniqueDropped.Add(droppedItem);
                currentRate = nextRate;
                nextRate = droppedItem.percent / 100;
                currentTotal += nextRate;
                if (randomDrop >= currentRate && (randomDrop < nextRate || randomDrop < currentTotal))
                {
                    dropped.Add(droppedItem);
                    break;
                }
            }
            index++;
        }
        return dropped
           .GroupBy(droppedItem => droppedItem)
           .Select(group => new TableItem(group.Key, group.Count()))
           .ToList();
    }
}
