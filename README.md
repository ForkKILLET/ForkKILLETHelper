# ForkKILLETHelper

[![release](https://img.shields.io/github/v/release/ForkKILLET/ForkKILLETHelper)](https://github.com/ForkKILLET/ForkKILLETHelper/releases)

[![game banana](https://gamebanana.com/mods/embeddables/424995?type=large)](https://gamebanana.com/mods/424995)

## 机制

### 上瘾

玩家触发上瘾状态后 1.6 秒死亡，再次触发可以重置死亡计时器。切板会清除上瘾状态。

## 实体

### `AddictingBlock` 上瘾方块

会触发上瘾状态的方块。

- `triggerMethod` 触发方式：抓左侧 / 抓右侧 / 站在上面三种方式组合，可以触发的边显示为粉红色。

### `AddictingRefill` 上瘾水晶

会触发上瘾状态的水晶。
除了低体力、冲刺数不足时，上瘾状态时也能吃。

- `wave` 是否浮动
- `oneUse` 是否一次性
- `oneTimeSatisfication` 是否只能满足一次：是则吃一次后变成普通水晶。
- `respawnTime` 回复时间

### `NoAddictingStrawberry` 不上瘾草莓

只要会话内进入过上瘾状态，不上瘾草莓就会飞走。

## 地图

### `TestMap` 测试地图

#### `Addiction Demo` 用于测试上瘾相关实体

## 感谢

- 蔚蓝MiaoNet 群的群友，他们提供了很多帮助！（我第一次写 C#）
- [CommunalHelper/ShroomHelper](https://github.com/CommunalHelper/ShroomHelper/blob/dev/Code/Entities/OneDashWingedStrawberry.cs)，不上瘾草莓的很多代码来自于这个 helper 的单冲草莓的代码！
- [Everest Wiki](https://github.com/EverestAPI/Resources/wiki)

