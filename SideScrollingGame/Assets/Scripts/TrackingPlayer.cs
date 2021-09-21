using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private Vector3 offset;
    void Start()
    {
        offset = this.gameObject.transform.position - player.transform.position; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.gameObject.transform.position = player.transform.position + offset;
    }
}
