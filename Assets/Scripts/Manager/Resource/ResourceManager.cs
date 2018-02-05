using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System.IO;
namespace Framework
{
    public class ResourceManager : MonoBehaviour
    {
        private AssetBundle shared;

        public void initialize(Action func)
        {
            byte[] stream;
            string uri = string.Empty;
            uri = Util.DataPath + "shared.assetbundle";
            if (func != null) func();
        }
        // Use this for initialization
        void Start()
        {

        }
        public AssetBundle LoadBundle(string name)
        {
            byte[] stream = null;
            AssetBundle bundle = null;
            string uri = Util.DataPath + name.ToLower() + ".assetbundle";
            stream = File.ReadAllBytes(uri);
            bundle = AssetBundle.LoadFromMemory(stream);
            return bundle;
        }
        // Update is called once per frame
        void Update()
        {
            if (shared != null) shared.Unload(true);
        }
    }
}

