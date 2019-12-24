using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class bird : MonoBehaviour
{
    public AIPath aiPath;
    public bool isFly = false;
    public GameObject fa;
    public float speed;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Fix();
        Move();

    }
    
    void Fix()
    {

        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void Move()
    {
        if (!isFly)
        {
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                fa.transform.position = new Vector2(fa.transform.position.x + speed * Time.deltaTime, fa.transform.position.y);

            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                fa.transform.position = new Vector2(fa.transform.position.x - speed * Time.deltaTime, fa.transform.position.y);
            }
        }
    }
}
