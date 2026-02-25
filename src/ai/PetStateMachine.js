import * as THREE from 'https://unpkg.com/three@0.160.0/build/three.module.js';

export const PetBehaviorType = Object.freeze({
  Idle: 'Idle',
  SeekUser: 'SeekUser',
  Eat: 'Eat',
  Sleep: 'Sleep',
  Play: 'Play',
});

export class PetStateMachine {
  constructor({ needs, petObject, userObject, moveSpeed = 1.9, stopDistance = 1.4 }) {
    this.needs = needs;
    this.petObject = petObject;
    this.userObject = userObject;
    this.moveSpeed = moveSpeed;
    this.stopDistance = stopDistance;

    this.currentState = PetBehaviorType.Idle;
    this.interactionFlags = { feed: false, pet: false, call: false, play: false };

    this._eatTimer = 0;
    this._playTimer = 0;
  }

  signalFeed() { this.interactionFlags.feed = true; }
  signalPet() { this.interactionFlags.pet = true; }
  signalCall() { this.interactionFlags.call = true; }
  signalPlay() { this.interactionFlags.play = true; }

  consumeFlags() {
    this.interactionFlags.feed = false;
    this.interactionFlags.pet = false;
    this.interactionFlags.call = false;
    this.interactionFlags.play = false;
  }

  update(dt) {
    const next = this._decideNextState();
    if (next !== this.currentState) {
      this.currentState = next;
      this._eatTimer = 0;
      this._playTimer = 0;
    }

    switch (this.currentState) {
      case PetBehaviorType.Eat:
        this._eatTimer += dt;
        if (this._eatTimer >= 1.2) {
          this.needs.feed(28);
          this.consumeFlags();
          this._eatTimer = 0;
        }
        break;
      case PetBehaviorType.Sleep:
        this.needs.sleep(12 * dt);
        break;
      case PetBehaviorType.Play:
        this._playTimer += dt;
        if (this._playTimer >= 0.8) {
          this.needs.play(6);
          this.consumeFlags();
          this._playTimer = 0;
        }
        break;
      case PetBehaviorType.SeekUser:
        this._seekUser(dt);
        break;
      case PetBehaviorType.Idle:
      default:
        if (this.interactionFlags.pet) {
          this.needs.pet(8);
          this.consumeFlags();
        }
        break;
    }
  }

  _decideNextState() {
    if (this.interactionFlags.feed || this.needs.hunger > 70) return PetBehaviorType.Eat;
    if (this.needs.energy < 20) return PetBehaviorType.Sleep;
    if (this.interactionFlags.play) return PetBehaviorType.Play;
    if (this.interactionFlags.call || this.needs.socialNeed > 60) return PetBehaviorType.SeekUser;
    return PetBehaviorType.Idle;
  }

  _seekUser(dt) {
    if (!this.userObject) return;

    const delta = new THREE.Vector3().subVectors(this.userObject.position, this.petObject.position);
    const distance = delta.length();

    if (distance > this.stopDistance) {
      const dir = delta.normalize();
      this.petObject.position.addScaledVector(dir, this.moveSpeed * dt);
      const lookTarget = new THREE.Vector3(this.userObject.position.x, this.petObject.position.y, this.userObject.position.z);
      this.petObject.lookAt(lookTarget);
    } else {
      this.needs.pet(2);
      this.consumeFlags();
    }
  }
}
