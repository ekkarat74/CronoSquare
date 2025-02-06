using System;
using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public static BaseEnemy Instance { get; private set; }

    [System.Serializable]
    public class BulletLine
    {
        public GameObject bulletPrefab;
        public Transform[] bulletSpawnPoints;
        public Vector3[] moveDirections;
        public float cooldown = 1.0f; // เพิ่ม cooldown สำหรับแต่ละ BulletLine
        [HideInInspector] public float nextFireTime; // ใช้ติดตามเวลาถัดไปที่อนุญาตให้ยิงได้
    }

    public BulletLine[] bulletLines;
    public float fireRate = 1.0f;
    public float bulletSpeed = 10.0f;

    [Header("Movement")]
    public float stopDistance = 5.0f;
    public float moveSpeed = 2.0f;
    private bool hasStopped = false;
    private bool isWaiting = false;
    private float stopTime = 0f;
    [SerializeField] private float moveAgainTime = 10f;
    [SerializeField] private float moveDuration = 5f;

    [Header("Animation")]
    public Animator animator;

    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (BulletLine line in bulletLines)
        {
            line.nextFireTime = Time.time;
        }
    }

    public void Update()
    {
        Move();
        StartCoroutine(PlayAnimationAndShoot());
    }

    public void Move()
    {
        if (!hasStopped)
        {
            if (transform.position.y > stopDistance)
            {
                transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            }
            else
            {
                hasStopped = true;
                stopTime = Time.time;
            }
        }
        else if (hasStopped && !isWaiting)
        {
            if (Time.time >= stopTime + moveAgainTime)
            {
                isWaiting = true;
                stopTime = Time.time;
            }
        }
        else if (isWaiting)
        {
            if (Time.time < stopTime + moveDuration)
            {
                transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator PlayAnimationAndShoot()
    {
        // เล่น animation
        animator.SetTrigger("PlayAnimation"); // ใช้ Trigger สำหรับ animation

        // รอจนกว่า animation จะเสร็จ
        // สมมติว่า animation ใช้เวลา 1 วินาที (ปรับเวลาให้ตรงกับ duration ของ animation ของคุณ)
        yield return new WaitForSeconds(2f);

        // เรียกใช้ฟังก์ชัน Shoot เพื่อยิงกระสุน
        Shoot();
    }

    private void Shoot()
    {
        BulletLine currentLine = bulletLines[currentIndex];

        if (Time.time >= currentLine.nextFireTime)
        {
            for (int i = 0; i < currentLine.bulletSpawnPoints.Length; i++)
            {
                Transform spawnPoint = currentLine.bulletSpawnPoints[i];
                GameObject bullet = Instantiate(currentLine.bulletPrefab, spawnPoint.position, spawnPoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector3 moveDirection = currentLine.moveDirections[i];
                    rb.velocity = moveDirection * bulletSpeed;
                }

                BaseBullet baseBullet = bullet.GetComponent<BaseBullet>();
                if (baseBullet != null)
                {
                    baseBullet.moveDirection = currentLine.moveDirections[i];
                }
            }

            // อัปเดต nextFireTime ตาม cooldown
            currentLine.nextFireTime = Time.time + currentLine.cooldown;

            // ย้ายไปที่ BulletLine ถัดไป
            currentIndex = (currentIndex + 1) % bulletLines.Length;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Clear"))
        {
            Destroy(gameObject);
        }
    }
}
