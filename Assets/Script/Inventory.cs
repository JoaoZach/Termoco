using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemType> items = new List<ItemType>();

    public enum ItemType
    {
        Beer,
        Wine,
        Cocktail
    }

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemType item)
    {
        items.Add(item);
        Debug.Log("Added: " + item);
    }

    public bool UseItem(ItemType item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            return true;
        }

        return false;
    }
}