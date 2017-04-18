using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class garge : MonoBehaviour {

    private Player player;
    public Image g_bar; //ゲージ
    private RectTransform hoge;

        // Use this for initialization
	void Start () {
        #region  ゲージのスケール情報
        hoge = GameObject.Find("Image").GetComponent<RectTransform>();
        hoge.localScale = new Vector3(3, 0.5f, 1);
        #endregion
        
        //player = GameObject.Find("Player").GetComponent<Player>();//Player取得
        g_bar = GameObject.Find("Image").GetComponent<Image>();              
	}
    float _gage = 1.0f;
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            _gage -= 0.01f;

        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _gage = 1f;
        }
        if (_gage < 0.0f)
        {
            _gage = 1.0f;
        }
        g_bar.fillAmount= _gage; 
           
	}
}
