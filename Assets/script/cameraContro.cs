using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraContro : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player.position.x < -7)
        { 
        transform.position = new Vector3(-7,0, -10f);
        }
        else
        {

            transform.position = new Vector3(player.position.x, 0, -10f);
        }
    }
}