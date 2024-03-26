using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassBy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    [Header("脚本触发")]
    public GameObject triggerScript;
    [Header("生成僵尸属性")]
    public TextAsset zombieAttribute;
    [Header("是否激活")]
    public bool active;
    [Header("是否采用范围触发")]
    public bool isRangeMethod;
    [Header("是否生成最终boss")]
    public bool isFinalBoss;
    [Header("目标NPC")]
    public GameObject targetNPC;
    [Header("生成僵尸的间隔时间")]
    public float spawnTime;
    [Header("激活的组件")]
    public List<GameObject> activeComponents;
    private List<ZombieAttribute> zombieAttributes;
    private bool isTouched = false;
    private float countTime;
    [Header("解禁玩家的距离")]
    public float playerDistance;
    void Start()
    {
        parseZombieAttribute(zombieAttribute);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        float distance = player.transform.position.x -  transform.position.x;
        // Debug.Log(distance);
        if(isFinalBoss && zombieAttributes.Count > 0){
            GenerateFinalBoss(zombieAttributes);
        }
        else if(isFinalBoss && zombieAttributes.Count == 0){
            return;
        }
        if(isRangeMethod){
            if(Mathf.Abs(distance) < 2){
                isTouched = true;
            }
        }
        else{
            if(distance > 0){
                isTouched = true;
            }
        }
        if(isTouched && active){
            countTime += Time.deltaTime;
            if(countTime >= spawnTime && zombieAttributes.Count > 0){
                Generate();
            }
            else if(zombieAttributes.Count == 0){
                // Destroy(gameObject);
            }
        }

    }
    void Generate(){
        Vector3 playerPosition = transform.position;
        float start = playerPosition.x - 8;
        float end = playerPosition.x - 4;
        GenerateOne(start, end, zombieAttributes[0]);
        zombieAttributes.RemoveAt(0);
        start = playerPosition.x + 4;
        end = playerPosition.x + 8;
        GenerateOne(start, end, zombieAttributes[0]);
        zombieAttributes.RemoveAt(0);
        countTime = 0;
    }
    // 生成最终boss
    void GenerateFinalBoss(List<ZombieAttribute> zombieAttributes){
        Vector3 spawnPosition = transform.position;
        ZombieAttribute zombieAttribute = zombieAttributes[0];
        // int index = Random.Range(0, GameManger.instance.zombies.Count);
        GameObject zombie = Instantiate(GameManger.instance.zombies[zombieAttribute.type], spawnPosition,Quaternion.identity);
        zombie.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        zombie.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + 0.9f, spawnPosition.z);
        // zombie.transform.SetParent(this.gameObject.transform);
        if(triggerScript != null)
            zombie.GetComponent<LastPrefabCheck>().triggerScript = triggerScript;
        if(targetNPC != null)
            zombie.GetComponent<LastPrefabCheck>().targetNPC = targetNPC;
        if(activeComponents.Count > 0){
            foreach(var component in activeComponents){
                zombie.GetComponent<LastPrefabCheck>().activeObjects.Add(component);
            }
        }
        if(playerDistance > 13){
            zombie.GetComponent<LastPrefabCheck>().playerDistance = playerDistance;
        }
        zombie.GetComponent<zombieController>().speed = zombieAttribute.speed;
        zombie.GetComponent<zombieController>().attackSpeed = zombieAttribute.attackSpeed;
        zombie.GetComponent<zombieController>().repelSpeed = zombieAttribute.repelSpeed;
        zombie.GetComponent<zombieController>().attackPoint = zombieAttribute.attackPoint;
        zombie.GetComponent<zombieController>().defensePoint = zombieAttribute.defensePoint;
        zombie.GetComponent<SpriteRenderer>().sortingLayerName = "middle";
        // 获取当前sortingLayer所有GameObject的个数
        int count = GameObject.FindGameObjectsWithTag("zombie").Length;
        // 设置当前生成的僵尸的sortingOrder
        zombie.GetComponent<SpriteRenderer>().sortingOrder = count;
        zombieAttributes.RemoveAt(0);
    }
    // 生成一个僵尸
    void GenerateOne(float start, float end, ZombieAttribute zombieAttribute)
    {
        Vector3 spawnPosition = player.transform.position;
        spawnPosition.x = Random.Range(start, end);
        // int index = Random.Range(0, GameManger.instance.zombies.Count);
        GameObject zombie = Instantiate(GameManger.instance.zombies[zombieAttribute.type], spawnPosition,Quaternion.identity);
        // zombie.transform.SetParent(this.gameObject.transform);
        zombie.transform.position = new Vector3(spawnPosition.x, -2.0f, spawnPosition.z);
        if(triggerScript != null)
            zombie.GetComponent<LastPrefabCheck>().triggerScript = triggerScript;
        if(targetNPC != null)
            zombie.GetComponent<LastPrefabCheck>().targetNPC = targetNPC;
        if(activeComponents.Count > 0){
            foreach(var component in activeComponents){
                zombie.GetComponent<LastPrefabCheck>().activeObjects.Add(component);
            }
        }
        if(playerDistance > 13){
            zombie.GetComponent<LastPrefabCheck>().playerDistance = playerDistance;
        }
        zombie.GetComponent<zombieController>().speed = zombieAttribute.speed;
        zombie.GetComponent<zombieController>().attackSpeed = zombieAttribute.attackSpeed;
        zombie.GetComponent<zombieController>().repelSpeed = zombieAttribute.repelSpeed;
        zombie.GetComponent<zombieController>().attackPoint = zombieAttribute.attackPoint;
        zombie.GetComponent<zombieController>().defensePoint = zombieAttribute.defensePoint;
        zombie.GetComponent<SpriteRenderer>().sortingLayerName = "middle";
        // 获取当前sortingLayer所有GameObject的个数
        int count = GameObject.FindGameObjectsWithTag("zombie").Length;
        // 设置当前生成的僵尸的sortingOrder
        zombie.GetComponent<SpriteRenderer>().sortingOrder = count;

    }
    void parseZombieAttribute(TextAsset zombieAttribute){
        zombieAttributes = new List<ZombieAttribute>();
        string[] lines = zombieAttribute.text.Split('\n');
        // Debug.Log(lines.Length);
        for(int i = 0; i < lines.Length; i+=7){
            int num = int.Parse(lines[i]);
            for(int j=0;j<num;j++){
                int type = int.Parse(lines[i+1]);
                float speed = float.Parse(lines[i+2]);
                float attackSpeed = float.Parse(lines[i+3]);
                float repelSpeed = float.Parse(lines[i+4]);
                float attackPoint = float.Parse(lines[i+5]);
                float defensePoint = float.Parse(lines[i+6]);
                zombieAttributes.Add(new ZombieAttribute(type, speed, attackSpeed, repelSpeed, attackPoint, defensePoint));
            }
        }
    }
}
