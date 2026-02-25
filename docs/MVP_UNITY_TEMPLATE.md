# MY_PET MVP Unity 项目目录模板

> 目标：单宠物 + 第一人称视角 + 可运行宠物行为/情绪建模。后续可扩展到多宠物。

## 1) 推荐目录结构

```text
MY_PET/
├─ Assets/
│  ├─ Art/
│  │  ├─ Models/
│  │  ├─ Materials/
│  │  ├─ Textures/
│  │  └─ Animations/
│  ├─ Audio/
│  │  ├─ BGM/
│  │  └─ SFX/
│  ├─ Prefabs/
│  │  ├─ Player/
│  │  ├─ Pet/
│  │  └─ Environment/
│  ├─ Scenes/
│  │  ├─ Bootstrap.unity
│  │  └─ HomeRoom.unity
│  ├─ ScriptableObjects/
│  │  └─ Pet/
│  ├─ UI/
│  └─ Scripts/
│     ├─ Core/
│     │  ├─ GameBootstrap.cs
│     │  └─ TimeService.cs
│     ├─ Input/
│     │  └─ UserInteractionController.cs
│     ├─ Pet/
│     │  ├─ PetNeedsSystem.cs
│     │  ├─ PetMemory.cs
│     │  └─ PetEmotionModel.cs
│     └─ AI/
│        ├─ PetStateMachine.cs
│        └─ States/
│           ├─ IPetState.cs
│           ├─ IdleState.cs
│           ├─ SeekUserState.cs
│           ├─ EatState.cs
│           ├─ SleepState.cs
│           └─ PlayState.cs
├─ Packages/
├─ ProjectSettings/
└─ docs/
   ├─ MVP_UNITY_TEMPLATE.md
   ├─ GAMEPLAY_SPEC.md
   └─ AI_MODEL_SPEC.md
```

## 2) 命名与分层约定（MVP）

- Core: 全局服务与启动流程。
- Input: 用户输入，统一转化为“交互事件”。
- Pet: 宠物状态数据、需求衰减、情绪计算。
- AI: 决策层（状态机/行为树），只消费数据，不直接采集输入。
- ScriptableObjects: 参数配置（便于策划调参，不改代码）。

## 3) 场景最小依赖

- 一个 First Person 玩家预制体。
- 一个 Pet 预制体（挂载 `PetNeedsSystem` + `PetStateMachine`）。
- 一个可交互食盆（喂食触发源）。
- 一个可交互玩具（玩耍触发源）。
- 一个 UI 调试面板（显示 hunger/energy/mood/bond）。

## 4) MVP 验收标准（建议）

- 用户可通过输入触发：喂食、抚摸、呼唤、玩耍。
- 宠物需求值会随时间变化并影响状态切换。
- 宠物可在至少 4 种状态间切换：Idle / Eat / Sleep / SeekUser（可加 Play）。
- 关闭并重启应用后，宠物基础状态可恢复（本地存档）。

