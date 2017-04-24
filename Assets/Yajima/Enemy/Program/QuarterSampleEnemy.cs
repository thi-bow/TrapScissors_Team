using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuarterSampleEnemy : Enemy {

    #region 変数
    public int m_RotateDegree = 180;

    private float m_Deg1 = 60.0f, m_Deg2 = 120.0f, m_WallDeg = 0.0f;
    #endregion

    #region 基盤クラス
    // Use this for initialization
    protected override void Start()
    {
        if ((int)Mathf.Abs(transform.rotation.eulerAngles.y) >= 90)
        {
            m_Deg1 = -120.0f;
            m_Deg2 = -60.0f;
            m_WallDeg = 180.0f;
        }
    }

    //// Update is called once per frame
    //void Update () {

    //}
    #endregion

    #region override関数
    protected override void MoveVelocity()
    {
        transform.position += transform.right * m_Speed * Time.deltaTime;
    }

    protected override void TurnWall()
    {
        // 壁に当たった、崖があった場合は折り返す
        if (m_WChackPoint == null || !m_WChackPoint.IsWallHit()) return;
        //if ((int)rotate.z == 30) degree = 60.0f;
        //if ((int)rotate.z == 150) degree = 120.0f;
        // 角度の設定
        SetDegree();
        // 衝突後の処理
        m_WChackPoint.ChangeDirection();
    }
    #endregion

    #region private関数
    // 角度の設定
    private void SetDegree()
    {
        var wall = m_WChackPoint.GetHitWallObj();
        // 角度の取得
        var degree = m_Deg1;
        var rotate = wall.transform.rotation.eulerAngles;
        if ((int)rotate.z == 150) degree = m_Deg2;
        // テクスチャの回転
        var spriteRotateX = 0.0f;
        var sprite = gameObject.transform.FindChild("EnemySprite");
        var deg = Mathf.Abs(m_WallDeg - (int)Mathf.Abs(rotate.z));
        // 一定角度以上なら、スプライトを180°回転させる
        if (deg >= 90.0f) spriteRotateX = 180;
        sprite.transform.Rotate(new Vector3(spriteRotateX, 0.0f, 0.0f));
        // オブジェクトの回転
        transform.Rotate(transform.forward, degree);
    }
    #endregion

    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(QuarterSampleEnemy), true)]
    [CanEditMultipleObjects]
    public class QuarterEnemyEditor : EnemyEditor
    {
        SerializedProperty RotateDegree;

        protected override void OnChildEnable()
        {
            RotateDegree = serializedObject.FindProperty("m_RotateDegree");
        }

        protected override void OnChildInspectorGUI()
        {
            QuarterSampleEnemy enemy = target as QuarterSampleEnemy;

            // int
            RotateDegree.intValue = EditorGUILayout.IntField("折り返し時の角度(度数法)", enemy.m_RotateDegree);
        }
    }
#endif
    #endregion
}
