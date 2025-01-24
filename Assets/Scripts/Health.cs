using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] float takeDamageCooldownTime;
    bool canTakeDamage = true;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHP -= damage;
            StartCoroutine(TakeDamageCooldown());
            if (currentHP <= 0) Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(takeDamageCooldownTime);
        canTakeDamage = true;
    }
}