using System;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 激励视频播放请求（建造者），通过链式调用配置回调后 <see cref="Show"/> 执行。
    /// </summary>
    public class RewardAdPlayBuilder
    {
        private readonly RewardADSDKComponent m_Owner;

        internal Action LoadedCallback { get; private set; }
        internal Action<AdErrorEventArgs> ErrorCallback { get; private set; }
        internal Action<bool> CompleteCallback { get; private set; }

        internal RewardAdPlayBuilder(RewardADSDKComponent owner)
        {
            m_Owner = owner;
        }

        /// <summary>
        /// 广告加载完成（可选）。
        /// </summary>
        public RewardAdPlayBuilder OnLoaded(Action onLoaded)
        {
            LoadedCallback = onLoaded;
            return this;
        }

        /// <summary>
        /// 播放出错（可选）。
        /// </summary>
        public RewardAdPlayBuilder OnError(Action<AdErrorEventArgs> onError)
        {
            ErrorCallback = onError;
            return this;
        }

        /// <summary>
        /// 播放出错（可选）。
        /// </summary>
        public RewardAdPlayBuilder OnError(Action<int, string> onError)
        {
            ErrorCallback = args => onError?.Invoke(args.ErrorCode, args.ErrorMessage);
            return this;
        }

        /// <summary>
        /// 广告关闭，参数为是否完整观看。
        /// </summary>
        public RewardAdPlayBuilder OnComplete(Action<bool> onComplete)
        {
            CompleteCallback = onComplete;
            return this;
        }

        /// <summary>
        /// 开始播放。
        /// </summary>
        public void Show()
        {
            m_Owner.ExecutePlay(this);
        }
    }
}
