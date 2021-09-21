using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronController : MonoBehaviour
{

    private Rigidbody2D rigid;
    public bool isGrounded{get;set;}
    public bool isCollided{get;set;}
    [SerializeField]private float moveSpeed;
    [SerializeField]private float jumpForce;

    [SerializeField]private float rightForce;
    [SerializeField]private float smoothTime;

    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundedRadius;
    [SerializeField]private Transform collisionCheck;
    [SerializeField]private Vector2 collisionSize;
    [SerializeField]private LayerMask stageMask;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //地面にあたっているか判定
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, stageMask);
        for(int i = 0; i < colliders.Length; i++){
            if(colliders[i].gameObject != this.gameObject){
                isGrounded = true;
            }
        }


        //衝突判定
        isCollided = false;
        colliders = Physics2D.OverlapBoxAll(collisionCheck.position, collisionSize, stageMask);
        for(int i = 0; i < colliders.Length; i++){
            if(colliders[i].gameObject != this.gameObject){
                isCollided = true;
            }
        }
    }

    public void Move(float move, bool jump){
        Vector2 targetVelocity = new Vector2(move + rightForce, rigid.velocity.y); 
        rigid.velocity = targetVelocity;//Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref currentVelocity, smoothTime);
        if(isGrounded && jump){
            rigid.AddForce(new Vector2(0f, jumpForce));
        }
    }
    void Update()
    {
        float movement = Input.GetAxis("Horizontal") * moveSpeed;
        bool jump = Input.GetButtonDown("Jump");
        Move(movement, jump);
        if(isCollided) Debug.Log("collision!!!");
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
        Gizmos.DrawWireCube(collisionCheck.position, collisionSize);
    }
}
