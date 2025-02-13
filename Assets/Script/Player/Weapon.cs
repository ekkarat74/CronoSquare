using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float timer;
    public float fireRate;
    public float bulletSpeed = 10f;
    public bool canShoot;
    [SerializeField] private int bulletAmount;
    [SerializeField] private float startAngle = 0f, endAngle = 90f;

    private Vector2 bulletMoveDirection;

    public void Shoot()
    {
        if (!canShoot)
        {
            timer += Time.deltaTime;
            if (timer > fireRate)
            {
                canShoot = true;
                timer = 0;
            }
        }
        if (canShoot == true)
        {
            canShoot = false;
            if (bulletAmount <= 1)
            {
                startAngle = 0;
                endAngle = 0;
                float angleStep = (endAngle - startAngle) / (float)bulletAmount;
                float angle = startAngle;
                for (int i = 0; i < bulletAmount; i++)
                {

                    // Calculate bullet direction
                    float bulDirX = Mathf.Sin(angle * Mathf.PI / 180f);
                    float bulDirY = Mathf.Cos(angle * Mathf.PI / 180f);

                    Vector2 bulletDirection = new Vector2(bulDirX, bulDirY).normalized;

                    // Instantiate the bullet
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                    // Set bullet direction and speed
                    playerBullet bulletComponent = bullet.GetComponent<playerBullet>();
                    if (bulletComponent != null)
                    {
                        bulletComponent.Initialize(bulletDirection, bulletSpeed);
                    }

                    angle += angleStep;
                }
            }
            else
            {
                float angleStep = (endAngle - startAngle) / (float)bulletAmount;
                float angle = startAngle;
                for (int i = 0; i < bulletAmount; i++)
                {

                    // Calculate bullet direction
                    float bulDirX = Mathf.Sin(angle * Mathf.PI / 180f);
                    float bulDirY = Mathf.Cos(angle * Mathf.PI / 180f);

                    Vector2 bulletDirection = new Vector2(bulDirX, bulDirY).normalized;

                    // Instantiate the bullet
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                    // Set bullet direction and speed
                    playerBullet bulletComponent = bullet.GetComponent<playerBullet>();
                    if (bulletComponent != null)
                    {
                        bulletComponent.Initialize(bulletDirection, bulletSpeed);
                    }

                    angle += angleStep;
                }

            }
        }
    }

    public void increaseBulletAmount(int amount)
    {
        bulletAmount += amount;
        Debug.Log("bullet increase" + bulletAmount);
        if (bulletAmount > 3)
        {
            bulletAmount = 3;
        }
    }

    public void increaseFireRate(float amount)
    {
        fireRate = fireRate - amount;
        if (fireRate < 0.5)
        {
            fireRate = 0.5f;
        }
        Debug.Log("fire rate increase" + fireRate);
    }

    public void increaseLife()
    {
        UnitStats stats = new UnitStats();
        stats.CurHP += 1;
        if (stats.CurHP > 3)
        {
            stats.CurHP = 3;
        }
        Debug.Log(stats.CurHP);
    }

    public void Null01()
    {
        Debug.Log("Get nothing");
    }
}
