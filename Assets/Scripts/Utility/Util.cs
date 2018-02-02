using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.IO;
using System.Text;

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
                go.GetComponent<GameManager>().LuaIns.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
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
       
        //数据存放目录
        public static string DataPath
        {
            get
            {
                string game = Const.AppName.ToLower();
                if(Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/" + game + "/";
                }
                if(Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    return Application.streamingAssetsPath + "/";
                }
                if(Const.DebugMode)
                {
                    if(Application.isEditor)
                    {
                        return Application.dataPath + "/StreamingAssets/";
                    }
                }
                return "";
            }
        }

        public static string AppContentPath()
        {
            string path = string.Empty;
            switch(Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "Raw/";
                    break;
                default:
                    path = Application.dataPath + "/StreamingAssets/";
                    break;
            }
            return path;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }
    }
}

