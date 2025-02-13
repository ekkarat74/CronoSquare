using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : UnitStats
{
    public delegate void EnemyDestroyed(); // ประกาศ delegate สำหรับ event
    public event EnemyDestroyed OnEnemyDestroyed; // ประกาศ event

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DecreaseHealthE(int amount)
    {
        CurHP -= amount;

        if (CurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed.Invoke(); // เรียก event
        }

        gameManager.UpdateScore(50);
        gameManager.ComboKill(1);
        Destroy(gameObject);
    }
}