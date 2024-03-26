using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePointController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BlueBar;
    public float bluePoint;
    public float fade_blue_point;
    public float growPerSecond = 0.1f;
    void Start()
    {
        bluePoint = 100;
        fade_blue_point = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        if(fade_blue_point < 100)
            fade_blue_point += growPerSecond * Time.deltaTime;
        else 
            fade_blue_point = 100;
        if(bluePoint != fade_blue_point){
            if(bluePoint > fade_blue_point){
                bluePoint -= 0.1f;
            }
            else{
                bluePoint = fade_blue_point;
            }
        }
        // Debug.Log(healthPoint);
        BlueBar.transform.localScale = new Vector3(bluePoint / 100.0f, 1, 1);
    }
}
