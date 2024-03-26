using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    // public float hitPoint;
    void Start()
    {
        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySkill(){
        Destroy(gameObject);
    }
    void DestroySkill1(){
        player.GetComponent<playerController>().createFierce = false;
        Destroy(gameObject);
    }
    // void OnTriggerEnter2D(Collider2D other){
    //     if(other.CompareTag("zombie")){
    //         other.GetComponent<zombieController>().reduceHealthPoint(hitPoint);
    //     }
    // }
}
