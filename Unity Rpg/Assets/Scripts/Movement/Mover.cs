using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] public Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        [System.Serializable]
        struct MoverSaveData {
            public SerializableVector3 position;   
            public SerializableVector3 rotation;
        }

        public object CaptureState() {
            MoverSaveData data = new MoverSaveData();
            data.position =new SerializableVector3( transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state) {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }

        /* Alt way to do save with multiple params
        public object CaptureState() {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotaiton"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state) {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }

        Are you trying to spawn objects at run-time and wondering how this would fit into our system?
        As you might have seen UUIDs are generated at edit time. GameObjects spawned in the scene at runtime do not get tracked by the save system nor does it try to respawn objects on load. 
        There are two ways to solve this. We will evaluate the two using the example of weapon drops spawned by enemies when they die. 
        We will assume that you want to keep weapons in the same place between saves.
        
        Run-time Object Manager
        In this approach, you would have a manager object in every scene responsible for saving and loading all the pickups.
        On save, the manager would find all the pickup game objects, query the important state, save this to an array and return that.
        On restore, the manager would iterate through the array and spawn all the game objects.
        Optionally, this manager could be responsible for spawning the objects in the first place given that it already knows how to do that.

        Make the Spawner Responsible
        Alternatively, you can put the responsibility in the hands of the object that spawned the run-time object. In our example, this would be the enemy. 
        When the enemy dies, it records a list of the GameObjects it spawned. When saving it would query the scene for these objects. 
        Check if they have already been picked up (in which case we can remove them from the list) and store their state to an array.
        When restoring we would respawning the objects from the same array.
         */
    }
}
