using System;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 插屏广告播放请求（建造者），通过链式调用配置回调后 <see cref="Show"/> 执行。
    /// </summary>
    public class InterstitialAdPlayBuilder
    {
        private readonly InterstitialAdSDKComponent m_Owner;

        internal Action LoadedCallback { get; private set; }
        internal Action<AdErrorEventArgs> ErrorCallback { get; private set; }
        internal Action<bool> CompleteCallback { get; private set; }

        internal InterstitialAdPlayBuilder(InterstitialAdSDKComponent owner)
        {
            m_Owner = owner;
        }

        public InterstitialAdPlayBuilder OnLoaded(Action onLoaded)
        {
            LoadedCallback = onLoaded;
            return this;
        }

        public InterstitialAdPlayBuilder OnError(Action<AdErrorEventArgs> onError)
        {
            ErrorCallback = onError;
            return this;
        }

        public InterstitialAdPlayBuilder OnError(Action<int, string> onError)
        {
            ErrorCallback = args => onError?.Invoke(args.ErrorCode, args.ErrorMessage);
            return this;
        }

        /// <summary>展示结束，参数表示是否正常关闭（非错误中断）。</summary>
        public InterstitialAdPlayBuilder OnComplete(Action<bool> onComplete)
        {
            CompleteCallback = onComplete;
            return this;
        }

        public void Show()
        {
            m_Owner.ExecutePlay(this);
        }
    }
}
