using UnityEngine;

namespace Pinwheel.Poseidon
{
    [System.Serializable]
    public struct PWaterProfileDefault
    {
        [SerializeField]
        private int meshResolution;
        public int MeshResolution
        {
            get
            {
                return meshResolution;
            }
            set
            {
                meshResolution = value;
            }
        }
    }
}
