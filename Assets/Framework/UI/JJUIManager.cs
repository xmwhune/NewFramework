using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace JJ
{
    public class JJUIManager : JJASingle
    {
        private static JJUIManager mUIManager;
        protected JJUIManager() : base(typeof(JJUIManager))
        {
            JJSingleHub.Instance.Register(this);
        }
        public static JJUIManager Instance
        {
            get
            {
                if (mUIManager == null)
                    mUIManager = new JJUIManager();
                return mUIManager;
            }
        }

        Dictionary<Type, JJBaseUI> uiDic = new Dictionary<Type, JJBaseUI>();
        private  void Register<T>(T instance) where T : JJBaseUI
        {
            if (!uiDic.ContainsKey(typeof(T)))
            {
                uiDic.Add(typeof(T), instance);
                instance.OnAwake();
            }
        }
        private  void UnRegister<T>() where T : JJBaseUI
        {
            if (uiDic.ContainsKey(typeof(T)))
            {
                uiDic[typeof(T)].OnDestory();
                uiDic.Remove(typeof(T));
            }
        }
        private void UNRegisterAll()
        {
            foreach(var v in uiDic)
            {
                v.Value.OnDestory();
            }
            uiDic.Clear();
        }
        public T ShowUIView<T>()where T: JJBaseUI,new ()
        {
            JJBaseUI ui = null;
            if (uiDic.ContainsKey(typeof(T)))
             ui =   uiDic[typeof(T)];
            else
            {
                ui = new T();
                Register(ui);
            }
            ui?.OnActive();
            return (T)ui;
        }
        public T GetUIView<T>()where T:JJBaseUI
        {
            JJBaseUI ui = null;
            if (uiDic.ContainsKey(typeof(T)))
                ui = uiDic[typeof(T)];
            return (T)ui;
        }
        public T CloseUIView<T>()where T:JJBaseUI,new()
        {
            JJBaseUI ui = null;
            if (uiDic.ContainsKey(typeof(T)))
            {
                ui = uiDic[typeof(T)];
                ui.OnDisible();
            }
            return (T)ui;
        }
        public override void OnDestory()
        {
            UNRegisterAll();
            uiDic = null;
            base.OnDestory();

        }
    
    }
}
