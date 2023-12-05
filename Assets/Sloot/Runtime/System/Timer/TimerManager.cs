using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sloot {
    public class TimerManager : MonoBehaviour {
        public static TimerManager instance;

        public static void Activate(Timer timer) {
            if (timer == null) { return; }
            if (instance == null) {
                GameObject newTimerManager = new("TimerManager");
                newTimerManager.AddComponent<TimerManager>();
                instance = newTimerManager.GetComponent<TimerManager>();
            }
        }
    }
}
