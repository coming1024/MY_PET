import * as THREE from 'https://unpkg.com/three@0.160.0/build/three.module.js';
import { PointerLockControls } from 'https://unpkg.com/three@0.160.0/examples/jsm/controls/PointerLockControls.js';
import { PetNeedsSystem } from './core/PetNeedsSystem.js';
import { PetStateMachine } from './ai/PetStateMachine.js';
import { UserInteractionController } from './input/UserInteractionController.js';

const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

const scene = new THREE.Scene();
scene.background = new THREE.Color(0x1a2330);

const camera = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 0.1, 200);
camera.position.set(0, 1.6, 5);

const controls = new PointerLockControls(camera, document.body);
document.body.addEventListener('click', () => controls.lock());

const light = new THREE.HemisphereLight(0xffffff, 0x334455, 1.2);
scene.add(light);
const dir = new THREE.DirectionalLight(0xffffff, 1.1);
dir.position.set(3, 6, 2);
scene.add(dir);

const ground = new THREE.Mesh(
  new THREE.PlaneGeometry(40, 40),
  new THREE.MeshStandardMaterial({ color: 0x2a3a2a })
);
ground.rotation.x = -Math.PI / 2;
scene.add(ground);

const petObject = new THREE.Mesh(
  new THREE.SphereGeometry(0.35, 24, 24),
  new THREE.MeshStandardMaterial({ color: 0xf3c778 })
);
petObject.position.set(0, 0.35, 0);
scene.add(petObject);

const needs = new PetNeedsSystem();
const stateMachine = new PetStateMachine({ needs, petObject, userObject: camera });
const input = new UserInteractionController(stateMachine);
input.bind();

const move = { forward: false, backward: false, left: false, right: false };
window.addEventListener('keydown', (e) => {
  const k = e.key.toLowerCase();
  if (k === 'w') move.forward = true;
  if (k === 's') move.backward = true;
  if (k === 'a') move.left = true;
  if (k === 'd') move.right = true;
});
window.addEventListener('keyup', (e) => {
  const k = e.key.toLowerCase();
  if (k === 'w') move.forward = false;
  if (k === 's') move.backward = false;
  if (k === 'a') move.left = false;
  if (k === 'd') move.right = false;
});

const clock = new THREE.Clock();
const velocity = new THREE.Vector3();
const hud = document.getElementById('hud');

function animate() {
  requestAnimationFrame(animate);
  const dt = Math.min(clock.getDelta(), 0.033);

  const speed = 3.5;
  velocity.set(0, 0, 0);
  if (move.forward) velocity.z -= speed * dt;
  if (move.backward) velocity.z += speed * dt;
  if (move.left) velocity.x -= speed * dt;
  if (move.right) velocity.x += speed * dt;
  controls.moveRight(velocity.x);
  controls.moveForward(velocity.z);

  camera.position.y = 1.6;

  needs.update(dt);
  stateMachine.update(dt);

  hud.textContent = [
    `State: ${stateMachine.currentState}`,
    `Hunger: ${needs.hunger.toFixed(1)}`,
    `Energy: ${needs.energy.toFixed(1)}`,
    `Mood: ${needs.mood.toFixed(1)}`,
    `Bond: ${needs.bond.toFixed(1)}`,
    `Social: ${needs.socialNeed.toFixed(1)}`,
  ].join('\n');

  renderer.render(scene, camera);
}

window.addEventListener('resize', () => {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
});

animate();
