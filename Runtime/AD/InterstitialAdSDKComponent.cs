using Dories.Componentization.Runtime;

namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 插屏广告基类：子类在 <see cref="OnInitialize"/> 中完成平台初始化，通过 <see cref="Play"/> 展示。
    /// </summary>
    public abstract class InterstitialAdSDKComponent : Component
    {
        public bool IsInitialized { get; protected set; }

        protected override void OnAcquire(object userData = null)
        {
            base.OnAcquire(userData);
            OnInitialize(userData);
        }

        protected virtual void OnInitialize(object userData)
        {
            IsInitialized = true;
        }

        public InterstitialAdPlayBuilder Play() => new InterstitialAdPlayBuilder(this);

        internal void ExecutePlay(InterstitialAdPlayBuilder request) => OnPlay(request);

        protected abstract void OnPlay(InterstitialAdPlayBuilder request);

        public override void Dispose()
        {
            DestroyAd();
            base.Dispose();
        }

        protected virtual void DestroyAd()
        {
        }

        protected static void InvokeLoaded(InterstitialAdPlayBuilder request)
        {
            request.LoadedCallback?.Invoke();
        }

        protected static void InvokeError(InterstitialAdPlayBuilder request, int errorCode, string errorMessage)
        {
            request.ErrorCallback?.Invoke(new AdErrorEventArgs(errorCode, errorMessage));
        }

        protected static void InvokeComplete(InterstitialAdPlayBuilder request, bool success)
        {
            request.CompleteCallback?.Invoke(success);
        }

        protected static void FinishWithError(InterstitialAdPlayBuilder request, int errorCode, string errorMessage)
        {
            InvokeError(request, errorCode, errorMessage);
            InvokeComplete(request, false);
        }
    }
}
