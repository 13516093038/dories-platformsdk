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
                Log.Error("TTInterstitial", "初始化失败：广告位 ID 为空");
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

            if (!m_Ad.IsLoaded())
            {
                m_Ad.Load();
            }

            InvokeLoaded(request);
            m_Ad.Show();
        }

        protected override void DestroyAd()
        {
            UnsubscribeAdEvents();
            m_Ad?.Destroy();
            m_Ad = null;
            m_ActiveRequest = null;
        }

        private void CreateAdInstance()
        {
            DestroyAd();

            var param = new CreateInterstitialAdParam
            {
                InterstitialAdId = m_AdUnitId
            };
            m_Ad = TT.CreateInterstitialAd(param);
            SubscribeAdEvents();
            m_Ad.Load();
        }

        private void SubscribeAdEvents()
        {
            if (m_Ad == null)
            {
                return;
            }

            m_Ad.OnLoad += OnAdLoad;
            m_Ad.OnError += OnAdError;
            m_Ad.OnClose += OnAdClose;
        }

        private void UnsubscribeAdEvents()
        {
            if (m_Ad == null)
            {
                return;
            }

            m_Ad.OnLoad -= OnAdLoad;
            m_Ad.OnError -= OnAdError;
            m_Ad.OnClose -= OnAdClose;
        }

        private void OnAdLoad()
        {
            Log.Info("TTInterstitial", "Load");
        }

        private void OnAdError(int errCode, string errMsg)
        {
            Log.Error("TTInterstitial", $"Error {errCode}: {errMsg}");

            var request = m_ActiveRequest;
            m_ActiveRequest = null;

            if (request == null)
            {
                return;
            }

            FinishWithError(request, errCode, errMsg);
        }

        private void OnAdClose()
        {
            Log.Info("TTInterstitial", "Close");
            DouYinInterstitialAdPolicy.RecordInterstitialShown();

            var request = m_ActiveRequest;
            m_ActiveRequest = null;

            if (request == null)
            {
                return;
            }

            InvokeComplete(request, true);
        }
    }
}
#endif
