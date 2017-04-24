using UnityEngine;
using System.Collections;

public class SampleEnemy3D : Enemy
{
    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public void OnTriggerEnter(Collider other)
    {
        OnCollidePlayer(other);
    }

    public void OnTriggerStay(Collider other)
    {
        OnCollidePlayer(other);
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
}
