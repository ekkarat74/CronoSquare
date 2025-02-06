using UnityEngine;

public class BulletMissile : BaseBullet
{
    // ความเร็วในการติดตามเป้าหมายของกระสุน (มิสไซล์)
    public float trackingSpeed = 5f; 

    // ตัวแปรสำหรับเก็บตำแหน่งของวัตถุที่มีแท็ก 'Player'
    private Transform player;
    private Vector2 direction;

    private Rigidbody2D rb;
    private float rotateSpeed = 200f;
    float rotateAmount;


    private void Start()
    {
        // ค้นหา GameObject ที่มีแท็ก 'Player' และเก็บตำแหน่งในตัวแปร player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        // ตรวจสอบว่ามีวัตถุที่มีแท็ก 'Player' อยู่หรือไม่
        if (player != null)
        {
            // คำนวณทิศทางจากกระสุนไปยังตำแหน่งของผู้เล่นตราบใดที่แกน y อยู่สูงกว่าผู้เล่น
            if (transform.position.y > player.position.y)
            {
                direction = (Vector2)player.position - (Vector2)transform.position;
                direction.Normalize();
                transform.Translate(direction * trackingSpeed * Time.deltaTime);
                rotateAmount = Vector3.Cross(direction, transform.up).z;
                rb.angularVelocity = rotateAmount * rotateSpeed;
                // หมุ่นกระสุนไปหาผู้เล่น
            }
            else
            {
                rotateAmount = 0;
                rb.angularVelocity = rotateAmount * rotateSpeed;

                transform.Translate(direction * trackingSpeed * Time.deltaTime);
            }




            // เคลื่อนที่กระสุนในทิศทางของผู้เล่นด้วยความเร็วที่กำหนด

            // เรียกฟังก์ชันทำลายกระสุนหลังจากทำงาน
            base.OnDestroy();
        }
        else
        {
            // แสดงข้อความเตือนถ้าหาผู้เล่นไม่เจอ
            Debug.LogWarning("Player not found!");
            
            // เรียกฟังก์ชันทำลายกระสุน
            base.OnDestroy();
        }
    }
}