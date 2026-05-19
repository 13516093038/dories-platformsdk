# Dories Platform SDK

UPM 包：小游戏平台 SDK 封装（激励视频、插屏广告等）。

## 依赖

- `com.dories.componentization`

## 抖音小游戏

- 需在 Unity 中安装 **TTSDK**（ByteGame 开发者工具），并启用 `DOUYINMINIGAME` 宏（WebGL 平台）。
- 在场景挂载 `ZYPlatformSDKSystem`，于 Inspector 配置广告位 ID。

## 使用示例

```csharp
// 激励视频
sdk.RewardAd.Play()
    .OnComplete(success => { if (success) { /* 发奖励 */ } })
    .Show();

// 插屏
sdk.InterstitialAd.Play()
    .OnComplete(success => { })
    .Show();
```

## 发布

见聚合仓 `docs/UPM-Git-Release.md` 中 `com.dories.platformsdk` 小节。
