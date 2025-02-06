using UnityEngine;

public class BulletSpread : BaseBullet
{
    public float damageRadius = 5f; // รัศมีของความเสียหาย

    private void OnDestroy()
    {
        // เมื่อกระสุนถูกทำลาย ให้สร้างความเสียหายแบบกระจาย
        base.OnDestroy();
        SpreadDamage();
    }

    private void SpreadDamage()
    {
        // หาวัตถุทั้งหมดภายในรัศมีของความเสียหาย
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        // ลูปผ่านวัตถุทั้งหมดที่โดนกระสุน
        foreach (var hitCollider in hitColliders)
        {
            // ทำลายวัตถุที่อยู่ในรัศมีของความเสียหาย
            Destroy(hitCollider.gameObject);
        }

        // เพิ่ม Debug เพื่อตรวจสอบการกระจายความเสียหาย
        Debug.Log("Damage spread in radius: " + damageRadius);
    }

    // ฟังก์ชันสำหรับวาด Gizmos เพื่อดู Damage Radius ใน Scene View
    private void OnDrawGizmos()
    {
        // กำหนดสีของ Gizmos
        Gizmos.color = Color.red;

        // วาดวงกลมที่ตำแหน่งของกระสุนด้วยรัศมีที่กำหนด
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}