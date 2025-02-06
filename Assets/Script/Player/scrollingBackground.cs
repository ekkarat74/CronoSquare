using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // ความเร็วในการเลื่อน
    private Vector2 startOffset; // ค่าเริ่มต้นของ texture offset
    private Material material; // Material ของพื้นหลัง

    void Start()
    {
        // ดึง Material จาก Renderer ของ GameObject
        material = GetComponent<Renderer>().material;
        // เก็บค่า texture offset เริ่มต้น
        startOffset = material.mainTextureOffset;
    }

    void Update()
    {
        // คำนวณค่า offset ใหม่ตามเวลาและความเร็ว
        float offset = Time.time * scrollSpeed;
        // ปรับค่า offset ในแกน Y เพื่อเลื่อนลง
        material.mainTextureOffset = new Vector2(startOffset.x, offset);
    }

    void OnDestroy()
    {
        // รีเซ็ตค่า texture offset เมื่อ GameObject ถูกทำลาย
        material.mainTextureOffset = startOffset;
    }
}