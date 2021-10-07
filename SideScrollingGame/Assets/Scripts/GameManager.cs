using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameProgress {
    title,
    play,
    result
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Blocks;
    [SerializeField] private GameObject player;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text countText;

    [SerializeField] private Button titleButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Text gameover;

    [SerializeField] private Text scoreResult;
    [SerializeField] private Image panel;


    public int looptime{get;set;}
    public int distance{get;set;}
    public gameProgress status{get;set;}
    public float score{get;set;}

    private bool done;
    void Start()
    {
        GenerateStage(10, 10);
        looptime = 0;
        distance = 0;
        status = gameProgress.title;
        StartCoroutine("CountDown");
        titleButton.onClick.AddListener(clickTitle);
        registerButton.onClick.AddListener(clickRegister);
        done = false;
        panel.enabled = false;
        titleButton.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);
        gameover.enabled = false;
        scoreResult.enabled = false;
    }

    void Update()
    {
        if(status == gameProgress.result && !done){
            done = true;
            panel.enabled = true;
            titleButton.gameObject.SetActive(true);
            registerButton.gameObject.SetActive(true);
            gameover.enabled = true;
            scoreResult.enabled = true;
            //playerprefにスコア保存
            PlayerPrefs.SetFloat("Score", score);
            scoreResult.text = score + "m";
        }
        scoreText.text = "Score:" + (looptime * 100 + Mathf.Floor(player.transform.position.x)) + "m";
        score = (looptime * 100 + Mathf.Floor(player.transform.position.x));
    }
    public void LoopStage(){
        if(looptime <= 5){
            GenerateStage(10, 15 - looptime);
        }else{
            GenerateStage(10, 10);
        }
    }
    public void GenerateStage(int max, float range){
        float putPosX = 14.0f;
        int putBloks = 0;
        while(putPosX < 95.0f && putBloks < max){
            int rand = Random.Range(0,3);
            if(rand == 0){
                putPosX += 1.5f;
                GameObject.Instantiate(Blocks[0], new Vector3(putPosX, -1f, 0), Quaternion.identity);
                putPosX += 1.5f;
            }else if(rand == 1){
                putPosX += 1.0f;
                GameObject.Instantiate(Blocks[1], new Vector3(putPosX, 1f, 0f), Quaternion.identity);
                putPosX += 1.0f;
                
            }else{
                putPosX += 3.0f;
                GameObject.Instantiate(Blocks[2], new Vector3(putPosX, -1.5f, 0f), Quaternion.identity);
                putPosX += 3.0f;
            }
            putBloks++;
            putPosX += Random.Range(range - 3, range + 3);
        }
    }    

    public void GenerateBlock(int type, Transform transform){
        GameObject.Instantiate(Blocks[type], transform.position, Quaternion.identity);
    }

    IEnumerator CountDown(){
        countText.text = "3";
        yield return new WaitForSeconds(1);
        countText.text = "2";
        yield return new WaitForSeconds(1);
        countText.text = "1";
        yield return new WaitForSeconds(1);
        countText.text = "";
        status = gameProgress.play;
    }

    public void clickTitle(){
        SceneManager.LoadScene("Title");
    }
    public void clickRegister(){
        SceneManager.LoadScene("Register");
    }
}
