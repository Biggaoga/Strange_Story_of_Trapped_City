using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float attackPoint;
    public float defensePoint;
    Animator animator;
    public int Slash;
    public List<GameObject> skills;


    public GameObject skill1CD;
    public bool createFierce;
    public bool missing;
    public GameObject skill2CD;
    public GameObject skill3CD;
    public GameObject gameManager;
    private GameObject skill;
    public int front = 1;
    [Header("技能")]
    private int comboStep;
    public float interval;
    private float timer;
    private bool isAttack;
    private bool skill1;
    private bool skill2;
    private bool skill3;
    [Header("状态条")]
    public GameObject healthBar;
    public GameObject blueBar;
    [Header("打击感")]
    public float shakeTime;
    public float shakeStrength;
    public int pauseFrame;
    [Header("是否关卡阻止")]
    public bool isStop;
    [Header("跑步音效")]
    public AudioClip run;
    [Header("闪现音效")]
    public AudioClip flashSound;
    public AudioSource flashAudioSource;
    [Header("剑气音效")]
    public AudioClip skill3Sound;
    public AudioSource skill3AudioSource;
    [Header("挥砍音效")]
    public AudioClip slashSound;
    public AudioSource slashAudioSource;
    public AudioClip slashSound2;
    public AudioSource slashAudioSource2;
    [Header("命中音效")]
    public AudioClip hitSound;
    public AudioSource hitAudioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        Slash = 0;
        createFierce = false;
        missing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.GetComponent<GameManger>().GetIsStart()){
            // Debug.Log("Stop");
            animator.SetBool("isRunning", false);
            return;
        } 
        if(!isAttack && !skill1 && !skill2&& !skill3) {
            move();
        }
        slash();
        if(animator.GetBool("isRunning") && !GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().clip = run;
            GetComponent<AudioSource>().Play();
        }
        else if(!animator.GetBool("isRunning") && GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Stop();
        }
    }

    void move(){
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.position += movement * speed * Time.deltaTime;
        if(transform.position.x < -7)
            transform.position = new Vector3(-7, transform.position.y, transform.position.z);
        if(isStop){
            if(transform.position.x > GameManger.instance.maxDistance)
            transform.position = new Vector3(GameManger.instance.maxDistance, transform.position.y, transform.position.z);
        }
        else{
            if(transform.position.x > 107)
                transform.position = new Vector3(107, transform.position.y, transform.position.z);
        }

        // Debug.Log(moveHorizontal);
        if(moveHorizontal != 0){
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(moveHorizontal, 1, 1);
            front = (int)moveHorizontal;
        }
        else{
            animator.SetBool("isRunning", false);
        }
    }

    void slash(){
        if(!isAttack && !skill1 && !skill2 && !skill3){
            if(Input.GetKeyDown(KeyCode.J)){
                isAttack = true;
                comboStep++;
                timer = interval;
                if(comboStep > 2){
                    comboStep = 1;
                }
                animator.SetTrigger("LightAttack");
                animator.SetInteger("ComboStep", comboStep);
                if(comboStep == 1){
                    slashAudioSource.PlayOneShot(slashSound);
                }
                else{
                    slashAudioSource2.PlayOneShot(slashSound2);
                }

            }
            else if(Input.GetKeyDown(KeyCode.L) && blueBar.GetComponent<BluePointController>().fade_blue_point >= 10 && skill1CD.GetComponent<Slider>().value == 1.0f){
                // animator.SetBool("Skill1", true);
                animator.SetTrigger("S1");
                skill1 = true;
                blueBar.GetComponent<BluePointController>().fade_blue_point -= 10;
                skill1CD.GetComponent<CDController>().OnEnable();
                missing = true;
                // Debug.Log("Skill1");
            }
            else if(Input.GetKeyDown(KeyCode.K)&& blueBar.GetComponent<BluePointController>().fade_blue_point >= 10 && skill3CD.GetComponent<Slider>().value == 1.0f){
                animator.SetTrigger("S3");
                skill3 = true;
                blueBar.GetComponent<BluePointController>().fade_blue_point -= 10;
                skill3CD.GetComponent<CDController>().OnEnable();
                Vector3 position = transform.position;
                position.x += (-1.7f* front);
                GameObject s3 = Instantiate(skills[2], position, Quaternion.identity);
                s3.transform.localScale = new Vector3(front, 1, 1);
                s3.GetComponent<Skill3Controller>().speed *= front;
                s3.GetComponent<Skill3Controller>().acceleration *= front; 
                skill3AudioSource.PlayOneShot(skill3Sound);
            }
            else if(Input.GetKeyDown(KeyCode.Space)&&blueBar.GetComponent<BluePointController>().fade_blue_point >= 10 && skill2CD.GetComponent<Slider>().value == 1.0f){
                // animator.SetBool("Skill2", true);
                animator.SetTrigger("S2");
                skill2 = true;
                transform.localScale = new Vector3(front, 1, 1);
                blueBar.GetComponent<BluePointController>().fade_blue_point -= 10;
                skill2CD.GetComponent<CDController>().OnEnable();
                missing = true;
                flashAudioSource.PlayOneShot(flashSound);
            }
            if(timer != 0){
                timer -= Time.deltaTime;
                if(timer < 0){
                    timer = 0;
                    comboStep = 0;
                }
            }

        }

    }
    public void reduceHealthPoint(float attack){
        healthBar.GetComponent<healthController>().fade_health_point -= (attack-defensePoint);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "zombie" && isAttack){
            GameManger.instance.HitPause(pauseFrame);
            GameManger.instance.CameraShake(shakeTime, shakeStrength);
            other.GetComponent<zombieController>().reduceHealthPoint(attackPoint);
            Vector3 pos = other.transform.position;
            pos.x += 0.5f * front;
            GameObject blood = Instantiate(GameManger.instance.blood, pos, Quaternion.identity);
            // 2秒后删除
            Destroy(blood, 2.0f);
            hitAudioSource.PlayOneShot(hitSound);
            if(other.GetComponent<zombieController>().hitAudioSource != null)
                other.GetComponent<zombieController>().hitAudioSource.PlayOneShot(other.GetComponent<zombieController>().hitSound);
        }
    }
    public void OverAttack(){
        isAttack = false;
    }
    // 创建刀光特效
    void Fierce(){
        createFierce = true;
    }

    void createThunder(){
        Vector3 position = transform.position;
        position.y += 1.0f;
        position.x += 3.0f * front;
        skill = Instantiate(skills[1], position, Quaternion.identity);
        skill.transform.localScale = new Vector3(front,1,1);
    }

    void stopSkill1(){
        // animator.SetBool("Skill1", false);
        skill1 = false;
        missing = false;
    }

    void stopSkill2(){
        // Debug.Log("StopSkill2");
        // animator.SetBool("Skill2", false);
        skill2 = false;
        missing = false;
    }
    void stopSkill3(){
        skill3 = false;
    }
}
