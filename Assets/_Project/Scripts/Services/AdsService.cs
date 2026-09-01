using System;
using YG;

namespace _Project.Scripts.Services
{
    public class AdsService
    {
        public void UseInter(Action action)
        {
            if (YG2.isTimerAdvCompleted && YG2.isSDKEnabled)
            {
                void OnClose(bool wasShown)
                {
                    YG2.onCloseInterAdvWasShow -= OnClose;

                    action.Invoke();
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
            if (YG2.isTimerAdvCompleted && YG2.isSDKEnabled)
            {
                void OnClose(bool wasShown)
                {
                    YG2.onCloseInterAdvWasShow -= OnClose;

                    action.Invoke();
                }
                YG2.onCloseInterAdvWasShow += OnClose;
                YG2.InterstitialAdvShow();
            }
            else
            {
                action.Invoke();
            }
        }
    }
}