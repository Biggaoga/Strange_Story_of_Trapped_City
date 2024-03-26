using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera;
    public float width;
    public int mapNums;
    private float totalWidth;
    void Start()
    {
        Camera = GameObject.Find("Main Camera");
        width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        totalWidth = width * mapNums;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos  = transform.position;
        if(Camera.transform.position.x > transform.position.x + totalWidth / 2.0f){
            pos.x += totalWidth;
            transform.position = pos;
        }
        else if(Camera.transform.position.x < transform.position.x - totalWidth / 2.0f){
            pos.x -= totalWidth;
            transform.position = pos;
        }
    }
}
