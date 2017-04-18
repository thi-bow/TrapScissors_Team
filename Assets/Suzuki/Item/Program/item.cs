using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class item : MonoBehaviour {
    public Image _speed;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1.0f, 1.0f, 0.01f));
        transform.Translate(0.01f, 0.004f, 0.02f); 

	}

    public void OnTriggerEnter2D(Collider2D collider)
    {
        
        var _target = collider.gameObject;
        if (_target.tag == "Player")
        {
            Destroy(gameObject);
        }

    }
}
