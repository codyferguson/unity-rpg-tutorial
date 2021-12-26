using System;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistenObjects();

            hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
            GameObject persistentObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}