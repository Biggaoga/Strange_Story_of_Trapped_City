using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("生成的僵尸种类")]
    public List<GameObject> zombies;
    public GameObject player;
    public float spawnTime;
    public float remainTime;
    [Header("触发的脚本")]
    public List<GameObject> triggerScripts;
    [Header("生成僵尸属性")]
    // public List<ZombieAttribute> zombieAttributes;
    public float zombieSpeed;
    public float zombieAttackSpeed;
    public float zombieRepelSpeed;
    public float zombieAttackPoint;
    public float zombieDefensePoint;
    private float countTime; 
    public int waveTimes; 
    void Start()
    {
        waveTimes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        if(remainTime < 0){
            remainTime = 0;
            countTime = 0;
            waveTimes++;
            return;
        }
        else if(remainTime == 0)
        {
            return;
        }
        remainTime -= Time.deltaTime;
        countTime += Time.deltaTime;
        if(countTime >= spawnTime)
        {
            Vector3 playerPosition = player.transform.position;
            float start = playerPosition.x - 8;
            float end = playerPosition.x - 4;
            SpawnPlatform(start, end, zombieSpeed, zombieAttackSpeed, zombieRepelSpeed, zombieAttackPoint, zombieDefensePoint);
            start = playerPosition.x + 4;
            end = playerPosition.x + 8;
            SpawnPlatform(start, end, zombieSpeed, zombieAttackSpeed, zombieRepelSpeed, zombieAttackPoint, zombieDefensePoint);
            countTime = 0;
        }
    }
    public void SpawnPlatform(float start, float end, float zombieSpeed, float zombieAttackSpeed, float zombieRepelSpeed, float zombieAttackPoint, float zombieDefensePoint)
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x = Random.Range(start, end);
        int index = Random.Range(0, zombies.Count);
        GameObject zombie = Instantiate(zombies[index], spawnPosition,Quaternion.identity);
        zombie.transform.SetParent(this.gameObject.transform);
        zombie.GetComponent<LastPrefabCheck>().triggerScript = triggerScripts[waveTimes];
        zombie.GetComponent<zombieController>().speed = zombieSpeed;
        zombie.GetComponent<zombieController>().attackSpeed = zombieAttackSpeed;
        zombie.GetComponent<zombieController>().repelSpeed = zombieRepelSpeed;
        zombie.GetComponent<zombieController>().attackPoint = zombieAttackPoint;
        zombie.GetComponent<zombieController>().defensePoint = zombieDefensePoint;
        zombie.GetComponent<SpriteRenderer>().sortingLayerName = "middle";
        // 获取当前sortingLayer所有GameObject的个数
        int count = GameObject.FindGameObjectsWithTag("zombie").Length;
        // 设置当前生成的僵尸的sortingOrder
        zombie.GetComponent<SpriteRenderer>().sortingOrder = count;

    }
    // 根据未来生成的僵尸数来确定时间
    public void SetRemainTime(int zombieCount)
    {
        remainTime = zombieCount * spawnTime;
    }
}
