using UnityEngine;
using UnityEditor;
using JJ;

namespace JJ
{
    public enum UIType
    {
        Fixed,
        PopUp,
        Normol,
    }
    /// <summary>
    /// 相对父物体，作为子物体的顺序
    /// </summary>
    public enum UIPosType
    {
        Normol,
        Last,
        First
    }
    public class JJBaseUI 
    {
        protected GameObject gameObject;
        protected Transform transform;

        protected bool isActive = false;
        public UIType uiType { get; private set; }
        public UIPosType uiPosTypd { get; private set; }
        private JJBaseUI() { }
        protected JJBaseUI(string path,UIType uiType,UIPosType uIPos):this()
        {
            gameObject = GameObject.Instantiate(Resources.Load<GameObject>(path));
            transform = gameObject.transform;
            this.uiType = uiType;
            this.uiPosTypd = uIPos;
        }
        public  virtual void OnAwake()
        {
            switch (uiType)
            {
                case UIType.Fixed:
                    transform.SetParent(JJRootUI.Instance.fixedTF);
                    break;
                case UIType.PopUp:
                    transform.SetParent(JJRootUI.Instance.popUpTF);
                    break;
                case UIType.Normol:
                    transform.SetParent(JJRootUI.Instance.normolTF);
                    break;
            }
        }
        public virtual void OnActive()
        {
            isActive = true;
            gameObject.SetActive(true);
            switch(uiPosTypd)
            {
                case UIPosType.First:
                    transform.SetAsFirstSibling();
                    break;
                case UIPosType.Last:
                    transform.SetAsLastSibling();
                    break;
                case UIPosType.Normol:
                    break;
            }
        }
        public virtual void OnUpdate() { }
        public virtual void OnLateUdate() { }
        public virtual void OnDisible()
        { 
            isActive = false; 
            gameObject.SetActive(false); 
        }
        public virtual void OnDestory() { }
    }

}
public class TestUI : JJBaseUI
{
    public TestUI() : base("UITest", UIType.Fixed, UIPosType.Last)
    {
    }
    public override void OnAwake()
    {
        base.OnAwake();
    }
    public override void OnActive()
    {
        base.OnActive();
    }
    public override void OnDisible()
    {
        base.OnDisible();
    }
}
