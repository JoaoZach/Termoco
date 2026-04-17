using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = BuildInventoryText();
    }

    string BuildInventoryText()
    {
        string result = "Inventory:\n";

        foreach (var item in Inventory.Instance.items)
        {
            result += "- " + item + "\n";
        }

        return result;
    }
}