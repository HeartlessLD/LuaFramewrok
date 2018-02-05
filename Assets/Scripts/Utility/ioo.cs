using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ioo
    {
        private static Hashtable prefabs = new Hashtable();
        //管理器对象
        private static GameObject _manager = null;
        public static GameObject manager
        {
            get
            {
                if (_manager == null)
                    _manager = GameObject.FindWithTag("GameManager");
                return _manager;
            }
        }
        //游戏管理器
        private static GameManager _gameManager = null;
        public static GameManager gameManager
        {
            get
            {
                if (_gameManager == null)
                    _gameManager = manager.GetComponent<GameManager>();
                return _gameManager;
            }
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        private static ResourceManager _resourceManager = null;
        public static ResourceManager resourceManager
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager = manager.GetComponent<ResourceManager>();
                return _resourceManager;
            }
        }

        /// <summary>
        /// 网络管理器
        /// </summary>
        private static NetworkManager _networkManager = null;
        public static NetworkManager networkManager
        {
            get
            {
                if (_networkManager == null)
                    _networkManager = manager.GetComponent<NetworkManager>();
                return _networkManager;
            }
        }

        //面板管理器
        private static PanelManager _panelManager = null;
        public static PanelManager panelManager
        {
            get
            {
                if(_panelManager == null)
                {
                    _panelManager = manager.AddComponent<PanelManager>();
                }
                return _panelManager;
            }
        }

        ///
        /// 载入Prefab
        ///
        public static GameObject LoadPrefab(string name)
        {
            GameObject go = GetPrefab(name);
            if (go != null)
                return go;
            go = Resources.Load("Prefabs/" + name, typeof(GameObject)) as GameObject;
            AddPrefab(name, go);
            return go;
        }

        public static GameObject GetPrefab(string name)
        {
            if (!prefabs.ContainsKey(name))
                return null;
            return prefabs[name] as GameObject; 
        }

        public static void AddPrefab(string name, GameObject go)
        {
            prefabs.Add(name, go);
        }

        //gui摄像机
        public static Transform guiCamera
        {
            get
            {
                GameObject go = GameObject.FindWithTag("GuiCamera");
                if (go != null)
                    return go.transform;
                return null;
            }
        }
    }
}
