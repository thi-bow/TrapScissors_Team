using UnityEngine;
using System.Collections;

public class WallChackPoint : MonoBehaviour
{
    private bool m_IsWallHit = false;   // 壁に衝突したか
    private GameObject m_HitWallObj;        // 当たったオブジェクト

    //private Vector2 m_Direction = Vector2.right;

    // Use this for initialization
    //void Start() {
    //    var parent = gameObject.GetComponentInParent<Transform>();
    //    // 親の方向を取得
    //    if (parent != null)
    //        m_Direction = parent.right;
    //}

    // Update is called once per frame
    void Update() { }

    // 壁に衝突したか
    public bool IsWallHit() { return m_IsWallHit; }

    // 衝突した壁を取得します
    public GameObject GetHitWallObj() { return m_HitWallObj; }

    // 方向を変えます
    public void ChangeDirection()
    {
        // m_Direction *= -1;
        //Vector2 dir = Vector2.one * -1;
        //ChangeDirection(dir.x, dir.y);
        //var pos = gameObject.transform.localPosition;
        //var newPos = new Vector3(
        //    pos.x * dir.x,
        //    pos.y * dir.y,
        //    0.0f
        //    );
        //gameObject.transform.localPosition = newPos;
        m_IsWallHit = false;
    }

    public void ChangeDirection(float right, float up)
    {
        var pos = gameObject.transform.localPosition;
        var newPos = new Vector3(
            pos.x * right,
            pos.y * up,
            0.0f
            );
        gameObject.transform.localPosition = newPos;
        m_IsWallHit = false;
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //if (collision.gameObject.tag != "Wall" && 
    //    //    collision.gameObject.name != "Wall") return;
    //    if (collision.gameObject.tag != "Ground") return;
    //    m_IsWallHit = true;
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Ground") return;
        m_IsWallHit = true;
        m_HitWallObj = collision.gameObject;
    }
}
