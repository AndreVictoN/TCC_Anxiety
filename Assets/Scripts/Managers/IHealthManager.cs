using Core.Singleton;
using UnityEngine;
using UnityEngine.UI;

public interface IHealthManager
{
    public void TakeDamage(float damage){}

    public void Heal(float healingAmount){}
}
