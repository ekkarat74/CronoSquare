using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon gun;

    private Vector2 moveDirection;
    private Vector2 lastPosition;

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        gun.Shoot();

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        lastPosition = rb.position; // เก็บตำแหน่งล่าสุดของผู้เล่น
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // เมื่อชน Block ให้หยุดการเคลื่อนไหวในทิศทางที่ชน
            rb.velocity = Vector2.zero; 
            rb.position = lastPosition; // คืนค่าตำแหน่งล่าสุดก่อนชน
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // เมื่ออยู่ใน Trigger ของ Block ให้หยุดการเคลื่อนไหวในทิศทางที่ชน
            rb.position = lastPosition; // ป้องกันการเคลื่อนไปข้างหน้าเมื่อยังชน Block
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            // เมื่อออกจาก Block ให้เคลื่อนที่ตามปกติ
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }
}