using UnityEngine;

public class Coin : Item
{
    protected override void OnPickup(Collider other)
    {
        // Qui aggiorni il CollectableManager
        CollectableManager.Instance.TakeCollectable(this as Item);
    }
}
