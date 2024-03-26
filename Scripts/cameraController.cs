using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject cloud;
    public GameObject grass;
    public float delayTime;
    private Vector3 offset;

    private Vector3 fadeOffset;
    private float lastPos;
    void Start()
    {
        offset = transform.position - player.transform.position;
        lastPos = player.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = player.transform.position + offset;
        Vector3 speed = (target - transform.position) / delayTime;
        transform.position = speed * Time.deltaTime + transform.position;
        if(transform.position.x < 0)
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        if(transform.position.x > 107)  
            transform.position = new Vector3(107, transform.position.y, transform.position.z);
        float move = transform.position.x - lastPos;
        cloud.transform.position = new Vector3(cloud.transform.position.x + move*0.5f, cloud.transform.position.y, cloud.transform.position.z);
        grass.transform.position = new Vector3(grass.transform.position.x + move*0.8f, grass.transform.position.y, grass.transform.position.z);
        lastPos = transform.position.x;
    }
}
