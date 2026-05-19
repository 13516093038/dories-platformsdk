using Dories.Componentization.Runtime;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 激励视频广告基类：子类在 <see cref="OnInitialize"/> 中完成平台初始化，通过 <see cref="Play"/> 播放。
    /// </summary>
    public abstract class RewardADSDKComponent : Component
    {
        public bool IsInitialized { get; protected set; }

        protected override void OnAcquire(object userData = null)
        {
            base.OnAcquire(userData);
            OnInitialize(userData);
        }

        /// <summary>平台相关初始化，由 <see cref="ZYPlatformSDKSystem"/> 或工厂传入配置。</summary>
        protected virtual void OnInitialize(object userData)
        {
            IsInitialized = true;
        }

        public RewardAdPlayBuilder Play() => new RewardAdPlayBuilder(this);

        internal void ExecutePlay(RewardAdPlayBuilder request) => OnPlay(request);

        protected abstract void OnPlay(RewardAdPlayBuilder request);

        public override void Dispose()
        {
            DestroyAd();
            base.Dispose();
        }

        protected virtual void DestroyAd()
        {
        }

        protected static void InvokeLoaded(RewardAdPlayBuilder request)
        {
            request.LoadedCallback?.Invoke();
        }

        protected static void InvokeError(RewardAdPlayBuilder request, int errorCode, string errorMessage)
        {
            request.ErrorCallback?.Invoke(new AdErrorEventArgs(errorCode, errorMessage));
        }

        protected static void InvokeComplete(RewardAdPlayBuilder request, bool isEnded)
        {
            request.CompleteCallback?.Invoke(isEnded);
        }

        protected static void FinishWithError(RewardAdPlayBuilder request, int errorCode, string errorMessage)
        {
            InvokeError(request, errorCode, errorMessage);
            InvokeComplete(request, false);
        }
    }
}
