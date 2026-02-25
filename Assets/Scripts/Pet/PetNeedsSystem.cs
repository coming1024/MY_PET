using UnityEngine;

namespace MyPet.Pet
{
    public enum PetLifeStage
    {
        Baby,
        Adult
    }

    [System.Serializable]
    public class PetNeedsSnapshot
    {
        public int ageDays;
        public PetLifeStage lifeStage;
        public float hunger;
        public float energy;
        public float mood;
        public float bond;
        public float socialNeed;
    }

    /// <summary>
    /// 宠物需求与成长模型（MVP 版）
    /// 0 = 极低，100 = 极高。
    /// </summary>
    public class PetNeedsSystem : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private string petId = "pet_001";

        [Header("Growth")]
        [SerializeField] private int ageDays = 1;
        [SerializeField] private PetLifeStage lifeStage = PetLifeStage.Baby;
        [SerializeField] private float growthSecondsPerDay = 120f;

        [Header("Needs (0~100)")]
        [SerializeField] private float hunger = 25f;
        [SerializeField] private float energy = 80f;
        [SerializeField] private float mood = 70f;
        [SerializeField] private float bond = 40f;
        [SerializeField] private float socialNeed = 30f;

        [Header("Decay Rates / sec")]
        [SerializeField] private float hungerIncreasePerSec = 0.35f;
        [SerializeField] private float energyDecreasePerSec = 0.2f;
        [SerializeField] private float moodDecreasePerSec = 0.08f;
        [SerializeField] private float socialNeedIncreasePerSec = 0.15f;

        private float growthTimer;

        public string PetId => petId;
        public int AgeDays => ageDays;
        public PetLifeStage LifeStage => lifeStage;
        public float Hunger => hunger;
        public float Energy => energy;
        public float Mood => mood;
        public float Bond => bond;
        public float SocialNeed => socialNeed;

        private void Update()
        {
            TickNeeds(Time.deltaTime);
            TickGrowth(Time.deltaTime);
        }

        public void TickNeeds(float dt)
        {
            hunger = Clamp01To100(hunger + hungerIncreasePerSec * dt);
            energy = Clamp01To100(energy - energyDecreasePerSec * dt);
            mood = Clamp01To100(mood - moodDecreasePerSec * dt);
            socialNeed = Clamp01To100(socialNeed + socialNeedIncreasePerSec * dt);

            if (hunger > 80f || energy < 20f)
            {
                mood = Clamp01To100(mood - 0.15f * dt);
            }
        }

        public void TickGrowth(float dt)
        {
            growthTimer += dt;
            if (growthTimer < growthSecondsPerDay)
            {
                return;
            }

            growthTimer = 0f;
            ageDays += 1;
            lifeStage = ageDays >= 7 ? PetLifeStage.Adult : PetLifeStage.Baby;
        }

        public void Feed(float amount = 30f)
        {
            hunger = Clamp01To100(hunger - amount);
            mood = Clamp01To100(mood + 8f);
            bond = Clamp01To100(bond + 2f);
        }

        public void Petting(float amount = 10f)
        {
            mood = Clamp01To100(mood + amount);
            bond = Clamp01To100(bond + 3f);
            socialNeed = Clamp01To100(socialNeed - 6f);
        }

        public void Play(float amount = 12f)
        {
            mood = Clamp01To100(mood + amount);
            energy = Clamp01To100(energy - 8f);
            socialNeed = Clamp01To100(socialNeed - 10f);
            bond = Clamp01To100(bond + 4f);
        }

        public void Sleep(float amount = 20f)
        {
            energy = Clamp01To100(energy + amount);
            mood = Clamp01To100(mood + 2f);
        }

        public PetNeedsSnapshot GetSnapshot()
        {
            return new PetNeedsSnapshot
            {
                ageDays = ageDays,
                lifeStage = lifeStage,
                hunger = hunger,
                energy = energy,
                mood = mood,
                bond = bond,
                socialNeed = socialNeed
            };
        }

        public void ApplySnapshot(PetNeedsSnapshot snapshot)
        {
            ageDays = snapshot.ageDays;
            lifeStage = snapshot.lifeStage;
            hunger = Clamp01To100(snapshot.hunger);
            energy = Clamp01To100(snapshot.energy);
            mood = Clamp01To100(snapshot.mood);
            bond = Clamp01To100(snapshot.bond);
            socialNeed = Clamp01To100(snapshot.socialNeed);
        }

        private static float Clamp01To100(float value)
        {
            return Mathf.Clamp(value, 0f, 100f);
        }
    }
}
