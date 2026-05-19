using Dories.Componentization.Runtime;
using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 平台 SDK 入口：在 Awake 中按平台初始化广告 Component，播放接口各平台一致。
    /// </summary>
    public class ZYPlatformSDKSystem : EntityMono
    {
        [SerializeField, Header("抖音激励视频广告位ID")]
        private string _rewardAdUnitIdDouYin;

        [SerializeField, Header("抖音插屏广告位ID")]
        private string _interstitialAdUnitIdDouYin;

        private RewardADSDKComponent _rewardAd;
        private InterstitialAdSDKComponent _interstitialAd;

        public RewardADSDKComponent RewardAd => _rewardAd;
        public InterstitialAdSDKComponent InterstitialAd => _interstitialAd;

        private void Awake()
        {
#if UNITY_EDITOR
            _rewardAd = AddComponent<MockRewardADSDKComponent>();
            _interstitialAd = AddComponent<MockInterstitialAdSDKComponent>();
#elif UNITY_WEBGL && DOUYINMINIGAME
            _rewardAd = AddComponent<TTRewardADSDKComponent>(_rewardAdUnitIdDouYin);
            _interstitialAd = AddComponent<TTInterstitialAdSDKComponent>(_interstitialAdUnitIdDouYin);
#endif
        }
    }
}
