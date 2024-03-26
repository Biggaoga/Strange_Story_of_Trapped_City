using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Controller : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySkill(){
        player.transform.position += player.GetComponent<playerController>().front * new Vector3(6.0f, 0, 0);
        Destroy(gameObject);
    }
}
