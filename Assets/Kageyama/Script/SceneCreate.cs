using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class SceneCreate : MonoBehaviour
{
    [SerializeField, TooltipAttribute("ステージ名")]
    private string _stageName;
    private TextAsset _csvFile; // CSVファイル
    private List<int> _csvData = new List<int>();   // CSVの中身を入れるリスト
    private int height = 0;
    private bool _endCreate;

    void Start()
    {
        _endCreate = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _endCreate = true;
        }
        while(_endCreate == false)
        {
            print("aa");
        }
    }
}

