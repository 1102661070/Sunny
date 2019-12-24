using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [SerializeField]//私有的也能显示出来
    private Rigidbody2D rb;
    [SerializeField]
    private Animator anim;

    public Collider2D coll;
    public Collider2D coll2;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public int[] collectScore;
    public GameObject[] item;

    public bool debug = true;
    public bool squatAble = true;
    private float squatNum = 0.00f;
    public Text[] CherryNum;//UI数字
    public bool isHurt;//是否受伤
    public float Collibering = 3;//是否碰撞东西
    private float collNumber = 1.0f;//碰撞东西的减速时间


    void Start()
    {
        //私有时，获取自己的rigidbody2D和Animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移动
        if (!isHurt)
        {
            Movement();
        }
        //动作
        SwitchAnim();
        //蹲的CD
        SquatCd();
        //移速
        moveSpeed();
        //更新UI
        UI();
    }

    void Movement()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.Z)) && coll.IsTouchingLayers(ground))
        {
            if (squatAble)
            {
                //蹲了就站起来，站起来了就蹲，开关型
                squatAble = false;
                if (!anim.GetBool("squat"))
                {
                    anim.SetBool("squat", true);
                    coll2.enabled = false;
                    anim.SetBool("idle", false);
                    anim.SetFloat("running", Mathf.Abs(0));
                }
                else
                {
                    anim.SetBool("squat", false);
                    coll2.enabled = true;
                    anim.SetBool("idle", true);
                }
            }
        }
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        //角色移动
        if (horizontalmove != 0)
        {
            if (horizontalmove > 1 || horizontalmove < -1)
            {
                horizontalmove = 0;
            }
            //移动
            rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);
            //设置动作，将running的数值为绝对值，蹲了就不切换跑步动作
            if (!anim.GetBool("squat"))
            {
                anim.SetFloat("running", Mathf.Abs(horizontalmove));
            }
        }
        #region
        //switch (facedirection)
        //{
        //    case -1:
        //        {
        //            Debug.Log("向左移动");
        //        }
        //        break;
        //    case 0:
        //        {
        //            Debug.Log("不动");
        //        }
        //        break;
        //    case 1:
        //        {
        //            Debug.Log("向右移动");
        //        }
        //        break;
        //}
        #endregion
        //角色转向
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //角色跳跃
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground) && anim.GetBool("jumping") == false && anim.GetBool("idle") == true)
        {
            jump();
        }


    }
    //跳跃
    void jump()
    {

        rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
        //跳跃时取消掉蹲
        if (anim.GetBool("squat"))
        {
            anim.SetBool("squat", false);
            coll2.enabled = true;
        }
        //开启跳跃
        anim.SetBool("falling", false);
        anim.SetBool("jumping", true);
    }
    //判断移速
    void moveSpeed()
    {
        if (anim.GetBool("idle") && anim.GetBool("squat"))
        {
            if (Collibering <= 0.0f)
            {
                speed = 100;
            }
            else
            {
                Collibering -= 0.1f;
                speed = 75;
            }
        }
        else if (anim.GetBool("idle") && !anim.GetBool("squat"))
        {
            if (debug)
            {
                speed = 999;
            }
            else
            {
                speed = 350;
            }
        }


    }

    //切换动画
    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {

            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);

        }
        if (isHurt)
        {
            anim.SetBool("Hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("Hurt", false);
                anim.SetFloat("running", 0);

            }
        }
    }
    //碰撞樱桃
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                jump();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!anim.GetBool("squat"))
            {
                if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-10, rb.velocity.y);
                    isHurt = true;
                    anim.SetBool("Hurt", true);
                }
                else if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(10, rb.velocity.y);
                    isHurt = true;
                    anim.SetBool("Hurt", true);
                }
            }
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("squat"))
            {
                Collibering = collNumber;
            }
        }
    }

    //能否跳跃
    void SquatCd()
    {
        if (!squatAble)
        {
            if (squatNum >= 0.01f)
            {
                squatNum = 0.00f;
                squatAble = true;
                return;
            }
            squatNum += 0.01f;
        }
    }
    //樱桃
    void addscore1()
    {
        collectScore[0] += 1;
    }
    void addscore2()
    {
        collectScore[1] += 1;
    }
    void UI()
    {
        CherryNum[0].text = collectScore[0].ToString();
        CherryNum[1].text = collectScore[1].ToString();
    }
}