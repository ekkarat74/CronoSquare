using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitStats : MonoBehaviour
{
    public static UnitStats instance;

    [SerializeField] private int live = 3;
    [SerializeField] private Image[] liveUI;

    [SerializeField] public float curHP;
    public float CurHP { get { return curHP; } set { curHP = value; } }

    [SerializeField] private float maxHP ;
    public float MaxHP { get { return maxHP; } }

    private bool isGameOver;
    [SerializeField] GameObject gameoverui;

    public TMP_Text HP;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        curHP = maxHP;
    }

    public void DecreaseHealth(float amount)
    {
        curHP -= amount;
        HP.text = "Health : " + curHP;

        if (curHP <= 0)
        {
            Continue();
        }
    }

    private void Continue()
    {
        if (live > 0)
        {
            live--;

            liveUI[live].enabled = false;
            
            gameoverui.SetActive(false);
            curHP = maxHP;
            Time.timeScale = 1;
        }
        else
        {
            isGameOver = true;
            gameoverui.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
