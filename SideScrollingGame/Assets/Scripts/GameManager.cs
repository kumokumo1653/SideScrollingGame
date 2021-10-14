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
    [SerializeField] private GameObject Box;
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
        GenerateStage(10, 10, 100);
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
        if(false && looptime <= 5){
            GenerateStage(10, 15 - looptime, 7 - looptime );
        }else{
            GenerateStage(10, 10, 2);
        }
    }
    public void GenerateStage(int max, float range, int flag){
        float putPosX = 14.0f;
        int putBloks = 0;
        while(putPosX < 95.0f && putBloks < max){
            int rand = Random.Range(0,3);
            GameObject temp;
            if(rand == 0){
                putPosX += 1.5f;
                temp = GameObject.Instantiate(Blocks[0], new Vector3(putPosX, -1f, 0), Quaternion.identity);
                putPosX += 1.5f;
            }else if(rand == 1){
                putPosX += 1.0f;
                temp = GameObject.Instantiate(Blocks[1], new Vector3(putPosX, 1f, 0f), Quaternion.identity);
                putPosX += 1.0f;
                
            }else{
                putPosX += 3.0f;
                temp = GameObject.Instantiate(Blocks[2], new Vector3(putPosX, -1.5f, 0f), Quaternion.identity);
                putPosX += 3.0f;
            }
            if(Random.Range(0, flag) == 0){
                if(rand == 0 || rand == 2){
                    GameObject.Instantiate(Box, new Vector3(temp.transform.position.x + Random.Range(5f, 5f), temp.transform.position.y + temp.transform.localScale.y + Random.Range(3f, 5f), 0f), Quaternion.identity);
                }else{
                    GameObject.Instantiate(Box, new Vector3(temp.transform.position.x, temp.transform.position.y + temp.transform.localScale.y + Random.Range(6f, 8f), 0f), Quaternion.identity);
                }
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
