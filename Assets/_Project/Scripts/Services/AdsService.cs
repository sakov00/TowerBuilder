using System;
using UnityEngine;
using YG;
using YG.Insides;

namespace _Project.Scripts.Services
{
    public class AdsService
    {
        public string rewardID;

        private Action _interAction;

        public void UseInter()
        {
            if (YG2.isTimerAdvCompleted && YG2.isSDKEnabled)
            {
                YG2.InterstitialAdvShow();
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
                action?.Invoke();
            }
        }
    }
}