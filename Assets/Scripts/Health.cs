using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] float takeDamageCooldownTime;
    [SerializeField] public UnityEvent<int> onHPChanged;
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
            onHPChanged.Invoke(currentHP);
            StartCoroutine(TakeDamageCooldown());
            if (currentHP <= 0) Kill();
        }
    }

    void Kill()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(takeDamageCooldownTime);
        canTakeDamage = true;
    }
}