using Gululu.Config;
using Hank.MainScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class MatePlayerTag : MonoBehaviour
    {
        [SerializeField]
        private MatePlayer.Locations location;
        public MatePlayer.Locations Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
    }
}

