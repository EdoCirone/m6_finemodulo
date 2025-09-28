using UnityEngine;

public class Healer : Item
{
    [SerializeField] int _healtAmount = 1;

    protected override void OnPickup(Collider other)
    {
        var life = other.GetComponent<LifeController>();
        if (life != null)
            life.AddHp(_healtAmount);
    }
}
