using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class TurnOff : MonoBehaviour
    {
        private void Start()
        {
            this.gameObject.SetActive(false);
        }
    }
}
