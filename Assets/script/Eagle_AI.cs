using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Eagle_AI : MonoBehaviour
{

    //传入目标
    public Transform target;

    //速度
    public float speed = 200f;

    //保存于下一个路径点的距离
    public float nextWaypointDistance = 3f;

    //当前路径
    Path path;
    //代表在当前路径上，正在向那个路标点移动
    int currentWaypoint = 0;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
