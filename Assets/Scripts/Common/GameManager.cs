using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class GameManager : BaseLua {

    private string message;
    private ResourceManager ResManager;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        InitGUI();
        DontDestroyOnLoad(gameObject);
        Util.Add<PanelManager>(gameObject);
        Util
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
