using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    [Header("技能速度")]
    public float speed;
    [Header("技能加速度")]
    public float acceleration;
    [Header("技能伤害")]
    public float hitPoint;  
    [Header("技能持续时间")]
    public float duration;
    [Header("打击感")]
    public float shakeTime;
    public float shakeStrength;
    public int pauseFrame;
    public SpriteRenderer spriteRenderer;
    [Header("技能方向")]
    public GameObject player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        rb.velocity = new Vector2(speed, 0);
        speed += acceleration * Time.deltaTime;
        if(speed < 1.0f){
            Color color = spriteRenderer.color;
            color.a -= speed * Time.deltaTime;
            spriteRenderer.color = color;
        }
        Destroy(gameObject, duration);
    }
    void OnTriggerEnter2D(Collider2D other){
        // Debug.Log(other.tag);

        if(other.CompareTag("zombie")){
            GameManger.instance.HitPause(pauseFrame);
            GameManger.instance.CameraShake(shakeTime, shakeStrength);
            other.GetComponent<zombieController>().reduceHealthPoint(hitPoint);
            // 出血
            Vector3 pos = other.transform.position;
            pos.x += 0.5f * player.GetComponent<playerController>().front;
            GameObject blood = Instantiate(GameManger.instance.blood, pos, Quaternion.identity);
            // 2秒后删除
            Destroy(blood, 2.0f);
            if(other.GetComponent<zombieController>().hitAudioSource != null)
                other.GetComponent<zombieController>().hitAudioSource.PlayOneShot(other.GetComponent<zombieController>().hitSound);
        }
    }
}
