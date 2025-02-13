using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invincibleShort : MonoBehaviour
{
    public float invincibleTime = 1.5f;

    Renderer render;

    Color colors;

    void Start()
    {
        GetComponent<UnitStats>();
        render = GetComponent<Renderer>();
        colors = render.material.color;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletEnemy") || collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(GetEnumerator());
    }

    IEnumerator GetEnumerator()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        colors.a = 0.5f;
        render.material.color = colors;
        Debug.Log("Invincibility");
        yield return new WaitForSeconds(invincibleTime);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        colors.a = 1f;
        render.material.color = colors;
    }

}
