using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 1.0f;                   // 移動速度
    [SerializeField]
    private float m_TrapHitSpeed = 3.0f;            // 移動速度
    [SerializeField]
    private float m_RageTime = 10.0f;               // 暴れる時間
    [SerializeField]
    private float m_ViewLength = 10.0f;             // プレイヤーが見える距離
    [SerializeField]
    private float m_ViewAngle = 30.0f;              // プレイヤーが見える角度
    [SerializeField]
    private Transform m_GroundPoint = null;         // 接地ポイント
    [SerializeField]
    private Transform m_RayPoint = null;            // レイポイント
    [SerializeField]
    private WallChackPoint m_WChackPoint = null;    // 壁捜索ポイント

    protected int m_Size = 1;                       // 動物の大きさ(内部数値)
    protected Vector2 m_Velocity = Vector2.right;   // 移動量
    protected Rigidbody2D m_Rigidbody;

    // モーション番号
    protected int m_MotionNumber = (int)AnimationNumber.ANIME_IDEL_NUMBER;

    private bool m_IsPravGround;                    // 前回の接地判定
    private string m_PlayerTag = "Player";          // プレイヤータグ
    private State m_State = State.Idel;             // 状態
    private float m_StateTimer = 0.0f;              // 状態の時間
    private DSNumber m_DSNumber = 
        DSNumber.DISCOVERED_CHASE_NUMBER;           // 追跡状態の番号                                                    
    private Player m_Player = null;                 // 当たったプレイヤー
    private List<State>
        m_DiscoveredStates = new List<State>();     // 発見後の行動

    //protected // アニメーション用のテクスチャリスト

    protected enum State
    {
        Idel,       // 待機状態
        Chase,      // 追跡状態
        Discover,   // 発見状態
        TrapHit,    // トラバサミに挟まれている状態
        Runaway,    // 逃亡状態
    }

    protected enum AnimationNumber
    {
        ANIME_IDEL_NUMBER = 0,
        ANIME_CHASE_NUMBER = 1,
        ANIME_DISCOVER_NUMBER = 2,
        ANIME_TRAP_NUMBER = 3,
        ANIME_RUNAWAY_NUMBER = 4,
        ANIME_DEAD_NUMBER = 5
    };

    // DiscoveredStateNumber
    protected enum DSNumber
    {
        DISCOVERED_CHASE_NUMBER = 0,
        DISCOVERED_RUNAWAY_NUMBER = 1
    }

    // Use this for initialization
    protected void Start()
    {
        // アニメーションリストにリソースを追加
        m_Rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (m_WChackPoint != null)
        {
            m_WChackPoint.transform.position =
                this.transform.position + this.transform.right * collider.radius;
        }

        m_DiscoveredStates.Add(State.Chase);
        m_DiscoveredStates.Add(State.Runaway);

        // スプライトの取得
        var sprite = gameObject.GetComponent<SpriteRenderer>();
        if (sprite == null) return;
        sprite.color = Color.red;
    }

    // Update is called once per frame
    protected void Update()
    {
        // 状態の更新
        UpdateState(Time.deltaTime);
    }

    // 状態の更新
    private void UpdateState(float deltaTime)
    {
        // 状態の変更
        switch (m_State)
        {
            case State.Idel: Idel(deltaTime); break;
            case State.Discover: Discover(deltaTime); break;
            case State.Chase: Chase(deltaTime); break;
            case State.TrapHit: TrapHit(deltaTime); break;
            case State.Runaway: Runaway(deltaTime); break;
        };

        if (IsGround()) print("接地");

        // 状態の時間加算
        m_StateTimer += deltaTime;

        // 位置ベクトルを代入
        Vector2 newVelocity = m_Rigidbody.velocity;
        Vector2 gravity = Vector2.up * m_Rigidbody.velocity.y;
        newVelocity = m_Velocity * m_Speed + gravity;
        m_Rigidbody.velocity = newVelocity;

        m_Velocity = Vector2.zero;
        m_IsPravGround = IsGround();
    }

    // 状態の変更
    protected void ChangeState(State state, AnimationNumber motion)
    {
        if (m_State == state) return;
        m_State = state;
        m_MotionNumber = (int)motion;
        m_StateTimer = 0.0f;
    }

    // 待機状態
    protected void Idel(float deltaTime)
    {
        //if (InPlayer())
        //{
        //    // 発見状態に遷移
        //    ChangeState(State.Discover, AnimationNumber.ANIME_IDEL_NUMBER);
        //    m_DSNumber = DSNumber.DISCOVERED_RUNAWAY_NUMBER;
        //    return;
        //};

        // 移動
        Move(deltaTime);
    }

    // 発見状態
    protected void Discover(
        float deltaTime,
        DSNumber number =
        DSNumber.DISCOVERED_CHASE_NUMBER)
    {
        // 接地したら、他の状態に遷移
        if (!m_IsPravGround && IsGround())
        {
            //ChangeState(State.Runaway, AnimationNumber.ANIME_RUNAWAY_NUMBER);
            ChangeState(
                m_DiscoveredStates[(int)m_DSNumber],
                AnimationNumber.ANIME_CHASE_NUMBER);
            return;
        }
    }

    // 追跡状態
    protected void Chase(float deltaTime)
    {
        // 移動
        ChasePlayer();
        //Move(deltaTime, 2.0f);
    }

    // トラバサミに挟まれている状態
    protected void TrapHit(float deltaTime)
    {
        // プレイヤーが壁に当たった場合は、折り返す
        //Collision2D.Equals

        // 移動(通常の移動速度の数倍)
        Move(deltaTime, m_TrapHitSpeed);

        if (m_Player._state == Player.State.Wait)
        {
            ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
            m_Player = null;
            // スプライトの取得
            var sprite = gameObject.GetComponent<SpriteRenderer>();
            if (sprite == null) return;
            sprite.color = Color.red;
        }
    }

    // 逃げ状態
    protected void Runaway(float deltaTime)
    {
        // 移動(通常の移動速度の数倍)
        Move(deltaTime, 4.0f);
        //find
    }

    protected void Move(float deltaTime, float subSpeed = 1.0f)
    {
        // 壁に衝突したときに、折り返す
        TurnWall();
        // 移動
        m_Velocity = m_Speed * subSpeed * this.transform.right * deltaTime;
    }

    // プレイヤーが見えているか
    protected bool InPlayer()
    {
        //var isPlayer = false;
        var pName = "Player";
        var player = GameObject.Find(pName);
        // プレイヤーがいない場合は返す
        print("プレイヤー調査");
        if (player == null) return false;
        // レイポイントからプレイヤーの位置までのレイを伸ばす
        RaycastHit2D hit = Physics2D.Raycast(
            m_RayPoint.position,
            player.transform.position
            );
        // プレイヤーに当たらなかった場合、
        // プレイヤー以外に当たった場合は返す
        print("見えているか調査");
        if (hit.collider == null || hit.collider.name != pName) return false;
        //if (hit.collider.name != pName) return false;
        // プレイヤーとの距離を求める
        var length = Vector2.Distance(
            m_RayPoint.position,
            player.transform.position
            );
        // 可視距離から離れていれば返す
        print("距離調査");
        if (length > m_ViewLength) return false;
        // 視野角の外ならば返す
        var dir = player.transform.position - m_RayPoint.position;
        var angle = Vector2.Angle(this.transform.forward, dir);
        print("角度調査");
        if (Mathf.Abs(angle) < angle) return false;
        // プレイヤーを見つけた
        print("見つけた");
        return true;
    }

    // 壁に衝突したときに、折り返します
    protected void TurnWall()
    {
        // 壁に当たった、崖があった場合は折り返す
        if (m_WChackPoint != null)
        {
            if (m_WChackPoint.IsWallHit())
            {
                //Quaternion.
                // 進行方向の逆の向きを向く(Y軸)
                var degree = this.transform.rotation.y * 180.0f + 180.0f;
                this.transform.rotation =
                    Quaternion.AngleAxis(
                        degree,
                        transform.up
                        );
                // 衝突後の処理
                m_WChackPoint.ChangeDirection();
            }
        }
    }

    // 接地しているか
    protected bool IsGround()
    {
        int layerMask = LayerMask.GetMask(new string[] { "Ground" });
        Collider2D hit =
            Physics2D.OverlapPoint(m_GroundPoint.position, layerMask);
        return hit != null;
    }

    //// プレイヤーとの向きを返します(単位ベクトル)
    //protected Vector2 PlayerDirection()
    //{
    //    var player = GameObject.Find("Player");
    //    // プレイヤーがいなければ、ゼロベクトルを返す
    //    if (player != null) return Vector2.zero;
    //    var direction = new Vector2(1.0f, 1.0f);
    //    var dir = player.transform.position - this.transform.position;
    //    if (dir.x < 0.0f) direction.x = -1.0f;
    //    if (dir.y < 0.0f) direction.y = -1.0f;
    //    return direction;
    //}

    // プレイヤーとの衝突判定処理
    protected void OnCollidePlayer(Collider2D collision)
    {
        var tag = collision.gameObject.tag;
        if (tag == m_PlayerTag)
        {
            //// 当たったプレイヤーを子供に追加
            //collision.gameObject.transform.parent = gameObject.transform;
            var player = collision.GetComponent<Player>();

            // プレイヤーがはさんだ状態なら、トラップヒット状態に遷移
            if (player._state == Player.State.Endure)
            {
                // トラップヒット状態に遷移
                ChangeState(
                State.TrapHit,
                AnimationNumber.ANIME_TRAP_NUMBER
                );
                m_Player = player;
                // スプライトの取得
                var sprite = gameObject.GetComponent<SpriteRenderer>();
                if (sprite == null) return;
                sprite.color = Color.yellow;
            }
        }
    }

    // プレイヤーの方向を向きます(単位ベクトル)
    public void ChasePlayer()
    {
        var player = GameObject.Find("Player");
        // プレイヤーがいなければ、返す
        //if (player != null) return;
        if (player != null)
        {
            ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
            return;
        }
        //var direction = new Vector2(1.0f, 1.0f);
        var direction = Vector2.right;
        var dir = player.transform.position - this.transform.position;
        var length = 2.0f;
        // 近づきすぎたら、移動しない
        if (Mathf.Abs(dir.x) < length) direction.x = 0.0f;
        // 方向転換
        if (dir.x < 0.0f) direction.x = -1.0f;
        //if (dir.y < 0.0f) direction.y = -1.0f;

        m_Velocity = m_Speed * direction * Time.deltaTime;
    }

    // 敵がはさまれた時の、暴れる時間を返します(秒数)
    public float RageTime() { return m_RageTime; }

    // 衝突判定(トリガー用)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollidePlayer(collision);
    }

    // 衝突中(トリガー用)
    public void OnTriggerStay2D(Collider2D collision)
    {
        OnCollidePlayer(collision);
    }

    //public void OnTriggerExit2D(Collider2D collision)
    //{
    //    var tag = collision.transform.tag;

    //    if(tag == m_PlayerTag)
    //    {
    //        ChangeState(State.Idel, AnimationNumber.ANIME_IDEL_NUMBER);
    //        return;
    //    }
    //}

    // 衝突判定
    public void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;

        // プレイヤーに当たった場合
        if (tag == m_PlayerTag)
        {
            // 当たったプレイヤーを子供に追加
            collision.gameObject.transform.parent = gameObject.transform;

            //if (player == null) return;
            //player
            //player._state = Player.State.Endure;
            //player._target = gameObject;
        }
    }

    // ギズモの描画
    public void OnDrawGizmos()
    {
        var player = GameObject.Find("Player");
        if (player == null) return;
        Gizmos.color = Color.red;
        // レイの描画
        Gizmos.DrawLine(m_RayPoint.position, player.transform.position);
    }



    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy), true)]
    public class EnemyEditor : Editor
    {
        SerializedProperty Speed;
        SerializedProperty TrapHitSpeed;
        SerializedProperty RageTime;
        SerializedProperty ViewLength;
        SerializedProperty ViewAngle;
        SerializedProperty GroundPoint;
        SerializedProperty RayPoint;
        SerializedProperty WChackPoint;

        public void OnEnable()
        {
            Speed = serializedObject.FindProperty("m_Speed");
            TrapHitSpeed = serializedObject.FindProperty("m_TrapHitSpeed");
            RageTime = serializedObject.FindProperty("m_RageTime");
            ViewLength = serializedObject.FindProperty("m_ViewLength");
            ViewAngle = serializedObject.FindProperty("m_ViewAngle");
            GroundPoint = serializedObject.FindProperty("m_GroundPoint");
            RayPoint = serializedObject.FindProperty("m_RayPoint");
            WChackPoint = serializedObject.FindProperty("m_WChackPoint");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            // 必ず書く
            serializedObject.Update();

            Enemy enemy = target as Enemy;

            // float
            Speed.floatValue = EditorGUILayout.FloatField("移動速度(m/s)", enemy.m_Speed);
            TrapHitSpeed.floatValue = EditorGUILayout.FloatField("はさまれた時の速度(m/s)", enemy.m_TrapHitSpeed);
            RageTime.floatValue = EditorGUILayout.FloatField("暴れる時間(秒)", enemy.m_RageTime);
            ViewLength.floatValue = EditorGUILayout.FloatField("視野距離(m)", enemy.m_ViewLength);
            ViewAngle.floatValue = EditorGUILayout.FloatField("視野角度(度数法)", enemy.m_ViewAngle);

            EditorGUILayout.Space();
            // Transform
            GroundPoint.objectReferenceValue = EditorGUILayout.ObjectField("接地ポイント", enemy.m_GroundPoint, typeof(Transform), true);
            RayPoint.objectReferenceValue = EditorGUILayout.ObjectField("レイポイント", enemy.m_RayPoint, typeof(Transform), true);

            EditorGUILayout.Space();
            // WallChackPoint
            WChackPoint.objectReferenceValue = EditorGUILayout.ObjectField("壁捜索ポイント", enemy.m_WChackPoint, typeof(WallChackPoint), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更できなくなる)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
