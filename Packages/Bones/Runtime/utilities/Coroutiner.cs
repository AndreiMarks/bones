using UnityEngine;
using System.Collections;
using System;

namespace Bones
{
    /// <summary>
    /// Sometimes you want to run coroutines but don't have a monobehavior available.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        /*
        /// <summary>
        /// Executues the given action after a delay.
        /// </summary>
        public void DoAfterDelay(Action action, float delay, MonoBehaviour monoBehaviour = null) {
            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(DoAfterDelayCoroutine(action, delay));
            else
                StartCoroutine(DoAfterDelayCoroutine(action, delay));
        }

        /// <summary>
        /// Executes the given action after delay in realtime (so can run if timescale != 1)
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="monoBehaviour">Bono behaviour.</param>
        public void DoAfterDelayRealtime(Action action, float delay, MonoBehaviour monoBehaviour = null) {
            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(DoAfterDelayCoroutine(action, delay, true));
            else
                StartCoroutine(DoAfterDelayCoroutine(action, delay, true));
        }

        /// <summary>
        /// Executes the given action when a condition is met before a timeout. Times in seconds.
        /// </summary>
        public void DoAfterConditionBecomesTrue(Action action, Func<bool> predicate, float timeout = 0, Action onTimeout = null,
                float checkInterval = 0.5f, MonoBehaviour monoBehaviour = null) {
            if (monoBehaviour != null)
                monoBehaviour.StartCoroutine(DoAfterConditionBecomesTrueCoroutine(action, predicate, timeout, onTimeout,
                    checkInterval));
            else
                StartCoroutine(DoAfterConditionBecomesTrueCoroutine(action, predicate, timeout, onTimeout, checkInterval));
        }

        /// <summary>
        /// Waits for real time (Ignores Time.timeScale)
        /// </summary>
        /// <param name="delay">Delay.</param>
        public static IEnumerator WaitForRealTime(float delay) {
            while (true) {
                float pauseEndTime = Time.realtimeSinceStartup + delay;
                while (Time.realtimeSinceStartup < pauseEndTime) {
                    yield return 0;
                }
                break;
            }
        }

        IEnumerator DoAfterDelayCoroutine(Action action, float delay, bool useRealtime = false) {
            if (useRealtime) {
                // can't waitforseconds when TimeScale is 0
                float pauseEndTime = Time.realtimeSinceStartup + delay;
                while (Time.realtimeSinceStartup < pauseEndTime)
                    yield return 0;
            }
            else
                yield return new WaitForSeconds(delay);

            if (action != null)
                action();
        }

        /// <summary>
        /// Will only time out of routine if the <paramref name="timeout"/> is set to a value higher than 0;
        /// </summary>
        /// <param name="action"></param>
        /// <param name="predicate"></param>
        /// <param name="timeout"></param>
        /// <param name="onTimeout"></param>
        /// <param name="checkInterval"></param>
        /// <returns></returns>
        IEnumerator DoAfterConditionBecomesTrueCoroutine(Action action, Func<bool> predicate, float timeout = 0,
                Action onTimeout = null, float checkInterval = 0.1f) {
            float startTime = Time.time;

            bool conditionMet = false;
            while (!(conditionMet = predicate()) && ((Time.time - startTime) < timeout || timeout <= 0))
                yield return new WaitForSeconds(checkInterval);

            if (conditionMet)
                action();
            else if (onTimeout != null)
                onTimeout();
        }

        /// <summary>
        /// Returns a coroutine that only exits once all <paramref name="coroutines"/> finish
        /// </summary>
        /// <param name="coroutines">Coroutines to wait for.</param>
        public static Coroutine WaitForAllToFinish(params Coroutine[] coroutines) {
            return Instance.StartCoroutine(_WaitForAllToFinish(coroutines));
        }

        /// <summary>
        /// Returns a coroutine that only exits once all <paramref name="coroutines"/> finish
        /// </summary>
        /// <param name="coroutines">Coroutines to wait for.</param>
        public static Coroutine WaitForAllToFinish(MonoBehaviour onBehaviour, params Coroutine[] coroutines) {
            return onBehaviour.StartCoroutine(_WaitForAllToFinish(coroutines));
        }

        static IEnumerator _WaitForAllToFinish(params Coroutine[] coroutines) {
            for (int i = 0; i < coroutines.Length; i++) {
                if (coroutines[i] != null)
                    yield return coroutines[i];
            }
        }
        */
    }
}
