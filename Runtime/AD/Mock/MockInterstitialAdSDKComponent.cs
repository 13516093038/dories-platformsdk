using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 编辑器 / 非抖音环境下的插屏 Mock。
    /// </summary>
    public class MockInterstitialAdSDKComponent : InterstitialAdSDKComponent
    {
        protected override void OnPlay(InterstitialAdPlayBuilder request)
        {
            Debug.Log("[MockInterstitial] Show");
            InvokeLoaded(request);
            InvokeComplete(request, true);
        }
    }
}
