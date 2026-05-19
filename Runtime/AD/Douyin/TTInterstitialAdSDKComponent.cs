#if UNITY_WEBGL && DOUYINMINIGAME
using TTSDK;
using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 抖音插屏广告，对应 tt.createInterstitialAd / InterstitialAd。
    /// https://developer.open-douyin.com/docs/resource/zh-CN/mini-game/develop/api/javascript-api/ads/interstitial-ad/interstitial-ad
    /// </summary>
    public class TTInterstitialAdSDKComponent : InterstitialAdSDKComponent
    {
        /// <summary>是否启用抖音插屏频率限制。</summary>
        public bool EnableFrequencyPolicy { get; set; } = true;

        private string m_AdUnitId;
        private TTInterstitialAd m_Ad;
        private InterstitialAdPlayBuilder m_ActiveRequest;

        protected override void OnInitialize(object userData)
        {
            m_AdUnitId = userData as string;
            IsInitialized = !string.IsNullOrEmpty(m_AdUnitId);

            if (!IsInitialized)
            {
                Debug.LogError("[TTInterstitial] 初始化失败：广告位 ID 为空");
                return;
            }

            CreateAdInstance();
        }

        protected override void OnPlay(InterstitialAdPlayBuilder request)
        {
            if (!IsInitialized || m_Ad == null)
            {
                FinishWithError(request, -1, "插屏广告未初始化");
                return;
            }

            if (EnableFrequencyPolicy && !DouYinInterstitialAdPolicy.CanShow())
            {
                var reason = DouYinInterstitialAdPolicy.GetBlockReason();
                FinishWithError(request, -1, reason ?? "频率限制");
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
            m_Ad = TT.CreateInterstitialAd(m_AdUnitId, OnNativeClose, OnNativeError);
        }

        private void OnNativeClose()
        {
            Debug.Log("[TTInterstitial] Close");
            DouYinInterstitialAdPolicy.RecordInterstitialShown();

            var request = m_ActiveRequest;
            m_ActiveRequest = null;

            if (request == null)
            {
                return;
            }

            InvokeComplete(request, true);
        }

        private void OnNativeError(int errCode, string errMsg)
        {
            Debug.LogError($"[TTInterstitial] Error {errCode}: {errMsg}");

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
