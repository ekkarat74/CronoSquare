using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class buffManager : MonoBehaviour
{
    public static buffManager Instance;
    public enum BuffType { fireRateBuf, bulletAdd, null01, increaseLife }
    public static BuffType[] possibleBuffs = { BuffType.fireRateBuf, BuffType.bulletAdd, BuffType.null01, BuffType.increaseLife };

    // อ้างอิงถึง Weapon ที่แนบกับ GameObject ที่ต้องการใช้บัฟ
    public Weapon weapon;

    void Awake()
    {
        Instance = this;
    }

    public void ApplyRandomBuff()
    {
        // ดึงบัฟแบบสุ่ม
        BuffType randomBuff = possibleBuffs[Random.Range(0, possibleBuffs.Length)];

        // ตรวจสอบว่าอ็อบเจ็กต์ Weapon มีอยู่หรือไม่
        if (weapon != null)
        {
            switch (randomBuff)
            {
                case BuffType.fireRateBuf:
                    weapon.increaseFireRate(2f);
                    break;
                case BuffType.bulletAdd:
                    weapon.increaseBulletAmount(1);
                    break;
                case BuffType.null01:
                    weapon.Null01();
                    break;
                case BuffType.increaseLife:
                    weapon.increaseLife();
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Weapon not assigned.");
        }
    }
}