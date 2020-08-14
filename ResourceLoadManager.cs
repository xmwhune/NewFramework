using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace JJ
{
    public enum ResourceLoadType
    {
        AsseteBundle,
        Resources,
        AsseteDataBase,
    }
    public class ResourceLoadManager : JJASingle
    {
        protected ResourceLoadManager() : base(typeof(ResourceLoadManager))
        {
            JJSingleHub.Instance.Register(this);
            assetBundlePath = Application.streamingAssetsPath +
#if UNITY_ANDROID
                "/Android";
#elif UNITY_IOS
"/IOS";
#else
#endif


        }
        private static ResourceLoadManager instance;
        public static ResourceLoadManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ResourceLoadManager();
                }
                return instance;
            }
        }

        private ResourceLoadType loadType = ResourceLoadType.AsseteBundle;

        private string assetBundlePath;
        public T Load<T>(string path)where T:UnityEngine.Object
        {
            switch(loadType)
            {
                case ResourceLoadType.Resources:
                    return Resources.Load<T>(path);
                //case ResourceLoadType.AsseteDataBase:
                //    return AssetDatabase.LoadAssetAtPath<T>("Assets/GameAssets/" + path);
                case ResourceLoadType.AsseteBundle:
                    return LoadByAsseteBundle<T>(path);
                default:
                    return null;
            }
        }
        private T LoadByAsseteBundle<T>(string path) where T : UnityEngine.Object
        {
            JJSingleHub.Instance.StartCoroutine(LoadFromAssetBundleMainfiles());
            return null;
        }
        IEnumerator LoadFromAssetBundle(string path)
        {
            string bundleUrl =Uri.EscapeUriString( assetBundlePath + "/" + path);
            using (UnityWebRequest quest  = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
            {
                yield return null;
            }
        }
        IEnumerator LoadFromAssetBundleMainfiles()
        {
            Uri bundleUrl =new Uri(assetBundlePath + "/" + "Android");
            using(UnityWebRequest quest = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl.AbsoluteUri))
            {
                yield return quest.SendWebRequest();
                
                if (quest.isDone && quest.error == null)
                {
                    AssetBundle manifestBundle = (quest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                 
                    if (manifestBundle != null)
                    {
                        try
                        {
                            AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                            if (manifest != null)
                            {
                                string[] bundlesName = manifest.GetAllAssetBundles();
                                foreach (var v in bundlesName)
                                {
                                    Debug.Log(v);
                                    string[] depBundlesName = manifest.GetAllDependencies(v);
                                    foreach (var n in depBundlesName)
                                        Debug.Log(n);
                                }
                            }
                        }
                        catch
                        {

                        }
                        manifestBundle.Unload(false);
                    }
                    else
                    {
                        Debug.Log(bundleUrl.AbsoluteUri);
                    }
                }
            }
        }
        public override void OnAppQuit()
        {
            base.OnAppQuit();
        }
        public override void OnAwake()
        {
            base.OnAwake();
        }
        public override void OnDestory()
        {
            base.OnDestory();
        }

    }
}

