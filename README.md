# MY_PET

Three.js first-person pet game MVP prototype.

## MVP bootstrap assets (Three.js)

- Three.js project folder template: `docs/MVP_THREEJS_TEMPLATE.md`
- Core class design: `docs/CORE_CLASS_DESIGN_THREEJS.md`
- Runnable behavior state machine skeleton scripts:
  - `src/core/PetNeedsSystem.js`
  - `src/ai/PetStateMachine.js`
  - `src/input/UserInteractionController.js`
  - `src/main.js`

## Quick start

1. Serve this folder as a static website (any local HTTP server).
2. Open `index.html` in browser via `http://localhost:<port>`.
3. Use keyboard:
   - `W/A/S/D`: move camera
   - `Mouse`: look around
   - `F`: feed
   - `P`: pet
   - `C`: call
   - `G`: play

## Notes

- Current MVP is intentionally simple: one pet, first-person view, state-machine behavior loop.
- Multi-pet support is prepared by `socialNeed` and clear AI/input separation.
