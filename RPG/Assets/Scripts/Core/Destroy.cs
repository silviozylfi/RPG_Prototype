using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Destroy : MonoBehaviour
    {
        //Variables
        [SerializeField] float destroyingDelay = 5f;
        void Start()
        {
            Destroy(gameObject, destroyingDelay);
        }

    }
}
