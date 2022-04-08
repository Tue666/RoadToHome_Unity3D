using UnityEngine;

public enum Rank
{
    B,
    A,
    S,
    SS,
    SSS
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Configuration/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Rank rank;
    public Sprite icon;
    [Multiline()]
    public string description;
    public int maximum;
    public int weight;
    public float sellPrice;
    public bool isUnique;
}
