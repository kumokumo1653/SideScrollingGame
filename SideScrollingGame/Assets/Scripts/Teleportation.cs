using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private Transform start;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player") {
            other.transform.position = new Vector2(start.position.x, other.transform.position.y);
        }
    }
}
