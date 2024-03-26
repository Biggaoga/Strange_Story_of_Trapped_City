using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPrefabCheck : MonoBehaviour
{
    private static int prefabCount = 0; // 静态计数器，记录实例个数
    public GameObject triggerScript;
    public GameObject targetNPC;
    public List<GameObject> activeObjects = new List<GameObject>();

    public float playerDistance;
    void Awake()
    {
        prefabCount++;
    }

    void OnDestroy()
    {
        prefabCount--;
        CheckIfLastPrefab();
    }

    void CheckIfLastPrefab()
    {
        if (prefabCount == 0)
        {
            if(triggerScript != null){               
                GameManger.instance.isStart = false;
                triggerScript.SetActive(true);
            }
            if(targetNPC != null)
                targetNPC.GetComponent<AutoMessageTrigger>().visitTimes++;
            if(activeObjects.Count > 0)
                foreach(var obj in activeObjects)
                    obj.SetActive(true);
            if(playerDistance > 13)
                GameManger.instance.maxDistance = playerDistance;
        }
    }
}
