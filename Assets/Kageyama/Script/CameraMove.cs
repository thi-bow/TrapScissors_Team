using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 offset = Vector3.zero;
    public bool _lerpfrag;

    void Start()
    {
        _lerpfrag = true;
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = player.transform.position.x + offset.x;
        newPosition.y = player.transform.position.y + offset.y;
        newPosition.z = player.transform.position.z + offset.z;
        //ピッタリと追いかける
        if (_lerpfrag == false) transform.position = newPosition;
        //スムーズに追いかける
        else transform.position = Vector3.Lerp(transform.position, newPosition, 5.0f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update () {
        //Move();
	}

    void Move()
    {
        //this.transform.Rotate(0, (Input.GetAxis("Camera") * 1), 0);
    }

    /// <summary>
    /// スムーズに追いかけるときはtrue、ピッタリくっつくときはfalseにする
    /// </summary>
    /// <param name="frag">スムーズにするかどうか</param>
    public void LerpFragChenge(bool frag)
    {
        _lerpfrag = frag;
    }
}
