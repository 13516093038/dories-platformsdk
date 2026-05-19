using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 编辑器 / 非抖音环境下的激励视频 Mock，默认模拟完整观看。
    /// </summary>
    public class MockRewardADSDKComponent : RewardADSDKComponent
    {
        private bool m_SimulateWatchComplete = true;

        public bool SimulateWatchComplete
        {
            get => m_SimulateWatchComplete;
            set => m_SimulateWatchComplete = value;
        }

        protected override void OnPlay(RewardAdPlayBuilder request)
        {
            Debug.Log($"[MockRewardAD] Play simulateComplete={m_SimulateWatchComplete}");
            InvokeLoaded(request);
            InvokeComplete(request, m_SimulateWatchComplete);
        }
    }
}
