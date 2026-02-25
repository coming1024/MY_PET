export class UserInteractionController {
  constructor(stateMachine) {
    this.stateMachine = stateMachine;
    this._onKeyDown = this._onKeyDown.bind(this);
  }

  bind() {
    window.addEventListener('keydown', this._onKeyDown);
  }

  dispose() {
    window.removeEventListener('keydown', this._onKeyDown);
  }

  onFeedPressed() {
    this.stateMachine.signalFeed();
  }

  onPetPressed() {
    this.stateMachine.signalPet();
  }

  onCallPressed() {
    this.stateMachine.signalCall();
  }

  onPlayPressed() {
    this.stateMachine.signalPlay();
  }

  _onKeyDown(event) {
    const key = event.key.toLowerCase();
    if (key === 'f') this.onFeedPressed();
    if (key === 'p') this.onPetPressed();
    if (key === 'c') this.onCallPressed();
    if (key === 'g') this.onPlayPressed();
  }
}
