using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHell : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player"){
            other.gameObject.GetComponent<ElectronController>().isCollided = true;
        }
    }
}
