using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMessageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public int visitTimes = 0;
    public GameObject player;
    [Header("激活对话列表")]
    public List<GameObject> dialogList;
    [Header("提示标志")]
    public GameObject pressR;
    public float distance;
    [Header("NPC在玩家走远后是否自动消失")]
    public bool autoDisappear = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        // Debug.Log(transform.position.x);
        distance = Vector3.Distance(player.transform.position, transform.position);
        if(autoDisappear && player.transform.position.x - transform.position.x > 30.0f){
            Destroy(gameObject);
        }
        if(distance < 0.8f && !pressR.activeSelf){
            pressR.SetActive(true);
        }
        else if(distance >= 0.8f && pressR.activeSelf){
            pressR.SetActive(false);
        }
        if(distance < 0.8f && Input.GetKeyDown(KeyCode.R) && !judgeActivate()){
            if(visitTimes >= dialogList.Count){
                return;
            }
            dialogList[visitTimes].SetActive(true);
            GameManger.instance.isStart = false;
            // gameManager.GetComponent<GameManger>().StopGame();
        }
    }

    bool judgeActivate(){
        foreach(var dialog in dialogList){
            if(dialog.activeSelf){
                return true;
            }
        }
        return false;   
    }

}
