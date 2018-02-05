using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PanelManager : MonoBehaviour
    {
        private Transform parent;
        Transform Parent
        {
            get
            {
                if(parent == null)
                {
                    parent = ioo.guiCamera;
                }
                return parent;
            }
        }

        public void CreatePanel(string name)
        {
            AssetBundle bundle = ioo.resourceManager.LoadBundle(name);
            StartCoroutine(StartCreatePanel(name, bundle, string.Empty));
        }

        IEnumerator StartCreatePanel(string name, AssetBundle bundle, string text = null)
        {
            name += "Panel";
            GameObject prefab = Util.LoadAsset(bundle, name);
            yield return new WaitForEndOfFrame();
            if(Parent.Find(name) != null || prefab == null)
            {
                yield break;
            }
            GameObject go = Instantiate(prefab, parent) as GameObject;
            go.name = name;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.parent = parent;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            yield return new WaitForEndOfFrame();
            go.AddComponent<BaseLua>().OnInit(bundle);
            Debug.Log("StartCreatePanel--" + name);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
