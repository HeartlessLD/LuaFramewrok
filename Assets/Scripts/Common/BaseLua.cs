using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Framework;
public class BaseLua : MonoBehaviour {

    private string data = null;
    private bool initialize = false;
    private Transform trans = null;
    private LuaState luaSate = null;
    private AssetBundle bundle = null;
    private List<LuaFunction> buttons = new List<LuaFunction>();

    protected LuaState Lua
    {
        get
        {
            if(luaSate == null)
            {
                luaSate = new LuaState();
            }
            return luaSate;
        }
    }
    // Use this for initialization
    void Start()
    {
        trans = transform;
        LuaState lua = Lua;
        lua.Start();
        lua[trans.name + ".transform"] = transform;
        lua[trans.name + ".gameObject"] = gameObject;
        CallMethod("Start");

    }
    
	
	// Update is called once per frame
	protected void OnClick()
    {
        CallMethod("OnClick");
    }

    protected void OnClickEvent(GameObject go)
    {
        CallMethod("OnClick", go);
    }

    public void OnInit(AssetBundle bundle, string text = null)
    {
        this.data = text;
        this.bundle = bundle;
        Debug.LogWarning("OnInit--->>" + name + "text:>" + text);
    }

    public GameObject GetGameObject(string name)
    {
        if (bundle == null)
            return null;
        return Util.LoadAsset(bundle, name);
    }

    public void AddClick(string button, LuaFunction luafunc)
    {
        Transform to = trans.Find(button);
        if (to == null)
            return;
        GameObject go = to.gameObject;
        UIEventListener.Get(go).onClick = delegate (GameObject o)
        {
            luafunc.Call<GameObject>(go);
            buttons.Add(luafunc);
        };
    }
    public void ClearClick()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i] != null)
            {
                buttons[i].Dispose();
                buttons[i] = null;
            }
        }
    }
    protected object[] CallMethod(string func)
    {
        string funcName = name + "." + func;
        funcName.Replace("(Clone)", "");
        LuaFunction function = Lua.GetFunction(funcName);
        object[] result = function.LazyCall(null);
        function.Dispose();
        return result;
    }
    protected object[] CallMethod(string func, GameObject go)
    {
        string funcName = name + "." + func;
        funcName.Replace("(Clone)", "");
        LuaFunction function = Lua.GetFunction(funcName);
        object[] result = function.LazyCall(go);
        function.Dispose();
        return result;
    }

    protected object[] CallMethod(string func, int key, ByteBuffer buffer)
    {
        string funcName = name + "." + func;
        funcName.Replace("(Clone)", "");
        LuaFunction function = Lua.GetFunction(funcName);
        object[] result = function.LazyCall(key, buffer);
        function.Dispose();
        return result;
    }

    protected void OnDestroy()
    {
        if(bundle)
        {
            bundle.Unload(true);
            bundle = null;
        }
        ClearClick();
        luaSate.CheckTop();
        luaSate.Dispose();
        luaSate = null;
        Util.ClearMemory();
        Debug.Log(name + "was destroy");
    }

}
