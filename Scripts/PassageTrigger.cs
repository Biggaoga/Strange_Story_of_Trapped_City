using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject pressR;
    public GameObject read;
    [Header("开启后是否禁用R")]
    public bool disableR;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        // Debug.Log(distance);
        if(distance < 2.0f && !pressR.activeSelf){
            pressR.SetActive(true);
        }
        else if(distance >= 2.0f && pressR.activeSelf){
            pressR.SetActive(false);
        }
        if(disableR){
            if(distance < 2.0f && Input.GetKeyDown(KeyCode.R)){
                read.SetActive(true);
                GameManger.instance.isStart = false;
            }
        }
        else{
            if(distance < 2.0f && Input.GetKeyDown(KeyCode.R)){
            read.SetActive(read.activeSelf ? false : true);
            GameManger.instance.isStart = read.activeSelf ? false : true;
        }
        }

    }
}
