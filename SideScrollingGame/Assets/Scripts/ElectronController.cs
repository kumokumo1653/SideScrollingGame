using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronController : MonoBehaviour
{

    private Rigidbody2D rigid;
    public bool isGrounded{get;set;}
    [SerializeField]private float moveSpeed;
    [SerializeField]private float jumpForce;

    [SerializeField]private float rightForce;
    [SerializeField]private float smoothTime;

    [SerializeField]private Transform groundCheck;
    [SerializeField]private float groundedRadius;
    [SerializeField]private LayerMask stageMask;
    [SerializeField]private int maxAirJump;
    [SerializeField]private GameManager gameManager;
    private Vector3 currentVelocity = Vector3.zero;
    private int AirJumpTime = 0;

    private int latencyTime = 10;
    private int currentTime = 0;

    private Animator animator;
    private PolygonCollider2D characterCollider;
    private Sprite sprite;
    [SerializeField] private Sprite[] sprites;

    private Vector2[][] characters = new Vector2[][]{
        //まっすぐ
        new Vector2[]{
            new Vector2(0.339541554f,0.0287427902f),
            new Vector2(0.146663606f,0.0118576214f),
            new Vector2(0.142070472f,0.245219484f),
            new Vector2(0f,0.421875f),
            new Vector2(-0.09375f,0.5f),
            new Vector2(-0.203125f,0.5f),
            new Vector2(-0.21875f,0.484375f),
            new Vector2(-0.234375f,0.421875f),
            new Vector2(-0.234375f,-0.265625f),
            new Vector2(-0.140625f,-0.5f),
            new Vector2(0.180383682f,-0.495737076f),
            new Vector2(0.16660732f,-0.391494334f),
            new Vector2(0.101736009f,-0.334518701f),
            new Vector2(0.139027894f,-0.250591308f),
            new Vector2(0.309646487f,-0.214469969f),
            new Vector2(0.362211227f,-0.143444136f),

        },
        //大きく開いてる方
        new Vector2[]{
            new Vector2(0.339541554f,0.0287427902f),
            new Vector2(0.146663606f,0.0118576214f),
            new Vector2(0.142070472f,0.245219484f),
            new Vector2(0f,0.421875f),
            new Vector2(-0.09375f,0.5f),
            new Vector2(-0.203125f,0.5f),
            new Vector2(-0.21875f,0.484375f),
            new Vector2(-0.234375f,0.421875f),
            new Vector2(-0.352318347f,-0.245634586f),
            new Vector2(-0.368515491f,-0.5f),
            new Vector2(0.335424066f,-0.498807192f),
            new Vector2(0.335463226f,-0.38688916f),
            new Vector2(0.180023789f,-0.31149289f),
            new Vector2(0.139027894f,-0.250591308f),
            new Vector2(0.309646487f,-0.214469969f),
            new Vector2(0.362211227f,-0.143444136f),

        },
        //小さく開いてる方
        new Vector2[]{
            new Vector2(0.339541554f,0.0287427902f),
            new Vector2(0.146663606f,0.0118576214f),
            new Vector2(0.142070472f,0.245219484f),
            new Vector2(0f,0.421875f),
            new Vector2(-0.09375f,0.5f),
            new Vector2(-0.203125f,0.5f),
            new Vector2(-0.21875f,0.484375f),
            new Vector2(-0.234375f,0.421875f),
            new Vector2(-0.295031905f,-0.348750204f),
            new Vector2(-0.249422967f,-0.5f),
            new Vector2(0.22715807f,-0.5f),
            new Vector2(0.233383834f,-0.38688916f),
            new Vector2(0.133624077f,-0.362532526f),
            new Vector2(0.103454769f,-0.255231261f),
            new Vector2(0.309646487f,-0.214469969f),
            new Vector2(0.362211227f,-0.143444136f),

        },
        //しゃがみ
        new Vector2[]{
            new Vector2(0.339541554f,0.0287427902f),
            new Vector2(0.146663606f,0.0118576214f),
            new Vector2(0.244110167f,0.227212444f),
            new Vector2(0.0807715654f,0.5f),
            new Vector2(-0.0338227153f,0.5f),
            new Vector2(-0.07284832f,0.377539843f),
            new Vector2(-0.151006103f,0.231638074f),
            new Vector2(-0.218140125f,0.084190011f),
            new Vector2(-0.295031905f,-0.348750204f),
            new Vector2(-0.249422967f,-0.5f),
            new Vector2(0.0521555543f,-0.5f),
            new Vector2(0.0278762579f,-0.367622823f),
            new Vector2(0.0790362358f,-0.312761158f),
            new Vector2(0.103454769f,-0.255231261f),
            new Vector2(0.309646487f,-0.214469969f),
            new Vector2(0.362211227f,-0.143444136f),

        },
        //ジャンプ
        new Vector2[]{
            new Vector2(0.367946625f,0.372759938f),
            new Vector2(0.171912611f,0.381123781f),
            new Vector2(0.0799919963f,0.410267472f),
            new Vector2(-0.124376237f,0.5f),
            new Vector2(-0.327595413f,0.5f),
            new Vector2(-0.305749238f,0.39606607f),
            new Vector2(-0.235697329f,0.231638074f),
            new Vector2(-0.249701381f,0.0620971918f),
            new Vector2(-0.351842046f,-0.323501229f),
            new Vector2(-0.353574932f,-0.5f),
            new Vector2(-0.156148374f,-0.50310117f),
            new Vector2(-0.0857440233f,-0.367622823f),
            new Vector2(0.0916606188f,-0.183360234f),
            new Vector2(0.128703773f,0.063536942f),
            new Vector2(0.249680281f,0.129547238f),
            new Vector2(0.37483573f,0.165855721f),
        },
    };
    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        characterCollider = this.gameObject.GetComponent<PolygonCollider2D>();
        
    }

    void FixedUpdate()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        if(sprite == sprites[0]){
            //まっすぐ
            characterCollider.points = characters[0];
        }else if(sprite == sprites[1] || sprite == sprites[2]){
            //大きい方
            characterCollider.points = characters[1];
        }else if(sprite == sprites[3] || sprite == sprites[4]){
            //小さい方
            characterCollider.points = characters[2];
        }else if(sprite == sprites[5]){
            //しゃがみ
            characterCollider.points = characters[3];
        }else if(sprite == sprites[6]){
            //ジャンプ
            characterCollider.points = characters[4];
        }
        //地面にあたっているか判定
        if(isGrounded == false){
            currentTime++;
        }
        if(isGrounded || currentTime >= latencyTime){
            currentTime = 0;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, stageMask);
            for(int i = 0; i < colliders.Length; i++){
                if(colliders[i].gameObject != this.gameObject){
                    isGrounded = true;
                    animator.SetBool("ground",true);
                    animator.SetBool("jump", false);
                }
            }

            //空中ジャンプ判定
            if(isGrounded){
                AirJumpTime = 0;
            } 

        }




    }

    public void Move(float move, bool jump){
        Vector2 targetVelocity = new Vector2(move + rightForce, rigid.velocity.y); 
        rigid.velocity = targetVelocity;//Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref currentVelocity, smoothTime);
        if(jump && AirJumpTime < maxAirJump){
            animator.SetBool("jump", true);
            rigid.AddForce(new Vector2(0f, jumpForce));
            AirJumpTime++;
            isGrounded = false;
            animator.SetBool("ground",false);
        }
        if(Mathf.Abs(move + rightForce) < float.Epsilon * 10){
           animator.SetBool("run", false); 
        }else{
            animator.SetBool("run", true);
        }
    }
    void Update()
    {

        if(gameManager.status == gameProgress.play){
            float movement = Input.GetAxis("Horizontal") * moveSpeed;
            bool jump = Input.GetButtonDown("Jump");
            Move(movement, jump);
        }

    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.tag == "Obstacle"){
            Debug.Log("collision");
            gameManager.status = gameProgress.result;
           animator.SetBool("run", false); 
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
    }


}
