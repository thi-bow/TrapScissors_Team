using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class garge : MonoBehaviour {

    private Player player;

    public Image g_bar;

	// Use this for initialization
	void Start () {
        //Player取得
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); 
        this.initParmeter();
        
       
	}
	
	// Update is called once per frame
	void Update () {
    
	}

    private void initParmeter()
    {
        //g_bar = gameObject.Find("a").GetComponent<Image>();

        g_bar.fillAmount = 100;
    }
    
}
