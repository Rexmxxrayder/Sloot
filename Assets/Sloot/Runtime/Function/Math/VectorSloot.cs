using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sloot {
    public class VectorSloot : MonoBehaviour { 
        public float GetRailLenght(List<Vector3> vectors, bool isLoop) {
            float length = 0;
            for (int i = 0; i < vectors.Count - 1; i++) {
                length += Vector3.Distance(vectors[i], vectors[i + 1]);
            }
            if (isLoop) {
                length += Vector3.Distance(vectors[vectors.Count - 1], vectors[0]);
            }
            return length;
        }
    }
}
