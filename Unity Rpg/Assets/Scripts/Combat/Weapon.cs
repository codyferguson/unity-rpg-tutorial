using UnityEngine;

namespace RPG.Combat {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform righthand, Transform leftHand, Animator animator) {
            if (equippedPrefab != null) {
                Instantiate(equippedPrefab, isRightHanded ? righthand : leftHand);
            }
            if (animatorOverride != null) {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetDamage() {
            return weaponDamage;
        }

        public float GetRange() {
            return weaponRange;
        }
    }
}
