using UnityEngine;

public class AimBullet : BaseBullet
{
    public float trackingSpeed = 5f;

    private Transform player;
    private Vector2 direction;

    private Rigidbody2D rb;
    private float rotateSpeed = 200f;
    float rotateAmount;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        direction = (Vector2)player.position - (Vector2)transform.position;
        direction.Normalize();
    }

    private void Update()
    {
        if (player != null)
        {
            
            transform.Translate(direction * speed * Time.deltaTime);
            
            base.OnDestroy();
        }
        else
        {
            Debug.LogWarning("Player not found!");

            base.OnDestroy();
        }
    }
}