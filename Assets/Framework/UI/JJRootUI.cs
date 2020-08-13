using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JJ
{
    public class JJRootUI : MonoBehaviour
    {
        private static string uiPath = "RootUI";
        private static JJRootUI instance;
        public static JJRootUI Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uiPath));
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<JJRootUI>();
                    instance.Init();
                }
                return instance;
            }
        }
        public Transform fixedTF { get; private set; }
        public Transform popUpTF { get; private set; }
        public Transform normolTF { get; private set; }
        private void Init()
        {
            fixedTF = transform.Find("Fixed");
            popUpTF = transform.Find("PopUp");
            normolTF = transform.Find("Normol");
        }
    }
}

