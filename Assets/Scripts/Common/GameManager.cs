﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using LuaInterface;
using System.IO;
using System;
using ICSharpCode.SharpZipLib.Zip;

public class GameManager : BaseLua {

    private string message;
    private ResourceManager ResManager;
    public LuaState LuaIns; // lua虚拟机

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        //InitGUI();
        DontDestroyOnLoad(gameObject);
        Util.Add<PanelManager>(gameObject);
        Util.Add<MusicManager>(gameObject);
        Util.Add<TimerManager>(gameObject);
        Util.Add<SocketClient>(gameObject);
        Util.Add<NetworkManager>(gameObject);
        ResManager = Util.Add<ResourceManager>(gameObject);
        CheckExtractResource();

        ZipConstants.DefaultCodePage = 65001;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = Const.GameFrameRate;
    }

    //资源获取
    public void CheckExtractResource()
    {
        bool isExists = Directory.Exists(Util.DataPath) &&
            Directory.Exists(Util.DataPath + "lua/") &&
            File.Exists(Util.DataPath + "files.txt");
        if(isExists || Const.DebugMode)
        {
            StartCoroutine(OnUpdateResource());
            return;
        }
        StartCoroutine(OnExtractResource()); //解压
    }

    //资源获取解压
    IEnumerator OnExtractResource()
    {
        string dataPath = Util.DataPath; //数据目录
        string resPath = Util.AppContentPath(); //游戏包资源目录
        if (Directory.Exists(dataPath))
            Directory.Delete(dataPath);
        Directory.CreateDirectory(dataPath);
        string infile = resPath + "files.txt";
        string outfile = dataPath + "files.txt";
        if (File.Exists(outfile))
            File.Delete(outfile);
        message = "正在解包：>files.txt";
        Debug.Log(message);
        if(Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;
            if(www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else
        {
            File.Copy(infile, outfile, true);
        }
        yield return new WaitForEndOfFrame();
        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        foreach(var file in files)
        {
            string[] fs = file.Split('|');
            infile = resPath + fs[0];
            outfile = dataPath + fs[0];
            message = "正在解包文件：>" + fs[0];
            Debug.Log(message);
            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else File.Copy(infile, outfile, true);
            yield return new WaitForEndOfFrame();
        }
        message = "解包完成!!!";
        yield return new WaitForSeconds(0.1f);
        message = string.Empty;
        //释放完成，开始启动更新资源
        StartCoroutine(OnUpdateResource());
    }

    IEnumerator OnUpdateResource()
    {
        if (!Const.UpdateMode)
        {
            ResManager.initialize(OnResourceInited);
            yield break;
        }
        WWW www = null;
        string dataPath = Util.DataPath;  //数据目录
        string url = string.Empty;
#if UNITY_5 
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            url = Const.WebUrl + "/ios/";
        }
        else
        {
            url = Const.WebUrl + "android/5x/";
        }
#else
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                url = Const.WebUrl + "/iphone/";
            } else {
                url = Const.WebUrl + "android/4x/";
            }
#endif
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt?v=" + random;
        if (Debug.isDebugBuild) Debug.LogWarning("LoadUpdate---->>>" + listUrl);

        www = new WWW(listUrl); yield return www;
        if (www.error != null)
        {
            OnUpdateFailed(string.Empty);
            yield break;
        }
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        File.WriteAllBytes(dataPath + "files.txt", www.bytes);
        string filesText = www.text;
        string[] files = filesText.Split('\n');

        for (int i = 0; i < files.Length; i++)
        {
            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string f = keyValue[0].Remove(0, 1);
            string localfile = (dataPath + f).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + f + "?v=" + random;
            bool canUpdate = !File.Exists(localfile);
            if (!canUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = Util.md5file(localfile);
                canUpdate = !remoteMd5.Equals(localMd5);
                if (canUpdate) File.Delete(localfile);
            }
            if (canUpdate)
            {   //本地缺少文件
                Debug.Log(fileUrl);
                message = "downloading>>" + fileUrl;
                www = new WWW(fileUrl); yield return www;
                if (www.error != null)
                {
                    OnUpdateFailed(path);   //
                    yield break;
                }
                File.WriteAllBytes(localfile, www.bytes);
            }
        }
        yield return new WaitForEndOfFrame();
        message = "更新完成!!";

        ResManager.initialize(OnResourceInited);
    }

    void OnUpdateFailed(string file)
    {
        message = "更新失败!>" + file;
    }

    void OnResourceInited()
    {
        LuaIns = new LuaState();
        LuaIns.Start();
        LuaIns.DoFile("logic/game"); //加载游戏
        LuaIns.DoFile("logic/network"); //加载网络
        ioo.networkManager.OnInit(); //初始化网络

        object[] panels = CallMethod("LuaScriptPanel");
        foreach(object o in panels)
        {
            string name = o.ToString().Trim();
            if (string.IsNullOrEmpty(name))
                continue;
            name += "Panel";
            Lua.DoFile("logic/" + name);
            Debug.LogWarning("LoadLua---->>>>" + name + ".lua");
        }
        CallMethod("OnInitOK");

    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 120, 960, 50), message);
    }



    void OnDestroy()
    {
        Debug.Log("~GameManager was destroyed");
    }
}
