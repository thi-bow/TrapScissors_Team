/*******************************************
制　作　者：影山清晃
最終編集者：影山清晃

最終更新日：2017/04/15

********************************************/
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //プレイヤーの状態
    public enum State
    {
        Wait,
        Touch,
        Endure,
    }
    public State _state;

    [SerializeField, TooltipAttribute("メインカメラ")]
    private GameObject _mainCamera;

    #region 鋏むときに必要な変数
    //鋏むんでいるオブジェクトを入れる
    public GameObject _target;
    //鋏む対象との距離
    private Vector3 _offset = Vector3.zero;
    //鋏んだときの反動中かどうか
    public bool _recoil;
    //離すまでの時間を計測する値
    private float _separateTime;
    [SerializeField, TooltipAttribute("離すまでの時間")]
    private float _separate;
    [TooltipAttribute("耐えてる最中のゲージの値")]
    public float _endureGage;
    [SerializeField, TooltipAttribute("ゲージの上がる値")]
    private float _gageUp;
    [TooltipAttribute("刃こぼれの値")]
    public float _bladeSpill;
    //落ちるまでの時間を計測する値
    private float _fallTime;
    [TooltipAttribute("落ちるまでの時間")]
    public float _fall;


    #endregion

    #region 移動や重力に必要な変数
    //移動する方向ベクトル
    private Vector3 _moveDirection = Vector3.zero;
    //プレイヤーの移動速度
    public float _speed;
    //落ちていく速度を加速させる
    private float _gadd;
    //地面に衝突しているかどうか
    private bool _groundOn;
    [SerializeField]
    private float _clampX, _clampZ;
    #endregion

    // Use this for initialization
    void Start ()
    {
        _recoil = false;
        _state = State.Wait;
        _groundOn = false;
        _separateTime = 0;
        _endureGage = 0;
        _bladeSpill = 4;
        _fallTime = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {       
        Move(3.0f);
        Action();

        //ビルボード
        Vector3 p = _mainCamera.transform.localPosition;
        transform.LookAt(p);
    }

    void Move(float gravity)
    {
        Vector3 newPosition = transform.position;
        
        //何かを挟んでいたら、位置を固定する
        if (_state != State.Wait)
        {
            _gadd = 0;
            newPosition = _target.transform.position + _offset;
        }
        //何も挟んでいないなら、通常移動をする
        if (_state == State.Wait)
        {

            var cameraRight = Vector3.Scale(_mainCamera.transform.right, new Vector3(1, 1, 1)).normalized;
            _moveDirection = (Vector3.forward - Vector3.right) * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal");
            newPosition += _moveDirection * _speed;

            //重力
            //if (_groundOn == false)_gadd += 0.05f;
            //newPosition.y -= gravity * Time.deltaTime * _gadd;
        }
        
        //移動する
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, _clampX, 4.5f),
                                         /*Mathf.Clamp(newPosition.y, -0.5f, 100)*/ newPosition.y,
                                         Mathf.Clamp(newPosition.z, -4.5f, _clampZ));
    }

    //ボタンを使った操作
    void Action()
    {
        //何も鋏んでいないときに挟もうとすると音を鳴らす
        if (_state == State.Wait)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SoundManger.Instance.PlaySE(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                SoundManger.Instance.PlaySE(1);
            }
        }

        //何かを挟んでいるときは離す
        else if (_state == State.Touch)
        {
            //ボタンを離したら、リセットする
            if(Input.GetKeyUp(KeyCode.Z))
            {
                _separateTime = 0;
                return;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                _separateTime += Time.deltaTime;
            }
            //一定時間ボタンを押していたら、離す
            if (_separateTime >= _separate)
            {
                _target = null;
                _state = State.Wait;
                return;
            }
        }

        //相手が暴れているときは耐える
        else if (_state == State.Endure)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _endureGage += _gageUp * _bladeSpill;
                GameObject obj = GameObject.Find("Image");
                if (obj == null)
                {
                    return;
                }
                garge g = obj.GetComponent<garge>();
                if (g == null)
                {
                    return;
                }
                g.SetGauge(_endureGage);
            }
            if (_endureGage >= 100)
            {
                _state = State.Touch;
                _endureGage = 0;
                _fallTime = 0;
                GameObject obj = GameObject.Find("Image");
                if (obj == null)
                {
                    return;
                }
                garge g = obj.GetComponent<garge>();
                if (g == null)
                {
                    return;
                }
                g.SetGauge(0);
            }
            _fallTime += Time.deltaTime;
            if (_fallTime >= _fall)
            {
                _target = null;
                _state = State.Wait;
                _endureGage = 0;
                _fallTime = 0;

            }
        }


    }

    //当たっている最中も取得する当たり判定
    void OnTriggerStay(Collider col)
    {
        //待機状態じゃないと、衝突しても無視する
        if (_state != State.Wait) return; 
        //何かとぶつかっているときにZボタンを押すと大きいハサミで鋏む
        if (Input.GetKeyDown(KeyCode.Z) && _target == null &&
            col.tag != "Ground" && (col.tag == "Gimmick" || col.tag == "Enemy"))
        {
            _target = col.gameObject;
            Trigger();
        }

        if (col.tag == "Ground")
        {
            _gadd = 0;
            _groundOn = true;
        }
    }
    
    //当たり判定が外れたとき
    void OnTriggerExit(Collider col)
    {
        //挟まらずに離れたらターゲットを外す
        if (_state == State.Wait)
        {
            _target = null;
        }
        //地面と離れたら重力が加速するようになる
        if (col.tag == "Ground")
        {
            _groundOn = false;
        }
    }

    public void Trigger()
    {
        _recoil = true;
        _offset = transform.position - _target.transform.position;
        _state = State.Endure;
        StartCoroutine(Recoil());
    }

    //鋏んだら少しの間反動で抜けないようにする
    IEnumerator Recoil()
    {
        yield return new WaitForSeconds(0.2f);
        _recoil = false;
    }

    /// <summary>
    /// 落ちるまでの時間を設定
    /// </summary>
    /// <param name="time">落ちるまでの時間(秒)</param>
    public void SetFall(float time)
    {
        _fall = time;
    }

    /// <summary>
    /// 刃こぼれの値を任意の値にセットする
    /// </summary>
    /// <param name="speed"></param>
    public void SetBladeSpill(float bladeSpill)
    {
        _bladeSpill = bladeSpill;
        //値を1未満、4より大きくしない
        Mathf.Clamp(_bladeSpill, 1, 4);
    }

    /// <summary>
    /// 刃こぼれを回復する
    /// </summary>
    /// <param name="add">回復する値</param>
    public void BladeSpillAdd(float add)
    {
        _bladeSpill += add;
        //値を1未満、4より大きくしない
        Mathf.Clamp(_bladeSpill, 1, 4);
        
    }

    /// <summary>
    /// 刃こぼれをさせる
    /// </summary>
    /// <param name="sub">刃こぼれする値</param>
    public void BladeSpillSub(float sub)
    {
        _bladeSpill -= sub;
        //値を1未満、4より大きくしない
        Mathf.Clamp(_bladeSpill, 1, 4);
    }

    /// <summary>
    /// どのくらい耐えているかの値を返す
    /// </summary>
    /// <returns></returns>
    public float EndureGage()
    {
        return _endureGage;
    }
}
