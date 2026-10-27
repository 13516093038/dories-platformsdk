#if UNITY_WEBGL && DOUYINMINIGAME
using TTSDK;
using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 抖音激励视频广告，对应 tt.createRewardedVideoAd / RewardedVideoAd。
    /// 全局仅应存在一个激励视频实例，重复创建无效，需先 Destroy 再重建。
    /// https://developer.open-douyin.com/docs/resource/zh-CN/mini-game/develop/api/javascript-api/ads/rewarded-video-ad/rewarded-video-ad
    /// </summary>
    public class TTRewardADSDKComponent : RewardADSDKComponent
    {
        private string m_AdUnitId;
        private TTRewardedVideoAd m_Ad;
        private RewardAdPlayBuilder m_ActiveRequest;

        protected override void OnInitialize(object userData)
        {
            m_AdUnitId = userData as string;
            IsInitialized = !string.IsNullOrEmpty(m_AdUnitId);

            if (!IsInitialized)
            {
                Log.Error("TTRewardAD", "初始化失败：广告位 ID 为空");
                return;
            }

            CreateAdInstance();
        }

        protected override void OnPlay(RewardAdPlayBuilder request)
        {
            if (!IsInitialized || m_Ad == null)
            {
                FinishWithError(request, -1, "激励视频未初始化");
                return;
            }

            m_ActiveRequest = request;
            InvokeLoaded(request);
            m_Ad.Show();
        }

        protected override void DestroyAd()
        {
            m_Ad?.Destroy();
            m_Ad = null;
            m_ActiveRequest = null;
        }

        private void CreateAdInstance()
        {
            DestroyAd();
            m_Ad = TT.CreateRewardedVideoAd(m_AdUnitId, OnNativeClose, OnNativeError);
        }

        private void OnNativeClose(bool isEnded, int closeCode)
        {
            Log.Info("TTRewardAD", $"Close isEnded={isEnded}, closeCode={closeCode}");

            if (isEnded)
            {
                DouYinInterstitialAdPolicy.RecordRewardedShown();
            }

            var request = m_ActiveRequest;
            m_ActiveRequest = null;

            if (request == null)
            {
                return;
            }

            InvokeComplete(request, isEnded);
        }

        private void OnNativeError(int errCode, string errMsg)
        {
            Log.Error("TTRewardAD", $"Error {errCode}: {errMsg}");

            var request = m_ActiveRequest;
            m_ActiveRequest = null;

            if (request == null)
            {
                return;
            }

            FinishWithError(request, errCode, errMsg);
        }
    }
}
#endif
