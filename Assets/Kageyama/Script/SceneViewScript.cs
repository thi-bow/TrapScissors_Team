using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Diagnostics;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
public class SceneViewScript
{
    //// ボタンの大きさ
    //const float ButtonWidth = 120f;

    //static SceneViewScript()
    //{
    //    SceneView.onSceneGUIDelegate += (sceneView) =>
    //    {
    //        Handles.BeginGUI();
    //        if (GUILayout.Button("生成", GUILayout.Width(ButtonWidth)))
    //        {
    //            //EditorApplication.ExecuteMenuItem("Assets/Kageyama/Prefab/sample1");
    //        }
    //        Handles.EndGUI();
    //    };
    //}
}
#endif
