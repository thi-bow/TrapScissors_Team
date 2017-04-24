using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RabbitEnemy : Enemy3D {

    [SerializeField]
    private float m_TurnLength = 1.0f;

    private float m_MoveLength = 0.0f;
    // Use this for initialization
    //protected override void Start()
    //{
    //    base.Start();
    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void Move(float deltaTime, float subSpeed = 1.0f)
    {
        base.Move(deltaTime, subSpeed);
        // 移動距離の加算
        m_MoveLength += Mathf.Abs(m_TotalVelocity.x) + Mathf.Abs(m_TotalVelocity.y) + Mathf.Abs(m_TotalVelocity.z);
    }

    protected override void TurnWall()
    {
        // 一定距離移動したら、折り返す
        if (m_MoveLength < m_TurnLength * 10) return;

        m_MoveLength = 0.0f;
        //base.TurnWall();
        // 角度の設定
        SetDegree();
        // 衝突後の処理
        m_WChackPoint.ChangeDirection();
    }
    #region シリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(RabbitEnemy), true)]
    [CanEditMultipleObjects]
    public class RabbitEditor : Enemy3DEditor
    {
        SerializedProperty TurnLength;

        protected override void OnChildEnable()
        {
            TurnLength = serializedObject.FindProperty("m_TurnLength");
        }

        protected override void OnChildInspectorGUI()
        {
            RabbitEnemy enemy = target as RabbitEnemy;

            // int
            TurnLength.floatValue = EditorGUILayout.FloatField("折り返す距離", enemy.m_TurnLength);
        }
    }
#endif
    #endregion
}
