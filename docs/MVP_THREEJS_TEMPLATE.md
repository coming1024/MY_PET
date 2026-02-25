# MY_PET MVP Three.js 项目目录模板

> 目标：单宠物 + 第一人称视角 + 可运行宠物行为/情绪建模，后续可扩展多宠物。

## 1) 推荐目录结构

```text
MY_PET/
├─ index.html
├─ src/
│  ├─ main.js
│  ├─ render/
│  │  ├─ SceneBootstrap.js
│  │  └─ FirstPersonController.js
│  ├─ core/
│  │  ├─ PetNeedsSystem.js
│  │  ├─ PetEmotionModel.js
│  │  └─ PetMemory.js
│  ├─ ai/
│  │  ├─ PetStateMachine.js
│  │  └─ states/
│  │     ├─ IdleState.js
│  │     ├─ SeekUserState.js
│  │     ├─ EatState.js
│  │     ├─ SleepState.js
│  │     └─ PlayState.js
│  ├─ input/
│  │  └─ UserInteractionController.js
│  └─ data/
│     └─ PetConfig.js
├─ public/
│  ├─ models/
│  ├─ textures/
│  └─ audio/
└─ docs/
   ├─ MVP_THREEJS_TEMPLATE.md
   ├─ CORE_CLASS_DESIGN_THREEJS.md
   ├─ GAMEPLAY_SPEC.md
   └─ AI_MODEL_SPEC.md
```

## 2) 分层约定

- `render/`: 只负责渲染、场景、相机和可视化更新。
- `core/`: 宠物需求、成长、情绪等可复用模型。
- `ai/`: 决策层（状态机/行为树），输入是 core 数据，输出是行为指令。
- `input/`: 用户输入和 UI 事件桥接。
- `data/`: 参数配置常量。

## 3) MVP 场景最小元素

- 第一人称相机（Pointer Lock）。
- 一个宠物 Mesh（初始可用 `SphereGeometry` 占位）。
- 一个食盆交互点、一个玩具交互点（可先用 invisible trigger）。
- 调试 HUD：显示 hunger/energy/mood/bond/currentState。

## 4) MVP 验收标准

- 用户输入可触发：喂食、抚摸、呼唤、玩耍。
- 宠物需求值随时间变化，并触发状态切换。
- 宠物至少具备 4 种状态：Idle / Eat / Sleep / SeekUser。
- 一次浏览器运行内可稳定更新循环（无明显卡顿或状态抖动）。
