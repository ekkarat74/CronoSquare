using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhaseBoss : MonoBehaviour
{
    public static PhaseBoss instance;

    // ตัวแปรเก็บ Prefab สำหรับ Phase ต่อไป
    public GameObject nextPhasePrefab;

    // ค่าชีวิตของ Boss
    public float health = 100f;

    // การตั้งค่า BulletLine
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
    public float bulletSpeed = 10.0f;

    // การตั้งค่าการเคลื่อนที่
    public float moveSpeed = 2f;        // ความเร็วในการเคลื่อนที่
    public float stopDistance = -3f;    // ตำแหน่งที่หยุด

    // Animation
    public Animator animator;
    private bool hasPlayedAnimation = false;
    private bool hasAnimationFinished = false;

    private int currentIndex = 0;

    void Awake()
    {
        instance = this;

        // เริ่มต้น nextFireTime สำหรับแต่ละ BulletLine
        foreach (BulletLine line in bulletLines)
        {
            line.nextFireTime = Time.time;
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            OnBossDestroyed();
            return;
        }

        MoveBoss();

        if (hasPlayedAnimation && hasAnimationFinished && Time.time >= bulletLines[currentIndex].nextFireTime)
        {
            Shoot();
        }
    }

    // ฟังก์ชันสำหรับลดค่าชีวิตของ Boss
    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    // ฟังก์ชันที่จะถูกเรียกเมื่อ Boss ถูกทำลาย
    void OnBossDestroyed()
    {
        if (nextPhasePrefab != null)
        {
            Instantiate(nextPhasePrefab, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    // ฟังก์ชันตรวจจับการชน
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bulletPlayer"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }

    // การเคลื่อนที่ของ Boss
    void MoveBoss()
    {
        if (transform.position.y > stopDistance && !hasPlayedAnimation)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        else if (!hasPlayedAnimation)
        {
            PlayAnimation();
        }
    }

    // การเล่นแอนิเมชัน
    void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("PlayAnimation");
            hasPlayedAnimation = true;
            StartCoroutine(CheckAnimationFinished());
        }
    }

    // ตรวจสอบว่าแอนิเมชันเสร็จสิ้นหรือยัง
    private IEnumerator CheckAnimationFinished()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        hasAnimationFinished = true;
    }

    // ฟังก์ชันยิงกระสุน
    private void Shoot()
    {
        BulletLine currentLine = bulletLines[currentIndex];

        for (int i = 0; i < currentLine.bulletSpawnPoints.Length; i++)
        {
            Transform spawnPoint = currentLine.bulletSpawnPoints[i];
            GameObject bullet = Instantiate(currentLine.bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 moveDirection = currentLine.moveDirections[i];
                rb.velocity = moveDirection * bulletSpeed;
                //rb.velocity = spawnPoint.right * bulletSpeed; // ยิงตามทิศทางที่ bulletSpawnPoint หันไป
            }

            BaseBullet baseBullet = bullet.GetComponent<BaseBullet>();
            if (baseBullet != null)
            {
                baseBullet.moveDirection = currentLine.moveDirections[i];

            }
        }

        // อัปเดต nextFireTime
        currentLine.nextFireTime = Time.time + currentLine.cooldown;

        // ไปที่ BulletLine ถัดไป
        currentIndex = (currentIndex + 1) % bulletLines.Length;
    }
}