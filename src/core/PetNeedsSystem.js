export class PetNeedsSystem {
  constructor() {
    this.petId = 'pet_001';
    this.ageDays = 1;
    this.lifeStage = 'baby';

    this.hunger = 25;
    this.energy = 80;
    this.mood = 70;
    this.bond = 40;
    this.socialNeed = 30;

    this._growthTimer = 0;
    this._growthSecondsPerDay = 120;
  }

  update(dt) {
    this.hunger = clamp(this.hunger + 0.35 * dt);
    this.energy = clamp(this.energy - 0.2 * dt);
    this.mood = clamp(this.mood - 0.08 * dt);
    this.socialNeed = clamp(this.socialNeed + 0.15 * dt);

    if (this.hunger > 80 || this.energy < 20) {
      this.mood = clamp(this.mood - 0.15 * dt);
    }

    this._growthTimer += dt;
    if (this._growthTimer >= this._growthSecondsPerDay) {
      this._growthTimer = 0;
      this.ageDays += 1;
      this.lifeStage = this.ageDays >= 7 ? 'adult' : 'baby';
    }
  }

  feed(amount = 30) {
    this.hunger = clamp(this.hunger - amount);
    this.mood = clamp(this.mood + 8);
    this.bond = clamp(this.bond + 2);
  }

  pet(amount = 10) {
    this.mood = clamp(this.mood + amount);
    this.bond = clamp(this.bond + 3);
    this.socialNeed = clamp(this.socialNeed - 6);
  }

  play(amount = 12) {
    this.mood = clamp(this.mood + amount);
    this.energy = clamp(this.energy - 8);
    this.socialNeed = clamp(this.socialNeed - 10);
    this.bond = clamp(this.bond + 4);
  }

  sleep(amount = 20) {
    this.energy = clamp(this.energy + amount);
    this.mood = clamp(this.mood + 2);
  }

  getSnapshot() {
    return {
      petId: this.petId,
      ageDays: this.ageDays,
      lifeStage: this.lifeStage,
      hunger: this.hunger,
      energy: this.energy,
      mood: this.mood,
      bond: this.bond,
      socialNeed: this.socialNeed,
    };
  }

  applySnapshot(snapshot) {
    this.petId = snapshot.petId ?? this.petId;
    this.ageDays = snapshot.ageDays ?? this.ageDays;
    this.lifeStage = snapshot.lifeStage ?? this.lifeStage;
    this.hunger = clamp(snapshot.hunger ?? this.hunger);
    this.energy = clamp(snapshot.energy ?? this.energy);
    this.mood = clamp(snapshot.mood ?? this.mood);
    this.bond = clamp(snapshot.bond ?? this.bond);
    this.socialNeed = clamp(snapshot.socialNeed ?? this.socialNeed);
  }
}

function clamp(v) {
  return Math.max(0, Math.min(100, v));
}
