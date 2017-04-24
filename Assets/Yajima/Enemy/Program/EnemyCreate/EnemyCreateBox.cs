using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyCreateBox : MonoBehaviour {

    [SerializeField]
    private int m_CreateCount = 3;          // 生成数
    [SerializeField]
    private float m_CreateTime = 2.0f;      // 生成間隔
    [SerializeField]
    private float m_CreateLength = 10.0f;   // 生成する距離
    [SerializeField]
    private GameObject m_CreateEnemy;       // 生成するオブジェクト
    [SerializeField]
    private bool m_IsStartCreate;           // ゲーム開始時に生成するか

    private float m_Timer = 0.0f;           // 経過時間

    //private Enemy3D m_Enemy;            // 敵のスクリプト

    // Use this for initialization
    void Start () {
        if (!m_IsStartCreate) return;
        // 敵の生成
        CreateEnemy();

        //if (m_CreateEnemy == null) return;
        //m_Enemy = m_CreateEnemy.GetComponent<Enemy3D>();
	}
	
	// Update is called once per frame
	void Update () {
        //var player = GameObject.Find("Player");
        //if (player == null) return;

        //var length = Vector3.Distance(
        //    this.transform.position, player.transform.position
        //    );
        // 一定距離に達しなかった、一定数生成した場合は返す
        //if (length > m_CreateLength ||
        //    this.transform.childCount > m_CreateCount) return;

        m_Timer += Time.deltaTime;

        // 条件を満たしたら、生成
        if (m_Timer < m_CreateTime ||
            this.transform.childCount > m_CreateCount) return;
        // 敵の生成
        CreateEnemy();
        m_Timer = 0.0f;
        //enemy.transform.parent = this.transform;
    }

    // 敵の生成
    private void CreateEnemy()
    {
        var enemy = m_CreateEnemy;
        Instantiate(
            enemy, this.transform.position,
            this.transform.rotation, this.transform
            );
    }

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyCreateBox), true)]
    [CanEditMultipleObjects]
    public class EnemyCreateBoxEditor : Editor
    {
        SerializedProperty CreateCount;
        SerializedProperty CreateTime;
        SerializedProperty CreateLength;
        SerializedProperty CreateEnemy;
        SerializedProperty IsStartCreate;

        public void OnEnable()
        {
            CreateCount = serializedObject.FindProperty("m_CreateCount");
            CreateTime = serializedObject.FindProperty("m_CreateTime");
            CreateLength = serializedObject.FindProperty("m_CreateLength");
            CreateEnemy = serializedObject.FindProperty("m_CreateEnemy");
            IsStartCreate = serializedObject.FindProperty("m_IsStartCreate");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            // 必ず書く
            serializedObject.Update();

            EnemyCreateBox createBox = target as EnemyCreateBox;

            // int
            CreateCount.intValue = EditorGUILayout.IntField("敵の最大生成数", createBox.m_CreateCount);

            // float
            CreateTime.floatValue = EditorGUILayout.FloatField("敵の生成間隔(秒)", createBox.m_CreateTime);
            CreateLength.floatValue = EditorGUILayout.FloatField("敵の生成距離(m)", createBox.m_CreateLength);
            // bool
            IsStartCreate.boolValue = EditorGUILayout.Toggle("ゲーム開始時に生成", createBox.m_IsStartCreate);

            EditorGUILayout.Space();
            // Transform
            CreateEnemy.objectReferenceValue = EditorGUILayout.ObjectField("生成する敵の種類", createBox.m_CreateEnemy, typeof(GameObject), true);
            //EditorGUILayout.Space();
            //// WallChackPoint
            //WChackPoint.objectReferenceValue = EditorGUILayout.ObjectField("壁捜索ポイント", enemy.m_WChackPoint, typeof(WallChackPoint), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
