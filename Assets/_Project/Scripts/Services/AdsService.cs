using System;
using UnityEngine;
using YG;
using YG.Insides;

namespace _Project.Scripts.Services
{
    public class AdsService
    {
        public string rewardID;
        
        public void UseInter(Action action)
        {
            if (YG2.isTimerAdvCompleted && YG2.isSDKEnabled)
            {
                void OnClose(bool wasShown)
                {
                    YG2.onCloseInterAdvWasShow -= OnClose;

                    action.Invoke();
                    Debug.Log("InterstitialAdvShow");
                }
                YG2.onCloseInterAdvWasShow += OnClose;
                YG2.InterstitialAdvShow();
            }
            else
            {
                action.Invoke();
            }
        }
        
        public void UseReward(Action action)
        {
            if (YG2.isSDKEnabled)
            {
                YG2.RewardedAdvShow(rewardID, action);
                YGInsides.SetTimerInterAdv();
                Debug.Log("RewardedAdvShow");
            }
            else
            {
                action.Invoke();
            }
        }
    }
}