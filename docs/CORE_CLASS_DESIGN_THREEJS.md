# 核心 JavaScript 类设计（字段与方法签名）

## PetNeedsSystem

职责：维护宠物需求值、成长值与交互反馈。

### 关键字段
- `petId: string`
- `ageDays: number`
- `lifeStage: 'baby' | 'adult'`
- `hunger: number`
- `energy: number`
- `mood: number`
- `bond: number`
- `socialNeed: number`

### 关键方法签名
- `update(dt: number): void`
- `feed(amount = 30): void`
- `pet(amount = 10): void`
- `play(amount = 12): void`
- `sleep(amount = 20): void`
- `getSnapshot(): PetNeedsSnapshot`
- `applySnapshot(snapshot: PetNeedsSnapshot): void`

---

## PetStateMachine

职责：根据需求值 + 输入事件决定宠物行为并输出位移目标。

### 关键字段
- `needs: PetNeedsSystem`
- `petObject: THREE.Object3D`
- `userObject: THREE.Object3D`
- `currentState: PetBehaviorType`
- `interactionFlags: { feed, pet, call, play }`

### 关键方法签名
- `update(dt: number): void`
- `signalFeed(): void`
- `signalPet(): void`
- `signalCall(): void`
- `signalPlay(): void`
- `consumeFlags(): void`

---

## UserInteractionController

职责：键盘/UI -> 行为信号桥接。

### 关键方法签名
- `bind(): void`
- `dispose(): void`
- `onFeedPressed(): void`
- `onPetPressed(): void`
- `onCallPressed(): void`
- `onPlayPressed(): void`

