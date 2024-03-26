using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;
    [Header("控制游戏暂停")]
    public GameObject gameManager;
    [Header("文本文件")]
    public TextAsset textFile;
    [Header("头像")]
    public Sprite face01, face02;
    [Header("调用刷怪笼")]
    public GameObject trigger;
    [Header("激活的组件")]
    public List<GameObject> activeComponents;
    [Header("取消激活的组件")]
    public List<GameObject> deactiveComponents;
    private int index;
    List<string> textList = new List<string>();
    bool textFinished;
    void Awake()
    {
        GetTextFromFile(textFile);
    }
    void OnEnable(){
        // textLabel.text = textList[index];
        // index++;
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) && index == textList.Count){
            gameObject.SetActive(false);
            index = 0;
            if(trigger != null)
                trigger.GetComponent<PassBy>().active = true;
            // gameManager.GetComponent<GameManger>().continueGame();
            GameManger.instance.isStart = true;
            if(activeComponents.Count > 0){
                foreach(var component in activeComponents){
                    component.SetActive(true);
                }
            }
            if(deactiveComponents.Count > 0){
                foreach(var component in deactiveComponents){
                    component.SetActive(false);
                }
            }
            return;
        }
        else if(Input.GetKeyDown(KeyCode.S) && textFinished){
            // textLabel.text = textList[index];
            // index++;
            StartCoroutine(SetTextUI());
        }
    }

    void GetTextFromFile(TextAsset file){
        textList.Clear();
        var lineData = file.text.Split('\n');
        foreach(var line in lineData){
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI(){
        textFinished = false;
        textLabel.text = "";
        switch(textList[index]){
            case "A":
                faceImage.sprite = face01;
                scale(faceImage, 200);
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                scale(faceImage, 200);
                index++;
                break;
        }
        for(int i = 0; i < textList[index].Length; i++){
            textLabel.text += textList[index][i];
            yield return new WaitForSeconds(0.05f);
        }
        index++;
        textFinished = true;
    }

    void scale(Image imageComponent, float height){
        float originalWidth = imageComponent.sprite.texture.width;
        float originalHeight = imageComponent.sprite.texture.height;

        // 计算缩放比例
        float scaleRatio = height / originalHeight;

        // 应用缩放
        imageComponent.rectTransform.sizeDelta = new Vector2(originalWidth* scaleRatio, height);
    }
}
