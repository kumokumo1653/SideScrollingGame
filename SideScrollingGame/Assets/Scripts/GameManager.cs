using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int looptime{get;set;}
    public int distance{get;set;}
    void Start()
    {
        GenerateStage(30);
        looptime = 0;
        distance = 0;
    }

    void Update()
    {
        scoreText.text = "Score:" + (looptime * 100 + Mathf.Floor(player.transform.position.x)) + "m";
    }
    public void GenerateStage(int max){
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
            putPosX += Random.Range(5f, 10f);
        }
    }    

    public void GenerateBlock(int type, Transform transform){
        GameObject.Instantiate(Blocks[type], transform.position, Quaternion.identity);
    }
}
