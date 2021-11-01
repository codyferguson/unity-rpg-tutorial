
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }
        void Update()
        {
            if(health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget combatTarget = hit.transform.gameObject.GetComponent<CombatTarget>();

                if (combatTarget == null) continue;

                if (!GetComponent<Fighter>().CanAttack(combatTarget.gameObject)) { continue; }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(combatTarget.gameObject);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
