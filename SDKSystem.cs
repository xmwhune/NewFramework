using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace JJ
{
    public  class SDKSystem : JJASingle
    {
        public enum ThirdpartyPlatform
        {
            QQ,
            Wechat,
            IOS,
            OPPO,
            FaceBook,
        }
        SdkAdapter sdk;
        public SDKSystem() : base(typeof(SDKSystem))
        {
            Debug.Log("SDKSystem 构造");
            JJSingleHub.Instance.Register(this);
        }
        private static SDKSystem instance;
        public static SDKSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new SDKSystem();
                return instance;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="action"></param>
        public  void Init(Action<string>action)
        {
            sdk.Init(action);
        }

        /// <summary>
        /// 点击广告事件注册
        /// </summary>
        /// <param name="callback"></param>
        public void SendVideoAdsEvent(Action<string> succeedCallback,Action<string>failureCallback)
        {
            sdk.SendVideoAdsEvent((i,s)=> {
                if (i == 1)
                    failureCallback?.Invoke(s);
                else if (i == 2)
                    succeedCallback?.Invoke(s);
            });
        }
        /// <summary>
        /// 注册内购事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void SendInappPayRequset(int id, Action<string> succesFunc = null, Action<string> failFunc = null)
        {
            sdk.SendInappPayRequset(id, (s) =>
            {
                if (s == "1")
                    succesFunc?.Invoke(s);
                else 
                    failFunc?.Invoke(s);
            });
        }
        /// <summary>
        /// 注册订阅事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void SendSubscriptionPayRequset(int id, Action<string> succesFunc = null, Action<string> failFunc = null)
        {
            sdk.SendSubscriptionPayRequset(id, (s) =>
            {
                if (s == "1")
                    succesFunc?.Invoke(s);
                else
                    failFunc?.Invoke(s);
            });
        }

        Dictionary<string, string> customDiv = new Dictionary<string, string>();
        /// <summary>
        /// 自定义统计事件
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="firstKey"></param>
        /// <param name="value"></param>
        public void SendCustomEvent(string mainKey,string firstKey,string value,int times=1)
        {
            customDiv.Clear();
            customDiv.Add(firstKey, value);
            if (times == 1)
            {
                sdk.SendCustomEvent(mainKey, customDiv);
            }
            else if (times > 1)
                sdk.SendCustomEvent(mainKey, customDiv, times);
        }
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="text"></param>
        public void ShareText(string text)
        {
            sdk.ShareText(text);
        }
        /// <summary>
        /// 本地推送
        /// </summary>
        /// <param name="id">自定义退送id</param>
        /// <param name="title">推送标题</param>
        /// <param name="content">推送内容</param>
        /// <param name="delayTime">延迟时间，从当前时间算起</param>
        public void LocalNotification(int id,string title,string content,int delayTime)
        {
            sdk.LocalNotification(id, title, content, delayTime);
        }
        /// <summary>
        /// 取消推送通知
        /// </summary>
        /// <param name="id">通知id ，为 -1时，取消所有通知</param>
        public void CancelLocalNotification(int id =-1)
        {
            sdk.CancelLocalNotification(id);
        }
        /// <summary>
        /// 用户反馈
        /// </summary>
        /// <param name="feedbackMsg"></param>
        /// <param name="contactWay"></param>
        /// <param name="archive"></param>
        /// <param name="callback"></param>
        public void FeedBackSubmit(string feedbackMsg, string contactWay,string archive,Action<string> callback)
        {
            sdk.FeedBackSubmit(feedbackMsg, contactWay, archive, callback);
        }
        /// <summary>
        /// 游戏推荐
        /// </summary>
        public void ShowGameRecommendView() 
        {
            sdk.ShowGameRecommendView();
        }
        /// <summary>
        /// 用户登陆服务端
        /// </summary>
        /// <param name="succeedCallback"></param>
        /// <param name="FailureCallback"></param>
        public void UserLogin(Action<object> succeedCallback, Action<string> FailureCallback)
        {

        }
        /// <summary>
        /// 获取第三方授权
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="action"></param>
        public void GetThirdpartyAuthorization( ThirdpartyPlatform platform , Action<string> action)
        {
            sdk.GetThirdpartyAuthorization(platform, action);
        }
        /// <summary>
        /// 获取game版本
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="callback"></param>
        public void GetAppVersion(int channel,Action<string >callback)
        {
            sdk.GetAppVersion(channel, callback);
        }
        /// <summary>
        /// 兑换
        /// </summary>
        /// <param name="redeemcode"></param>
        /// <param name="callback"></param>
        public void DedeemConsume(string redeemcode,Action<string> callback)
        {
            sdk.DedeemConsume(redeemcode, callback);
        }

        public override void OnAwake()
        {
            base.OnAwake();
            Debug.Log("SDKSystem.OnAwake");
        }
    }
}

