using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Inventory.ItemType itemType;

    public float showDistance = 2.5f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= showDistance)
        {
            PickupUIManager.Instance.SetCurrentItem(this);
        }
        else
        {
            // only clear if THIS item was active
            if (PickupUIManager.Instance != null)
                PickupUIManager.Instance.SetCurrentItem(null);
        }
    }

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemType);
        Destroy(gameObject);
    }
}