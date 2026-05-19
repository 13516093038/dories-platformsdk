namespace ZyPlatformSDKSystem.Runtime.AD
{
    /// <summary>
    /// 广告错误信息，对应抖音 onError 回调参数。
    /// </summary>
    public readonly struct AdErrorEventArgs
    {
        public int ErrorCode { get; }
        public string ErrorMessage { get; }

        public AdErrorEventArgs(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
