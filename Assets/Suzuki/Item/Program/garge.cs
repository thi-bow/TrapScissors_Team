using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class garge : MonoBehaviour
{
    // private Player _p;
    // private float _gage;
    private Image g_bar; //ゲージ
    private RectTransform _rect;
    private Image i_gauge;
    public GameObject _player;

    // Use this for initialization
    void Start()
    {
        //_p = _player.GetComponent<Player>();
        g_bar = GameObject.Find("Image").GetComponent<Image>();
        _rect = GameObject.Find("Image").GetComponent<RectTransform>();
        _rect.localScale = new Vector3(4.0f, 0.5f, 1.0f);
        g_bar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //g_bar.fillAmount = _p.EndureGage()/100;
        // LeanTween.value(_player, prevgeag, _p.EndureGage() / 100, 1f).setOnUpdate((float val)=>{ g_bar.fillAmount = val; }).setEase(LeanTweenType.easeInBack);

        //  prevgeag = _p.EndureGage() / 1000;
        //_gage = _player.GetComponent<Player>().EndureGage() / 100;

        //_gage++;
        //_gage += _p.EndureGage() / 1000;

        //g_bar.fillAmount = _player.GetComponent<Player>().EndureGage() / 100;

        //g_bar.fillAmount += _p.EndureGage() / 1000;
        //g_bar.fillAmount= Mathf.Clamp(g_bar.fillAmount, 0, _p.EndureGage() / 100);
        

    }
    public void SetGauge(float gauge)
    {
        float minGauge = g_bar.fillAmount;
        float maxGauge = gauge;
        LeanTween.value(_player, minGauge, maxGauge / 100, 1f).setOnUpdate((float val) => { g_bar.fillAmount = val; }).setEase(LeanTweenType.easeOutQuad);
    }
}