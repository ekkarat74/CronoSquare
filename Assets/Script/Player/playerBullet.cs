using UnityEngine;

public class playerBullet : MonoBehaviour
{

    public float timeBeforeDisappear;
    public int bulletDamage;
    public float rotation;
    public float speed;
    private float timer;
    private Vector2 direction;

    private Renderer render;
    private Color colors;

    private void Start()
    {
        render = GetComponent<Renderer>();
        colors = render.material.color;
        colors.a = 0.5f;
        render.material.color = colors;
    }

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBeforeDisappear)
        {
            Destroy(gameObject);
            timeBeforeDisappear = 0;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("BossEnemy"))
        {

            EnemyStats playerStats = collision.gameObject.GetComponent<EnemyStats>();
            if (playerStats != null)
            {
                playerStats.DecreaseHealthE(bulletDamage);
            }
            else
            {
                Debug.LogError("Cannot decrease health. UnitStats is null.");
            }
            Destroy(gameObject);

        }
        else if (collision.CompareTag("BlockBullet"))
        {
            Destroy(gameObject);
        }
    }

}

