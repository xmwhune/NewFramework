using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JJ
{
    public class JJSingle
    {
       private JJSingle(){}
       protected JJSingle(type t):this()
       {
           mType = t;
           mName = typeof(t).Name;
       }
       protected Type mType;
       protected string mName;
       public virtual void OnAwake(){}
       public virtual void OnActive(){}
       public virtual void OnUpdate(){}
       public virtual void OnLateUpdate(){}
       public virtual void OnPause(){}
       public virtual void OnContinue(){}
       public virtual void  OnDisable(){}
       public virtual void OnAppQuit(){}
       public virtual void OnDestroy(){}
    }
}

