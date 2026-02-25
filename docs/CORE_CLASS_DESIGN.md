# 核心 C# 类设计（字段与方法签名）

## PetNeedsSystem (MonoBehaviour)

职责：维护宠物需求、成长、基础事件响应。

### 关键字段
- `string petId`
- `int ageDays`
- `PetLifeStage lifeStage`
- `float hunger`
- `float energy`
- `float mood`
- `float bond`
- `float socialNeed`

### 关键方法签名
- `public void TickNeeds(float dt)`
- `public void TickGrowth(float dt)`
- `public void Feed(float amount = 30f)`
- `public void Petting(float amount = 10f)`
- `public void Play(float amount = 12f)`
- `public void Sleep(float amount = 20f)`
- `public PetNeedsSnapshot GetSnapshot()`
- `public void ApplySnapshot(PetNeedsSnapshot snapshot)`

---

## PetStateMachine (MonoBehaviour)

职责：根据需求值和用户交互信号切换行为状态。

### 关键字段
- `PetNeedsSystem needsSystem`
- `Transform userTransform`
- `float moveSpeed`
- `float stopDistance`
- `PetBehaviorType currentBehavior`

### 关键方法签名
- `public void SignalFeed()`
- `public void SignalPlay()`
- `public void SignalCall()`
- `public void SignalPetting()`
- `public void ConsumeInteractionFlags()`

---

## IPetState / PetStateBase

职责：定义行为状态统一生命周期。

### 方法签名
- `void Enter()`
- `void Tick(float dt)`
- `void Exit()`

### 状态实现（MVP）
- `IdleState`
- `SeekUserState`
- `EatState`
- `SleepState`
- `PlayState`

---

## UserInteractionController (MonoBehaviour)

职责：输入桥接（按钮/按键 -> AI 信号）。

### 方法签名
- `public void OnFeedPressed()`
- `public void OnPetPressed()`
- `public void OnCallPressed()`
- `public void OnPlayPressed()`

