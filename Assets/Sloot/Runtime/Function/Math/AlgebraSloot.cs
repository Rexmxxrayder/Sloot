using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sloot {
    public class AlgebraSloot : MonoBehaviour {
        public static int Pow(int number, int pow) {
            int result = 1;
            for (int i = 0; i < pow; i++) {
                result *= number;
            }
            return result;
        }
    }
}