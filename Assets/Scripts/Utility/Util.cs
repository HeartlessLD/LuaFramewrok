using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

namespace Framework
{
    public class Util
    {
        public static GameObject LoadAsset(AssetBundle bundle, string name)
        {
#if UNITY_5
            return bundle.LoadAsset(name, typeof(GameObject)) as GameObject;
#else
            return bundle.Load(name, typeof(GameObject)) as GameObject;
#endif
        }

        public static void ClearMemory()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
            GameObject go = GameObject.FindWithTag("GameManager");
            if(go != null)
            {
                go.GetComponent<GameManager>().Lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
            }
        }

        public static T Add<T>(GameObject go) where T: Component
        {
            if(go != null)
            {
                T[] ts = go.GetComponents<T>();
                for(int i = 0; i < ts.Length; i++)
                {
                    if(ts[i] != null)
                    {
                        GameObject.Destroy(ts[i]);
                    }
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }
       
    }
}

