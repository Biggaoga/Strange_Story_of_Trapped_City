using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ZombieAttribute{
    public int type;
    public float speed;
    public float attackSpeed;
    public float repelSpeed;
    public float attackPoint;
    public float defensePoint;
    public ZombieAttribute(int type, float speed, float attackSpeed, float repelSpeed, float attackPoint, float defensePoint)
    {
        this.type = type;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.repelSpeed = repelSpeed;
        this.attackPoint = attackPoint;
        this.defensePoint = defensePoint;
    }
}

public class GameManger : MonoBehaviour
{
    // Start is called before the first frame update
    static public GameManger instance;
    public GameObject PropertyPanel;
    public GameObject GameStartPanel;
    public GameObject HealthBar;
    public bool isStart;
    [Header("退出")]
    public GameObject Exit;
    public Text finalScore;
    [Header("通关的条件")]
    public string answer;
    public Text answerText;
    public GameObject success;
    [Header("打开密令日记")]
    public GameObject diary;
    public Text diaryText;
    public string diaryPassword;
    [Header("解开墓碑的密码")]
    public string tombPassword;
    public Text tombText;
    public List<GameObject> secrets;
    public GameObject confirmButton;
    public GameObject inputField;
    public List<GameObject> triggerActive;
    [Header("破译成功后反派女主出现")]
    public GameObject daugther;
    public GameObject talkScript;
    [Header("输入通关密码失败后显示书")]
    public GameObject book;
    [Header("生成的僵尸种类")]
    public List<GameObject> zombies;
    [Header("玩家最远能到达的位置")]
    public float maxDistance;
    private bool isShake;
    [Header("分数")]
    public Text scoreText;
    public int score;
    [Header("溅血")]
    public GameObject blood;
    private void Awake()
    {
        if(instance!=null)
            Destroy(gameObject);
        instance = this;
        isStart = true;
    }
    public void judgePass()
    {
        if(answer == answerText.text)
        {
            success.SetActive(true);
            isStart = false;
        }
        else
        {
            book.SetActive(true);
            isStart = true;
        }
    }
    public void openDiary()
    {
        if(diaryPassword == diaryText.text)
        {
            diary.SetActive(true);
            isStart = false;
        }
        else{
            isStart = true;
        }
    }
    public void openTomb()
    {
        if(tombPassword == tombText.text)
        {
            foreach(var secret in secrets)
            {
                secret.SetActive(true);
            }
            confirmButton.SetActive(false);
            inputField.SetActive(false);
            foreach(var trigger in triggerActive)
            {
                trigger.SetActive(true);
            }
        }
    }
    public void closeTomb(){
        if(triggerActive.Count > 0 && triggerActive[0].activeSelf)
        {
            daugther.SetActive(true);
            talkScript.SetActive(true);
        }
        else{
            isStart = true;
        }

    }
    public void addScore(int score)
    {
        this.score += score;
        scoreText.text = "得分： " + this.score;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart == false)
            return;
        if(Input.GetKeyDown(KeyCode.E)){
            instance.PropertyPanel.SetActive(!instance.PropertyPanel.activeSelf);
            // isStart = !isStart;
        }
        if(HealthBar.GetComponent<healthController>().healthPoint <= 0){
            instance.Exit.SetActive(true);
            finalScore.text = "得分： " + score;
            isStart = false;
        }
    }

    public bool GetIsStart()
    {
        return isStart;
    }
    public void continueGame()
    {
        isStart = true;
        // Debug.Log(isStart);
    }
    public void StopGame()
    {
        isStart = false;
    }

    public void StartGame()
    {
        isStart = true;
        // GameStartPanel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1f;
    }
    public void CameraShake(float duration, float strength)
    {
        if(!isShake)
            StartCoroutine(Shake(duration, strength));
    }
    IEnumerator Shake(float duration, float strength){
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPostion = camera.position;
        while(duration > 0){
            camera.position = startPostion + Random.insideUnitSphere * strength;
            duration -= Time.deltaTime;
            yield return null;
        }
        isShake = false;
    }
}
