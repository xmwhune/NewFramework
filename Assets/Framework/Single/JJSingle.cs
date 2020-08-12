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
    }
}

