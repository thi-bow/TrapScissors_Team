using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneManagerScript : MonoBehaviour
{
    protected static SceneManagerScript Change;
    #region シーン移動時に使うオブジェクト
    [SerializeField]
    private GameObject _fade_Object;
    private RectTransform _fade;
    [SerializeField]
    private GameObject _lord;
    #endregion

    #region フェードの管理
    [SerializeField]
    private bool _scene_Fade;
    //フェードインの時間
    [SerializeField]
    private float _intime;
    //フェードアウトの時間
    [SerializeField]
    private float _outtime;
    //シーン移動開始のフラグ
    private bool FadeStart;
    private EventSystem _eventSystem;
    #endregion

    #region 放置でシーン移行
    [SerializeField]
    private bool _leave_Alone;
    [SerializeField]
    private float _waiting_Time;
    [SerializeField]
    private string _waiting_Scene;
    private float _wTime_Count;
    #endregion

    #region BGMを流す
    [SerializeField]
    private bool _bgmMade;
    [SerializeField]
    private int _BGM_Number;
    [SerializeField]
    private bool _sound_FadeIN;
    [SerializeField]
    private bool _sound_FadeOUT;
    #endregion


    //どこでも参照可
    public static SceneManagerScript sceneManager
    {
        get
        {
            if (Change == null)
            {
                Change = (SceneManagerScript)FindObjectOfType(typeof(SceneManagerScript));
                if (Change == null)
                {
                    Debug.LogError("SceneChange Instance Error");
                }
            }

            return Change;
        }
    }

    void Awake()
    {
        //カーソル非表示
        //Cursor.visible = false;
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();
        _fade = _fade_Object.GetComponent<RectTransform>();
        if (_lord != null)
        {
            _lord.SetActive(false);
        }
        FadeStart = false;

        if (_scene_Fade == true)
        {
            FadeIn();
        }

        //フェード時間が0秒以下なら0.1を入れる
        if (_intime <= 0)  _intime  = 0.1f;
        if (_outtime <= 0) _outtime = 0.1f;
        _wTime_Count = 0;
    }

    void Start()
    {
        if (_bgmMade == true)
        {
            if (_sound_FadeIN == true) SoundManger.Instance.FadeInBGM(_BGM_Number);
            else SoundManger.Instance.PlayBGM(_BGM_Number);
        }
    }

    /// <summary>
    ///フェードイン 
    /// </summary>
    public void FadeIn()
    {
        FadeStart = true;
        //フェードイン中は入力不可にする
        _eventSystem.enabled = false;
        _fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        _fade_Object.SetActive(true);
        LeanTween.alpha(_fade, 0.0f, _intime)
            .setOnComplete(() =>
            {
                _fade_Object.SetActive(false);
                //フェードが終わったFlagを立てる
                FadeStart = false;
                //入力を可能にする
                _eventSystem.enabled = true;
            });
    }

    /// <summary>
    ///フェードアウトによるシーン移動(番号参照) 
    /// </summary>
    /// <param name="number">移動先のシーンの番号</param>
    public void FadeOut(int number)
    {
        if (FadeStart == true)
        {
            return;
        }
        FadeStart = true;
        _fade_Object.SetActive(true);
        _fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //BGMをフェードアウトさせる
        if (_bgmMade == true)
        {
            if (_sound_FadeOUT == true) SoundManger.Instance.FadeOutBGM();
        }
        LeanTween.alpha(_fade, 1, _outtime)
            .setOnComplete(() =>
            {
                SceneOut(number);
            });
    }

    /// <summary>
    ///フェードアウトによるシーン移動(名前参照) 
    /// </summary>
    /// <param name="name">移動先のシーンの名前</param>
    public void FadeOut(string name)
    {
        if (FadeStart == true)
        {
            return;
        }
        FadeStart = true;
        _fade_Object.SetActive(true);
        _fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        if (_bgmMade == true)
        {
            //BGMをフェードアウトさせる
            if (_sound_FadeOUT == true) SoundManger.Instance.FadeOutBGM();
        }
        LeanTween.alpha(_fade, 1, _outtime)
            .setOnComplete(() =>
            {
                SceneOut(name);
            });
    }

    /// <summary>
    /// シーン移動(番号参照)
    /// </summary>
    /// <param name="number">移動先のシーンの番号</param>
    public void SceneOut(int number)
    {
        SceneManager.LoadSceneAsync(number);
        SoundManger.Instance.StopSE();
        //ロード中に動かす画像があったら表示させる
        if (_lord != null)
        {
            _lord.SetActive(true);
        }
    }

    /// <summary>
    /// シーン移動(名前参照)
    /// </summary>
    /// <param name="name">移動先のシーンの名前</param>
    public void SceneOut(string name)
    {
        SceneManager.LoadSceneAsync(name);
        SoundManger.Instance.StopSE();
        //ロード中に動かす画像があったら表示させる
        if (_lord != null)
        {
            _lord.SetActive(true);
        }
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// シーン移動せずに画面全体を薄暗くして時間を止める
    /// </summary>
    public void FadeBlack()
    {
        if (FadeStart == true)
        {
            return;
        }
        _fade_Object.SetActive(true);
        FadeStart = true;
        LeanTween.alpha(_fade, 0.5f, 0.1f)
            .setOnComplete(() =>
            {
                FadeStart = false;
                TimeStop();
            });
    }

    /// <summary>
    /// 暗くなっている画面を明るくして時間を動かす
    /// </summary>
    public void FadeWhite()
    {
        if (FadeStart == true)
        {
            return;
        }

        FadeStart = true;
        TimeStart();
        LeanTween.alpha(_fade, 0, 0.1f)
            .setOnComplete(() =>
            {
                _fade_Object.SetActive(false);
                FadeStart = false;
            });
    }

    /// <summary>
    /// 時間を止める
    /// </summary>
    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// 時間を動かす
    /// </summary>
    public void TimeStart()
    {

        Time.timeScale = 1;
    }

    /// <summary>
    /// 任意の速さにする
    /// </summary>
    /// <param name="speed"></param>
    public void TimeSet(float speed)
    {

        Time.timeScale = speed;
    }

    public void Update()
    {
        //Escキーをおしたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }

        //放置でシーン移動をしないならこの下の処理を行わない
        if (_leave_Alone == false) return;
        _wTime_Count += Time.deltaTime;
        if(_wTime_Count >= _waiting_Time)
        {
            SceneOut(_waiting_Scene);
        }
        if (Input.anyKeyDown) _wTime_Count = 0;
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(SceneManagerScript))]
    public class SceneManagerEditor : Editor
    {
        SerializedProperty fade_Object;
        SerializedProperty lord;
        SerializedProperty Scene_Fade;
        SerializedProperty intime;
        SerializedProperty outtime;
        SerializedProperty leave_Alone;
        SerializedProperty waiting_Time;
        SerializedProperty waiting_Scene;
        SerializedProperty BgmMade;
        SerializedProperty BGM_Number;
        SerializedProperty Fade_IN_Sound;
        SerializedProperty Fade_OUT_Sound;

        public void OnEnable()
        {
            fade_Object = serializedObject.FindProperty("_fade_Object");
            lord = serializedObject.FindProperty("_lord");
            intime = serializedObject.FindProperty("_intime");
            outtime = serializedObject.FindProperty("_outtime");
            leave_Alone = serializedObject.FindProperty("_leave_Alone");
            waiting_Time = serializedObject.FindProperty("_waiting_Time");
            waiting_Scene = serializedObject.FindProperty("_waiting_Scene");
            BgmMade = serializedObject.FindProperty("_bgmMade");
            BGM_Number = serializedObject.FindProperty("_BGM_Number");
            Fade_IN_Sound = serializedObject.FindProperty("_sound_FadeIN");
            Fade_OUT_Sound = serializedObject.FindProperty("_sound_FadeOUT");
            Scene_Fade = serializedObject.FindProperty("_scene_Fade");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SceneManagerScript scene = target as SceneManagerScript;

            fade_Object.objectReferenceValue = EditorGUILayout.ObjectField("フェードさせる画像", scene._fade_Object, typeof(GameObject), true) as GameObject;
            lord.objectReferenceValue = EditorGUILayout.ObjectField("ロード中に出す画像", scene._lord, typeof(GameObject), true) as GameObject;

            EditorGUILayout.Space();
            Scene_Fade.boolValue = EditorGUILayout.Toggle("フェードインをさせる", scene._scene_Fade);
            EditorGUILayout.LabelField("フェード時間( IN / OUT )");
            EditorGUILayout.BeginHorizontal();
            intime.floatValue = EditorGUILayout.FloatField(scene._intime, GUILayout.Width(32));
            outtime.floatValue = EditorGUILayout.FloatField(scene._outtime, GUILayout.Width(32));
            EditorGUILayout.EndHorizontal();

            leave_Alone.boolValue = EditorGUILayout.Toggle("放置したらシーン移動", scene._leave_Alone);
            if(scene._leave_Alone == true)
            {
                waiting_Time.floatValue = EditorGUILayout.FloatField("放置時間", scene._waiting_Time);
                waiting_Scene.stringValue = EditorGUILayout.TextField("移動するシーン", scene._waiting_Scene);
            }

            EditorGUILayout.Space();
            BgmMade.boolValue = EditorGUILayout.Toggle("BGMを流す", scene._bgmMade);
            if (scene._bgmMade == true)
            {
                BGM_Number.intValue = EditorGUILayout.IntField("BGM番号", scene._BGM_Number);
                Fade_IN_Sound.boolValue = EditorGUILayout.Toggle("BGMフェードイン", scene._sound_FadeIN);
                Fade_OUT_Sound.boolValue = EditorGUILayout.Toggle("BGMフェードアウト", scene._sound_FadeOUT);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
