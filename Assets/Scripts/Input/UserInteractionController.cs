using MyPet.AI;
using MyPet.Pet;
using UnityEngine;

namespace MyPet.Input
{
    /// <summary>
    /// MVP 输入桥接器：将用户输入映射为宠物交互事件。
    /// 可接 UI Button，也可直接使用键盘测试。
    /// </summary>
    public class UserInteractionController : MonoBehaviour
    {
        [SerializeField] private PetStateMachine petStateMachine;
        [SerializeField] private PetNeedsSystem petNeedsSystem;

        private void Awake()
        {
            if (petStateMachine == null)
            {
                petStateMachine = FindAnyObjectByType<PetStateMachine>();
            }

            if (petNeedsSystem == null && petStateMachine != null)
            {
                petNeedsSystem = petStateMachine.Needs;
            }
        }

        private void Update()
        {
            // Demo 快捷键：
            // F = 喂食, P = 抚摸, C = 呼唤, G = 玩耍
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                OnFeedPressed();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                OnPetPressed();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                OnCallPressed();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                OnPlayPressed();
            }
        }

        public void OnFeedPressed()
        {
            petStateMachine?.SignalFeed();
        }

        public void OnPetPressed()
        {
            petStateMachine?.SignalPetting();
            petNeedsSystem?.Petting(8f);
        }

        public void OnCallPressed()
        {
            petStateMachine?.SignalCall();
        }

        public void OnPlayPressed()
        {
            petStateMachine?.SignalPlay();
        }
    }
}
