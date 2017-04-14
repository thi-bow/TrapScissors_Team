using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {


   public float  i_speed;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1.0f, 1.0f, 0.01f)*i_speed);
	}

    public void OnTriggerEnter2D(Collision2D collision)
    {
        var _target = collision.gameObject;
        if (_target.tag == "Player")
        {
            Destroy(gameObject);
        }

    }
}
