using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace JJ
{
    public abstract class SdkAdapter 
    {
        public abstract void Init(Action<string> callback);
        public abstract void SendVideoAdsEvent(Action<int, string> callback);
        public abstract void SendInappPayRequset(int id, Action<string> callback);
        public abstract void SendSubscriptionPayRequset(int id, Action<string> callback);
        public abstract void SendCustomEvent(string mainKey, Dictionary<string, string> lable);
        public abstract void SendCustomEvent(string mainKey, Dictionary<string, string> lable,int times);
        public abstract void ShareText(string text);
        public abstract void LocalNotification(int id, string title, string content, int delayTime);
        public abstract void CancelLocalNotification(int id = -1);
        public abstract void FeedBackSubmit(string feedbackMsg, string contactWay, string archive, Action<string> callback);
        public abstract void ShowGameRecommendView();
        public abstract void UserLogin(Action<object> succeedCallback, Action<string> FailureCallback);
        public abstract void GetThirdpartyAuthorization(SDKSystem.ThirdpartyPlatform platform, Action<string> action);
        public abstract void GetAppVersion(int channel, Action<string> callback);
        public abstract void DedeemConsume(string redeemcode, Action<string> callback);
    }
}
