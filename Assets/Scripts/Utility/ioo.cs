using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ioo
    {
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
        //管理器
        private static GameManager _gameManager = null;
        public static GameManager gameManager
        {
            get
            {
                if (gameManager == null)
                    _gameManager = manager.GetComponent<GameManager>();
                return _gameManager;
            }
        }
    }
}
