# MY_PET

Raising a pet game prototype.

## MVP bootstrap assets

- Unity project folder template: `docs/MVP_UNITY_TEMPLATE.md`
- Core class design: `docs/CORE_CLASS_DESIGN.md`
- Runnable behavior state machine skeleton scripts:
  - `Assets/Scripts/Pet/PetNeedsSystem.cs`
  - `Assets/Scripts/AI/PetStateMachine.cs`
  - `Assets/Scripts/Input/UserInteractionController.cs`

## Quick start in Unity

1. Create/open a Unity 2022/2023 LTS project.
2. Copy this repository's `Assets/Scripts` folder into your Unity project's `Assets`.
3. Create a pet GameObject and add:
   - `PetNeedsSystem`
   - `PetStateMachine`
4. Create a player object and assign its transform to `PetStateMachine.userTransform`.
5. Add `UserInteractionController` to any scene object.
6. Press Play and use:
   - `F` Feed
   - `P` Pet
   - `C` Call
   - `G` Play
