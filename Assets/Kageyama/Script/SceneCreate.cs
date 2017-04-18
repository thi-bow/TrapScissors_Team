using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Diagnostics;
#endif

public class SceneCreate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _stageObje;

    public void ObjeIns(int num)
    {
        Instantiate(_stageObje[num], new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));
    }

    //#if UNITY_EDITOR
    //    [CustomEditor(typeof(SceneCreate))]
    //    public class SceneCreateEditor : Editor
    //    {

    //        void Start()
    //        {
    //            UnityEditor.SceneView.onSceneGUIDelegate += OnSceneView;
    //        }

    //        void OnDestroy()
    //        {
    //            UnityEditor.SceneView.onSceneGUIDelegate -= OnSceneView;
    //        }

    //        public void OnSceneView(SceneView sceneView)
    //        {
    //            serializedObject.Update();
    //            SceneCreate scene = target as SceneCreate;
    //            Handles.BeginGUI();
    //            if (GUILayout.Button("ボタンです", GUILayout.Width(126)))
    //            {
    //                scene.ObjeIns(0);
    //            }
    //            Handles.EndGUI();

    //            serializedObject.ApplyModifiedProperties();
    //        }
    //    }
    //#endif
    }


#if UNITY_EDITOR
[InitializeOnLoad]
public class NewBehaviourScript
{
    SceneCreate scene = new SceneCreate();
    // ボタンの大きさ
    const float ButtonWidth = 120f;

    static NewBehaviourScript()
    {
        SceneView.onSceneGUIDelegate += (sceneView) =>
        {
            Handles.BeginGUI();
            if (GUILayout.Button("ボタンです", GUILayout.Width(ButtonWidth)))
            {
                //scene.ObjeIns(0);
            }
            Handles.EndGUI();
        };
    }
}
#endif
