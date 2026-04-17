using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupInput : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        ItemPickup item = FindClosestItem();

        if (item != null)
        {
            item.PickUp();
            PickupUIManager.Instance.SetCurrentItem(null);
        }
    }

    ItemPickup FindClosestItem()
    {
        ItemPickup[] items = Object.FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);

        float bestDist = 2.5f;
        ItemPickup closest = null;

        foreach (var item in items)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);

            if (dist < bestDist)
            {
                bestDist = dist;
                closest = item;
            }
        }

        return closest;
    }
}