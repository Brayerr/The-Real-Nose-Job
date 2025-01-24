using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileShooter : MonoBehaviour
{
    public UnityEvent<bool> onShotStart;
    public UnityEvent<bool> onShotEnd;

    [SerializeField] Animator animator;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootingSocket;
    [SerializeField] PlayerController playerController;

    [SerializeField] float shootingCooldownTime;
    [SerializeField] float windupTime;

    bool canShoot = true;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canShoot)
            {
                onShotStart?.Invoke(false);
                Shoot();
            }
        }
    }

    void Shoot()
    {
        StartCoroutine(ShootingCoroutine());
    }

    IEnumerator ShootingCoroutine()
    {
        if (animator.GetBool("isRunning"))
        {
            yield return new WaitForSeconds(.2f);
            animator.SetTrigger("ShootingTrigger");
            yield return new WaitForSeconds(windupTime + .2f);
        }
        else
        {
            animator.SetTrigger("ShootingTrigger");
            yield return new WaitForSeconds(windupTime);
        }
        var proj = Instantiate(projectile, shootingSocket.position, Quaternion.identity);
        if (proj.TryGetComponent<Projectile>(out Projectile booger))
        {
            booger.horizontal = playerController.lastHorizontal;
        }
        canShoot = false;
        yield return new WaitForSeconds(.5f);
        onShotEnd?.Invoke(true);
        yield return new WaitForSeconds(shootingCooldownTime);
        canShoot = true;
    }
}
