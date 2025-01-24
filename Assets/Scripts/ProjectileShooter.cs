using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootingSocket;
    [SerializeField] PlayerController playerController;

    [SerializeField] float shootingCooldownTime;
    [SerializeField] float windupTime;

    bool canShoot;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (canShoot)
        {
            StartCoroutine(ShootingCoroutine());
        }
    }

    IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(windupTime);
        var proj = Instantiate(projectile, shootingSocket.position, Quaternion.identity);
        if (proj.TryGetComponent<Projectile>(out Projectile booger))
        {
            booger.horizontal = playerController.getHorizontal();
        }

        canShoot = false;
        yield return new WaitForSeconds (shootingCooldownTime);
        canShoot = true;
    }
}
