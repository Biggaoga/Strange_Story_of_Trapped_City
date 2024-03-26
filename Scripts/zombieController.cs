using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zombieController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    Animator animator;
    private GameObject healthBar;
    [Header("初始属性")]
    public string type;
    public float attackPoint;
    public float defensePoint;
    public float speed;
    public float attackSpeed;
    public float repelSpeed;
    private GameObject ZombieHealthBar;
    [Header("显示属性UI组件")]
    public Text Type;
    public Text AttackPoint;
    public Text DefensePoint;
    [Header("是否翻转")]
    public int isFlip;
    [Header("受击音效")]
    public AudioClip hitSound;
    public AudioSource hitAudioSource;
    private float fadeHealthPoint;
    private bool isDead;
    GameObject skill;
    void Start()
    {
        animator = GetComponent<Animator>();
        fadeHealthPoint = 100;
        isDead = false;
        player = GameObject.Find("Character");
        healthBar = GameObject.Find("HealthBar");
        ZombieHealthBar = GameObject.Find("Enemy");
        Type = GameObject.Find("Type").GetComponent<Text>();
        AttackPoint = GameObject.Find("AttackPoint").GetComponent<Text>();
        DefensePoint = GameObject.Find("DefensePoint").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        if(!isDead){
            float distance = Vector3.Distance(player.transform.position, transform.position);
            findPlayer(distance);
            detectSkillls(distance);
        }
    }

    void move(float speed){
        if(player.transform.position.x < transform.position.x - 0.3f){
            transform.position += new Vector3(-0.05f, 0, 0) * speed * Time.deltaTime;
            Vector3 scale = transform.localScale;
            scale.x = isFlip * Math.Abs(scale.x);
            transform.localScale = scale;
        }
        else if(player.transform.position.x > transform.position.x + 0.3f){
            transform.position += new Vector3(0.05f, 0, 0) * speed * Time.deltaTime;
            Vector3 scale = transform.localScale;
            scale.x = -isFlip * Math.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void Dead(){
        GameManger.instance.addScore(1);
        Destroy(gameObject);
    }

    public void reduceHealthPoint(float attack){
        fadeHealthPoint -= (attack-defensePoint);
        animator.SetBool("isSlashed", true);
        if(fadeHealthPoint <= 0){
            isDead = true;
            animator.SetBool("isDead", true);
            ZombieHealthBar.GetComponent<healthController>().fade_health_point = 0;
        }
        else{
            ZombieHealthBar.GetComponent<healthController>().fade_health_point = fadeHealthPoint;
            Type.text = type;
            AttackPoint.text = "攻击力: "+attackPoint.ToString();
            DefensePoint.text = "防御力: "+defensePoint.ToString();
        }
    }

    void detectSkillls(float distance){
        if(distance < 6.0f && player.GetComponent<playerController>().createFierce && animator.GetBool("isSlashed") == false && skill == null){
            // Debug.Log(playerAnimator.GetBool("Skill1"));
            Vector3 pos = transform.position;
            pos.y += 1.0f;
            skill = Instantiate(player.GetComponent<playerController>().skills[0], pos, Quaternion.identity);
            // 出血
            Vector3 pos1 = transform.position;
            pos.x += 0.5f * player.GetComponent<playerController>().front;
            GameObject blood = Instantiate(GameManger.instance.blood, pos1, Quaternion.identity);
            Destroy(blood, 2.0f);
            
            reduceHealthPoint(50.0f);
            if(hitAudioSource != null)
                hitAudioSource.PlayOneShot(hitSound);
        }
    }

    void findPlayer(float distance){
        animator.SetFloat("distance", distance);
        if(animator.GetBool("isSlashed") == true){
            move(repelSpeed);
        }
        else if(distance < 12.0f && distance > 1.0f){
            // Debug.Log("Player is near");
            // animator.SetBool("isSlashed", false);
            move(speed);
        }
        else if(distance <= 1 && player.GetComponent<playerController>().missing == false){
            // Debug.Log("Player is close");
            // animator.SetBool("isSlashed", false);
            healthBar.GetComponent<healthController>().fade_health_point -= (attackPoint - player.GetComponent<playerController>().defensePoint);
            move(attackSpeed);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "NPC"){
            // Debug.Log("NPC");
            // animator.SetFloat("distance", 0);
            Destroy(other.gameObject);
            GameObject newZombie = Instantiate(GameManger.instance.zombies[0], transform.position, Quaternion.identity);
            // newZombie.transform.SetParent(this.gameObject.transform);
            // 获取当前sortingLayer所有GameObject的个数
            int count = GameObject.FindGameObjectsWithTag("zombie").Length;
            // 设置当前生成的僵尸的sortingOrder
            newZombie.GetComponent<SpriteRenderer>().sortingOrder = count;
        }
    }

    void StopBeSlashed(){
        // DeBug.Log("Reset");
        // Debug.Log("Reset");
        animator.SetBool("isSlashed", false);
    }
}
