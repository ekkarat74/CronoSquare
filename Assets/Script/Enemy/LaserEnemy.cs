using System;
using System.Collections;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // ความเร็วในการเคลื่อนที่ของศัตรู
    public float stopYPosition = -3f; // ตำแหน่งแกน Y ที่ศัตรูจะหยุด
    public float moveDownDuration = 5f; // ระยะเวลาที่จะเคลื่อนที่ลงหลังจากรอ
    public float waitTime = 10f; // เวลาที่จะรอก่อนเคลื่อนที่อีกครั้ง

    [Header("Laser Settings")]
    public Transform firePoint; // จุดยิงเลเซอร์
    public LineRenderer lineRenderer; // ตัวแสดงเลเซอร์
    public float damagePerSecond = 10f; // ความเสียหายต่อวินาที
    public float laserDuration = 3f; // ระยะเวลาที่เลเซอร์จะยิง
    public float laserCooldown = 2f; // เวลาที่จะรอก่อนยิงเลเซอร์ใหม่

    // Internal State
    private bool isMoving = true; // ตัวแปรภายในสำหรับตรวจสอบว่าศัตรูกำลังเคลื่อนที่หรือไม่
    private bool isShooting = false; // ตัวแปรภายในสำหรับตรวจสอบว่าศัตรูกำลังยิงหรือไม่

    private void Awake()
    {
        lineRenderer.enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {
        if (isMoving)
        {
            // เคลื่อนที่ศัตรูลงด้านล่าง
            transform.Translate(Vector2.down * speed * Time.deltaTime);

            // ตรวจสอบว่าศัตรูได้ถึงตำแหน่งที่ต้องหยุดแล้วหรือไม่
            if (transform.position.y <= stopYPosition)
            {
                isMoving = false; // หยุดการเคลื่อนที่
                StartCoroutine(WaitAndMoveAgain()); // เรียกใช้ coroutine เพื่อตั้งเวลาและเคลื่อนที่อีกครั้ง
            }
        }
        else if (!isShooting)
        {
            StartCoroutine(WaitAndShootLaser()); // รอ 3 วินาทีแล้วค่อยยิงเลเซอร์
        }

        // อัปเดตตำแหน่งของ LineRenderer ให้ขยับตาม firePoint ในทุกเฟรม
        if (lineRenderer.enabled)
        {
            UpdateLaserPosition();
            CheckLaserHit();
        }
    }

    IEnumerator WaitAndMoveAgain()
    {
        yield return new WaitForSeconds(waitTime); // รอ 10 วินาที
        float elapsedTime = 0f;

        while (elapsedTime < moveDownDuration)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            // อัปเดตตำแหน่งของ LineRenderer ให้ขยับตาม firePoint ในขณะที่เคลื่อนที่
            if (lineRenderer.enabled)
            {
                UpdateLaserPosition();
            }

            yield return null;
        }

        Destroy(gameObject); // ทำลายตัวเองหลังจากเคลื่อนที่เสร็จสิ้น
    }

    IEnumerator WaitAndShootLaser()
    {
        yield return new WaitForSeconds(3f); // รอ 3 วินาทีก่อนยิงเลเซอร์
        StartCoroutine(ShootLaser()); // เริ่มยิงเลเซอร์
    }

    IEnumerator ShootLaser()
    {
        isShooting = true;
        lineRenderer.enabled = true;

        // ยิงเลเซอร์ลงตรง ๆ ตามแกน Y
        Vector2 laserEndPosition = new Vector2(firePoint.position.x, firePoint.position.y - 10f); // กำหนดตำแหน่งปลายเลเซอร์ให้อยู่ด้านล่าง
        lineRenderer.SetPosition(0, firePoint.position); // ตำแหน่งเริ่มต้นของเลเซอร์
        lineRenderer.SetPosition(1, laserEndPosition); // ตำแหน่งปลายของเลเซอร์

        // รอให้เลเซอร์ยิงต่อเนื่อง 3 วินาที
        yield return new WaitForSeconds(laserDuration);

        lineRenderer.enabled = false; // ซ่อนเลเซอร์
        yield return new WaitForSeconds(laserCooldown); // รอ 2 วินาทีก่อนจะยิงใหม่

        isShooting = false; // พร้อมที่จะยิงใหม่
    }

    void UpdateLaserPosition()
    {
        // อัปเดตตำแหน่งของเลเซอร์ในขณะที่ enemy เคลื่อนที่
        Vector2 laserEndPosition = new Vector2(firePoint.position.x, firePoint.position.y - 10f);
        lineRenderer.SetPosition(0, firePoint.position); // ตำแหน่งเริ่มต้นของเลเซอร์
        lineRenderer.SetPosition(1, laserEndPosition); // ตำแหน่งปลายของเลเซอร์
    }

    void CheckLaserHit()
    {
        // ยิง raycast จากตำแหน่ง firePoint ลงไปตามแกน Y
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.down);
        // ตรวจสอบว่าถูกชนกับวัตถุที่มี tag "Player"
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // ลดพลังชีวิตของผู้เล่น
            UnitStats playerHealth = hit.collider.GetComponent<UnitStats>();
            if (playerHealth != null)
            {
                playerHealth.DecreaseHealth(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
