using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDController : MonoBehaviour
{
    public float CD;
    private float speed;

    private Slider slider;
    void Awake()
    {
        speed = 1.0f / CD;
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    void Start()
    {
        slider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        if(slider.value < 1)
            slider.value += speed * Time.deltaTime;
        else 
            slider.value = 1;
    }

    public void OnEnable(){
        slider.value = 0;
    }
}
