using UnityEngine;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 插屏广告展示频率策略（参考抖音插屏广告注意事项）。
    /// https://developer.open-douyin.com/docs/resource/zh-CN/mini-game/develop/api/javascript-api/ads/interstitial-ad/interstitial-ad-notice
    /// </summary>
    public static class DouYinInterstitialAdPolicy
    {
        /// <summary>启动后多少秒内不可展示插屏。</summary>
        public const float DefaultLaunchDelaySeconds = 30f;

        /// <summary>两次插屏之间的最小间隔（秒）。</summary>
        public const float DefaultMinIntervalSeconds = 60f;

        private static float s_LaunchRealtime;
        private static float s_LastInterstitialShowRealtime = -9999f;
        private static float s_LastRewardedShowRealtime = -9999f;

        static DouYinInterstitialAdPolicy()
        {
            ResetLaunchTime();
        }

        public static void ResetLaunchTime()
        {
            s_LaunchRealtime = Time.realtimeSinceStartup;
        }

        public static void RecordInterstitialShown()
        {
            s_LastInterstitialShowRealtime = Time.realtimeSinceStartup;
        }

        public static void RecordRewardedShown()
        {
            s_LastRewardedShowRealtime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// 当前是否允许展示插屏（不含业务层自定义条件）。
        /// </summary>
        public static bool CanShow(
            float launchDelaySeconds = DefaultLaunchDelaySeconds,
            float minIntervalSeconds = DefaultMinIntervalSeconds)
        {
            var now = Time.realtimeSinceStartup;

            if (now - s_LaunchRealtime < launchDelaySeconds)
            {
                return false;
            }

            if (now - s_LastInterstitialShowRealtime < minIntervalSeconds)
            {
                return false;
            }

            // 激励视频与插屏之间也需间隔
            if (now - s_LastRewardedShowRealtime < minIntervalSeconds)
            {
                return false;
            }

            return true;
        }

        public static string GetBlockReason(
            float launchDelaySeconds = DefaultLaunchDelaySeconds,
            float minIntervalSeconds = DefaultMinIntervalSeconds)
        {
            var now = Time.realtimeSinceStartup;

            if (now - s_LaunchRealtime < launchDelaySeconds)
            {
                return "启动后冷却时间内不可展示插屏";
            }

            if (now - s_LastInterstitialShowRealtime < minIntervalSeconds)
            {
                return "距上次插屏展示间隔不足";
            }

            if (now - s_LastRewardedShowRealtime < minIntervalSeconds)
            {
                return "距上次激励视频展示间隔不足";
            }

            return null;
        }
    }
}
