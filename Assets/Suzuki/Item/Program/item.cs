using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class item : MonoBehaviour
{
    
    private Player _p;
    private GameObject _player;
    public int _item;
    // Use this for initialization
    void Start()
    {
       // _p = _player.GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1.0f, 1.0f));
        //transform.Translate(0f, 0.004f, 0.02f);
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {

        var _target = collider.gameObject;
        if (_target.tag == "Player")
        {
            DontDestroyOnLoad(this.gameObject);
            Destroy(gameObject);

            //_p.BladeSpillAdd(1);
           

        }
       
    }

}
