using System;
using System.Collections.Generic;
using MyPet.Pet;
using UnityEngine;

namespace MyPet.AI
{
    public enum PetBehaviorType
    {
        Idle,
        SeekUser,
        Eat,
        Sleep,
        Play
    }

    public interface IPetState
    {
        PetBehaviorType StateType { get; }
        void Enter();
        void Tick(float dt);
        void Exit();
    }

    public sealed class PetContext
    {
        public readonly PetNeedsSystem Needs;
        public readonly Transform PetTransform;
        public readonly Transform UserTransform;

        public bool RequestFeed;
        public bool RequestPlay;
        public bool RequestCall;
        public bool RequestPetting;

        public PetContext(PetNeedsSystem needs, Transform petTransform, Transform userTransform)
        {
            Needs = needs;
            PetTransform = petTransform;
            UserTransform = userTransform;
        }
    }

    public abstract class PetStateBase : IPetState
    {
        protected readonly PetContext Context;
        protected readonly PetStateMachine Machine;

        public abstract PetBehaviorType StateType { get; }

        protected PetStateBase(PetContext context, PetStateMachine machine)
        {
            Context = context;
            Machine = machine;
        }

        public virtual void Enter() { }
        public virtual void Tick(float dt) { }
        public virtual void Exit() { }
    }

    public class PetStateMachine : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PetNeedsSystem needsSystem;
        [SerializeField] private Transform userTransform;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 2.2f;
        [SerializeField] private float stopDistance = 1.6f;

        [Header("Debug")]
        [SerializeField] private PetBehaviorType currentBehavior = PetBehaviorType.Idle;

        private readonly Dictionary<PetBehaviorType, IPetState> states = new();
        private IPetState currentState;
        private PetContext context;

        public PetBehaviorType CurrentBehavior => currentBehavior;
        public PetNeedsSystem Needs => needsSystem;
        public Transform UserTransform => userTransform;
        public float MoveSpeed => moveSpeed;
        public float StopDistance => stopDistance;

        private void Awake()
        {
            if (needsSystem == null)
            {
                needsSystem = GetComponent<PetNeedsSystem>();
            }

            context = new PetContext(needsSystem, transform, userTransform);
            BuildStates();
            ChangeState(PetBehaviorType.Idle);
        }

        private void Update()
        {
            if (currentState == null)
            {
                return;
            }

            var next = DecideNextBehavior();
            if (next != currentBehavior)
            {
                ChangeState(next);
            }

            currentState.Tick(Time.deltaTime);
        }

        public void SignalFeed()
        {
            context.RequestFeed = true;
        }

        public void SignalPlay()
        {
            context.RequestPlay = true;
        }

        public void SignalCall()
        {
            context.RequestCall = true;
        }

        public void SignalPetting()
        {
            context.RequestPetting = true;
        }

        public void ConsumeInteractionFlags()
        {
            context.RequestFeed = false;
            context.RequestPlay = false;
            context.RequestCall = false;
            context.RequestPetting = false;
        }

        private void BuildStates()
        {
            states[PetBehaviorType.Idle] = new IdleState(context, this);
            states[PetBehaviorType.SeekUser] = new SeekUserState(context, this);
            states[PetBehaviorType.Eat] = new EatState(context, this);
            states[PetBehaviorType.Sleep] = new SleepState(context, this);
            states[PetBehaviorType.Play] = new PlayState(context, this);
        }

        private PetBehaviorType DecideNextBehavior()
        {
            if (context.RequestFeed || needsSystem.Hunger > 70f)
            {
                return PetBehaviorType.Eat;
            }

            if (needsSystem.Energy < 20f)
            {
                return PetBehaviorType.Sleep;
            }

            if (context.RequestPlay)
            {
                return PetBehaviorType.Play;
            }

            if (context.RequestCall || needsSystem.SocialNeed > 60f)
            {
                return PetBehaviorType.SeekUser;
            }

            return PetBehaviorType.Idle;
        }

        private void ChangeState(PetBehaviorType next)
        {
            currentState?.Exit();
            currentState = states[next];
            currentBehavior = next;
            currentState.Enter();
        }
    }

    internal sealed class IdleState : PetStateBase
    {
        public override PetBehaviorType StateType => PetBehaviorType.Idle;

        public IdleState(PetContext context, PetStateMachine machine) : base(context, machine) { }

        public override void Tick(float dt)
        {
            if (Context.RequestPetting)
            {
                Context.Needs.Petting();
                Machine.ConsumeInteractionFlags();
            }
        }
    }

    internal sealed class SeekUserState : PetStateBase
    {
        public override PetBehaviorType StateType => PetBehaviorType.SeekUser;

        public SeekUserState(PetContext context, PetStateMachine machine) : base(context, machine) { }

        public override void Tick(float dt)
        {
            if (Context.UserTransform == null)
            {
                return;
            }

            Vector3 toUser = Context.UserTransform.position - Context.PetTransform.position;
            float distance = toUser.magnitude;

            if (distance > Machine.StopDistance)
            {
                Vector3 dir = toUser.normalized;
                Context.PetTransform.position += dir * Machine.MoveSpeed * dt;
                Context.PetTransform.forward = Vector3.Lerp(Context.PetTransform.forward, dir, dt * 8f);
            }
            else
            {
                Context.Needs.Petting(2f);
                Machine.ConsumeInteractionFlags();
            }
        }
    }

    internal sealed class EatState : PetStateBase
    {
        private float eatTimer;

        public override PetBehaviorType StateType => PetBehaviorType.Eat;

        public EatState(PetContext context, PetStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            eatTimer = 0f;
        }

        public override void Tick(float dt)
        {
            eatTimer += dt;
            if (eatTimer < 1.2f)
            {
                return;
            }

            Context.Needs.Feed(28f);
            Machine.ConsumeInteractionFlags();
            eatTimer = 0f;
        }
    }

    internal sealed class SleepState : PetStateBase
    {
        public override PetBehaviorType StateType => PetBehaviorType.Sleep;

        public SleepState(PetContext context, PetStateMachine machine) : base(context, machine) { }

        public override void Tick(float dt)
        {
            Context.Needs.Sleep(12f * dt);
        }
    }

    internal sealed class PlayState : PetStateBase
    {
        private float playTimer;

        public override PetBehaviorType StateType => PetBehaviorType.Play;

        public PlayState(PetContext context, PetStateMachine machine) : base(context, machine) { }

        public override void Enter()
        {
            playTimer = 0f;
        }

        public override void Tick(float dt)
        {
            playTimer += dt;
            if (playTimer < 0.8f)
            {
                return;
            }

            Context.Needs.Play(6f);
            Machine.ConsumeInteractionFlags();
            playTimer = 0f;
        }
    }
}
