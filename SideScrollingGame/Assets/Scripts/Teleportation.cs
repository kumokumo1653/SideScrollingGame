using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private new GameObject camera;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {

            other.transform.position = new Vector2(start.position.x, other.transform.position.y);
            InheritBlocks(DeleteBlocks());
            gameManager.GetComponent<GameManager>().GenerateStage(30);
            gameManager.GetComponent<GameManager>().looptime++;
        }
    }

    public GameObject[] DeleteBlocks(){
        GameObject[] array = new GameObject[]{};
        Collider2D[] Inheritcolliders = Physics2D.OverlapBoxAll(camera.transform.position, new Vector2(20f, 50f), 0f);
        foreach(Collider2D coll in Inheritcolliders){
            if(coll.gameObject.tag == "Obstacle"){
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = coll.gameObject; 
            }
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(50f, 0f), new Vector2 (140f, 50f), 0f);
        foreach(Collider2D coll in colliders){
            if(coll.gameObject.tag == "Obstacle"){
                Destroy(coll.gameObject);
            }
        }
        return array;
    }

    public void InheritBlocks(GameObject[] objs){
        foreach(GameObject obj  in objs){
            GameObject.Instantiate(obj, new Vector3(obj.gameObject.transform.position.x - 100f, obj.gameObject.transform.position.y, 0f), Quaternion.identity);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(50f, 0f), new Vector2 (100f, 50f));
    }
}


