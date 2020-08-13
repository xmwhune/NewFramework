using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace JJ
{
    /// <summary>
    /// 所有单例
    /// </summary>
    public class JJASingle
    {
        private  JJASingle()
        {
            Debug.Log("JJAsingle 构造");
        }
        public string Name { get; private set; }
        public  Type Type { get; private set; }
        protected JJASingle(Type t):this()
        {
            Type = t;
            Name = t.Name;
          
        }
        protected bool isActive;
        public virtual void OnAwake() { }
        public virtual void OnActive()
        {
            isActive = true;
        }
        public virtual void OnPause() { }
        public virtual void OnContinue() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUdate(){ }
        public virtual void OnDisible() { isActive = false; }
        public virtual void OnDestory() { }
        public virtual void OnAppQuit() { }
    }
    public class JJSingleHub : MonoBehaviour
    {
        private static JJSingleHub instance;
        public static JJSingleHub Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameObject("singleHub").AddComponent<JJSingleHub>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
        private Dictionary<Type, JJASingle> singleDic = new Dictionary<Type, JJASingle>();
        public void Register<T>(T single)where T:JJASingle
        {
            if (!singleDic.ContainsKey(typeof(T)))
            {
                singleDic.Add(typeof(T), single);
                single.OnAwake();
            }
        }
        public void UnRegister<T>()where T:JJASingle
        {
            if (singleDic.ContainsKey(typeof(T)))
            {
                singleDic[typeof(T)].OnDestory();
                singleDic.Remove(typeof(T));
            }
        }
        private void Update()
        {
            foreach (var v in singleDic)
                v.Value.OnUpdate();
        }
        private void OnApplicationPause(bool pause)
        {
            if(pause)
            {
                foreach (var v in singleDic)
                    v.Value.OnPause();
            }
            else
            {
                foreach (var v in singleDic)
                    v.Value.OnContinue();
            }
        }
        private void OnDestroy()
        {
            foreach (var v in singleDic)
            {
                Debug.Log(v.Value);
                v.Value.OnDestory();
            }
            singleDic = null;
        }
        private void OnApplicationQuit()
        {
            foreach (var v in singleDic)
                v.Value.OnAppQuit();
            singleDic.Clear();
     
        }
    }
}

