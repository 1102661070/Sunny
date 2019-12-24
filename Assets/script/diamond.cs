using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diamond : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("addscore2");
    }
}
