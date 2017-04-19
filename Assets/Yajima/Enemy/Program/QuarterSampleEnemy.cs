using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuarterSampleEnemy : Enemy {

    public int m_RotateDegree = 180;

	// Use this for initialization
	//void Start () {
	
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    protected override void MoveVelocity()
    {
        transform.position += transform.right * m_Speed * Time.deltaTime;
    }

    protected override void TurnWall()
    {
        // 壁に当たった、崖があった場合は折り返す
        if (m_WChackPoint == null || !m_WChackPoint.IsWallHit()) return;

        var wall = m_WChackPoint.GetHitWallObj();

        //if ((int)rotate.z == 30) degree = 60.0f;
        //if ((int)rotate.z == 150) degree = 120.0f;
        // 角度の取得
        var degree = 60.0f;
        var rotate = wall.transform.rotation.eulerAngles;
        if ((int)rotate.z == 150) degree = 120.0f;
        // テクスチャの回転
        var spriteRotateX = 0.0f;
        var sprite = gameObject.transform.FindChild("EnemySprite");
        if (Mathf.Abs(rotate.z) >= 90.0f)
            spriteRotateX = 180.0f;
        sprite.transform.Rotate(new Vector3(spriteRotateX, 0.0f, 0.0f));
        // オブジェクトの回転
        transform.Rotate(transform.forward, degree);
        // 衝突後の処理
        m_WChackPoint.ChangeDirection();
    }

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
}
