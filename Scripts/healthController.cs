using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float healthPoint;
    public float fade_health_point;
    public GameObject healthBar;
    public float growPerSecond = 0.1f;

    void Start()
    {
    }
    
    void Awake()
    {
        healthPoint = 100;
        fade_health_point = 100;
    }   

    // Update is called once per frame
    void Update()
    {
        // transform.position = player.transform.position + offset;
        // Debug.Log(fade_health_point);
        if(GameManger.instance.GetIsStart() == false)
            return;
        if(fade_health_point < 100)
            fade_health_point += growPerSecond * Time.deltaTime;
        else    
            fade_health_point = 100;
        if(healthPoint != fade_health_point){
            if(healthPoint > fade_health_point){
                healthPoint -= 0.1f;
            }
            else{
                healthPoint = fade_health_point;
            }
        }
        // Debug.Log(healthPoint);
        healthBar.transform.localScale = new Vector3(healthPoint / 100.0f, 1, 1);
    }

    void attack(){
        fade_health_point -= 10;
    }
}
