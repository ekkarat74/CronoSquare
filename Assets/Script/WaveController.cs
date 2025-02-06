using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;
    
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab; // Prefab ของศัตรู
        public Transform[] spawnPoints; // Array สำหรับกำหนดจุด spawn ที่ใช้สำหรับ prefab นี้
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyGroup[] enemyGroups; // Array ของกลุ่มศัตรูในคลื่นนี้
        public int enemyCountPerSpawnPoint; // จำนวนศัตรูต่อจุด spawn ในแต่ละกลุ่ม
        public float spawnInterval; // เวลาระหว่างการ spawn ของแต่ละศัตรู
    }

    public Wave[] waves; // Array สำหรับกำหนดหลายคลื่นของศัตรู
    public float timeBetweenWaves = 5f; // เวลาที่จะรอก่อนเริ่มคลื่นถัดไป
    public float waitTimeForFinalPrefab = 5f; // เวลาที่จะรอก่อน spawn finalPrefab

    public int currentWaveIndex = 0; // ตัวแปรเก็บคลื่นปัจจุบัน
    private int enemiesRemaining; // ตัวแปรเก็บจำนวนศัตรูที่ยังมีอยู่ในฉาก
    public GameObject finalPrefab; // Prefab ที่จะ spawn หลังจากศัตรูทั้งหมดถูกทำลาย
    public Transform[] finalSpawnPoints; // จุด spawn ของ prefab สุดท้าย

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(StartWaves()); // เริ่ม coroutine สำหรับจัดการการ spawn ของคลื่น
    }

    IEnumerator StartWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            enemiesRemaining = 0; // เริ่มต้นจำนวนศัตรูเป็น 0
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex])); // เรียกใช้คลื่นปัจจุบัน
            currentWaveIndex++;

            // รอเวลาที่กำหนดก่อนเริ่มคลื่นถัดไป
            yield return new WaitForSeconds(timeBetweenWaves); 
        }

        // เมื่อ spawn คลื่นสุดท้ายแล้ว ตรวจสอบว่าศัตรูทั้งหมดถูกทำลายหรือไม่
        yield return StartCoroutine(CheckForEnemiesDestroyed());
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemyGroup group in wave.enemyGroups)
        {
            for (int i = 0; i < wave.enemyCountPerSpawnPoint; i++)
            {
                foreach (Transform spawnPoint in group.spawnPoints)
                {
                    SpawnEnemy(group.enemyPrefab, spawnPoint); // spawn ศัตรูที่จุด spawn ที่กำหนดในกลุ่มนั้นๆ
                    enemiesRemaining++; // เพิ่มจำนวนศัตรูในตัวแปร enemiesRemaining
                }
                yield return new WaitForSeconds(wave.spawnInterval); // รอเวลาที่กำหนดก่อน spawn ตัวต่อไป
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation); // สร้างศัตรูที่จุด spawn ที่กำหนด
        enemy.GetComponent<EnemyStats>().OnEnemyDestroyed += HandleEnemyDestroyed; // เชื่อมต่อกับ event เมื่อศัตรูถูกทำลาย
    }

    void HandleEnemyDestroyed()
    {
        enemiesRemaining--; // ลดจำนวนศัตรูลงเมื่อศัตรูถูกทำลาย
    }

    IEnumerator CheckForEnemiesDestroyed()
    {
        while (enemiesRemaining > 0) // รอจนกว่าศัตรูทั้งหมดจะถูกทำลาย
        {
            yield return null; // รอในแต่ละ frame
        }

        // รอเวลาที่กำหนดใน waitTimeForFinalPrefab ก่อนที่จะ spawn finalPrefab
        yield return new WaitForSeconds(waitTimeForFinalPrefab); 

        // Spawn finalPrefab จากจุดที่กำหนด
        foreach (Transform spawnPoint in finalSpawnPoints)
        {
            Instantiate(finalPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
