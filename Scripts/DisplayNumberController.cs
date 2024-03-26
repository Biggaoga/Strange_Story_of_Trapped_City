using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNumberController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HealthBar;
    public GameObject BlueBar;
    public GameObject player;
    public Text healthText;
    public Text blueText;
    public Text attackPoint;
    public Text defensePoint;
    public Text speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.instance.GetIsStart() == false)
            return;
        float number = HealthBar.GetComponent<healthController>().growPerSecond;
        healthText.text = number.ToString();
        number = BlueBar.GetComponent<BluePointController>().growPerSecond;
        blueText.text = number.ToString();
        number = player.GetComponent<playerController>().attackPoint;
        attackPoint.text = number.ToString();
        number = player.GetComponent<playerController>().defensePoint;
        defensePoint.text = number.ToString();
        number = player.GetComponent<playerController>().speed;
        speed.text = number.ToString();
    }
}
