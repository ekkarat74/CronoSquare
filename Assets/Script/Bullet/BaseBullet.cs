using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    [SerializeField] private int Damage;
    
    // ตัวแปรสำหรับการกำหนดทิศทาง
    public Vector3 moveDirection = Vector3.down; // ค่าเริ่มต้นเป็นการเคลื่อนที่ลง

    public virtual void Update()
    {
        // ใช้ moveDirection เพื่อกำหนดทิศทางของกระสุน
        transform.Translate(moveDirection * speed * Time.deltaTime);
        
        // ทำลายกระสุนหลังจากผ่าน lifetime
        OnDestroy();
    }

    public void OnDestroy()
    {
        // ทำลายกระสุนเมื่อหมดอายุการใช้งาน
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ตรวจสอบว่า Tag ที่ชนคือ Enemy หรือ BulletEnemy หรือไม่
        if (collision.CompareTag("Enemy") || collision.CompareTag("BulletEnemy"))
        {
            // ถ้าเป็น Enemy หรือ BulletEnemy ให้ข้ามไปไม่ทำอะไร (ทะลุผ่าน)
            return;
        }
        if(collision.CompareTag("Clear"))
        {
            Destroy(gameObject);
        }

        // ตรวจสอบว่า Tag ที่ชนคือ Player หรือไม่
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player -50 ");
            UnitStats playerStats = collision.gameObject.GetComponent<UnitStats>();
            if (playerStats != null)
            {
                // ลดค่าพลังชีวิตของ Player
                playerStats.DecreaseHealth(Damage);
                
                // ทำลายกระสุนเมื่อชนกับ Player
                Destroy(gameObject); 
            }
            else
            {
                Debug.LogError("Cannot decrease health. UnitStats is null.");
            }
        }
    }
}