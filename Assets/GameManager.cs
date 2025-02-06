using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score;
    public int kill;
    public TMP_Text scoreText;
    public TMP_Text killText;
    public TMP_Text finalWaveText;
    public WaveController waveController;
    public static int buffTreashold = 5;

    [SerializeField] private float comboDuration;
    [SerializeField] private float comboTime = 0f;
    [SerializeField] private int comboMultiplier = 1;

    // เพิ่มตัวแปรอ้างอิงถึง buffManager
    public buffManager buffManagerInstance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void FixedUpdate()
    {
        if (comboTime > 0)
        {
            comboTime -= Time.deltaTime;
            if (comboTime <= 0)
            {
                ResetCombo();
            }
        }
        killText.text = "Combo: " + kill.ToString();
        CheckForBuff();
    }

    void ResetCombo()
    {
        kill = 0;
        comboMultiplier = 1;
        comboTime = 0f;
    }

    public void UpdateScore(int points)
    {
        int truePoints = points * comboMultiplier;
        score += truePoints;
        scoreText.text = "Score: " + score;
    }

    public void ComboKill(int kills)
    {
        kill += kills;
        comboMultiplier = kill;
        comboTime = comboDuration;
        killText.text = "Combo: " + kill;
    }

    public void OnFinalWaveReached()
    {
        StartCoroutine(ShowFinalWaveTextAndSpawn());
    }

    private IEnumerator ShowFinalWaveTextAndSpawn()
    {
        finalWaveText.gameObject.SetActive(true);
        finalWaveText.text = "Warning!";
        yield return new WaitForSeconds(3f);
        finalWaveText.gameObject.SetActive(false);
    }

    public void CheckForBuff()
    {
        if(kill % buffTreashold == 0 && kill > 0)
        {
            // ตรวจสอบว่ามีอินสแตนซ์ของ buffManager หรือไม่
            if (buffManagerInstance != null)
            {
                buffManagerInstance.ApplyRandomBuff();
            }
            else
            {
                Debug.LogWarning("buffManagerInstance is not assigned!");
            }
        }
    }
}
