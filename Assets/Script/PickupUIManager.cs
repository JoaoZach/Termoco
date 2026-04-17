using UnityEngine;
using TMPro;

public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance;

    public GameObject uiPanel;      // whole UI (canvas or panel)
    public TMP_Text uiText;

    private ItemPickup currentItem;

    void Awake()
    {
        Instance = this;
        HideUI();
    }

    void Update()
    {
        if (currentItem == null)
        {
            HideUI();
        }
    }

    public void SetCurrentItem(ItemPickup item)
    {
        currentItem = item;

        if (item == null)
        {
            HideUI();
            return;
        }

        ShowUI();
    }

    void ShowUI()
    {
        uiPanel.SetActive(true);
        uiText.text = "Press E to pick up";
    }

    void HideUI()
    {
        uiPanel.SetActive(false);
    }
}