using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace Sloot {
    public class RotationSloot : MonoBehaviour {
        #region Axis

        public static Vector3 TranslateVector3(string axis) {
            Dictionary<string, Vector3> vectors = new Dictionary<string, Vector3>();
            vectors.Add("x", Vector3.right);
            vectors.Add("right", Vector3.right);
            vectors.Add("invx", Vector3.left * 2);
            vectors.Add("-x", Vector3.left * 2);
            vectors.Add("left", Vector3.left);
            vectors.Add("y", Vector3.up);
            vectors.Add("up", Vector3.up);
            vectors.Add("invy", Vector3.down * 2);
            vectors.Add("-y", Vector3.down * 2);
            vectors.Add("down", Vector3.down);
            vectors.Add("z", Vector3.forward);
            vectors.Add("forward", Vector3.forward);
            vectors.Add("front", Vector3.forward);
            vectors.Add("invz", Vector3.back * 2);
            vectors.Add("-z", Vector3.back * 2);
            vectors.Add("back", Vector3.back);
            axis = axis.ToLower();
            Vector3 final = Vector3.zero;
            foreach (KeyValuePair<string, Vector3> kvp in vectors) {
                    final += kvp.Value * Regex.Matches(axis, kvp.Key).Count;
            }
            if (final == Vector3.zero) {
                Debug.LogError("Unrecognized vector\n" + "Vector3.forward Used");
                final = Vector3.forward;
            }
            return final.normalized;
        }

        public static Vector3 GetOriginAxis(Vector3 axis) {
            if (axis == Vector3.up)
                return Vector3.right;
            return Vector3.Cross(Vector3.up, axis).normalized;
        }

        /// <summary>
        /// Returns a vector which indicate the normalized direction where an object should be at X <paramref name="degrees"/> degrees of the origin (0/0/0) around the <paramref name="axis"/>.
        /// </summary>
        public static Vector3 GetDirectionOnAxis(float degrees, Vector3 axis) {
            axis = axis.normalized;
            Modulo360(degrees);
            return Quaternion.AngleAxis(degrees, axis) * GetOriginAxis(axis);
        }

        /// <summary>
        /// Returns a vector which indicate the position where an object should be to be at X <paramref name="degrees"/> and X <paramref name="distance"/> of the <paramref name="origin"/> around the <paramref name="axis"/>.
        /// </summary>
        public static Vector3 GetPosOnAxis(Vector3 origin, float degrees, float distance, Vector3 axis) {
            return origin + GetDirectionOnAxis(degrees, axis) * distance;
        }

        #endregion

        #region Angle

        public const float TWOPI = Mathf.PI * 2;
        public static float Modulo360(float toModulo) {
            return (toModulo % 360 + 360) % 360;
        }

        public static float ModuloTwoPI(float toModulo) {
            return (toModulo % TWOPI + TWOPI) % TWOPI;
        }

        public static float Lap360(float degree, int lap) {
            return Modulo360(degree) + 360 * lap;
        }

        public static float LapTwoPI(float radian, int lap) {
            return ModuloTwoPI(radian) + TWOPI * lap;
        }

        /// <summary>
        /// Returns a float which indicate the angle in radian of the <paramref name="target"/> around the <paramref name="position"/> using the <paramref name="axis"/>.
        /// </summary>
        public static float GetRadBasedOfTarget(Vector3 position, Vector3 target, Vector3 axis) {
            return Mathf.Deg2Rad * GetDegreeBasedOfTarget(position, target, axis);
        }

        public static float GetDegreeBasedOfTarget(Vector3 position, Vector3 target, Vector3 axis) {
            Vector3 positionTarget = target - position;
            float angle = Vector3.SignedAngle(GetOriginAxis(axis), positionTarget, axis);
            return angle < 0 ? 360 + angle : angle;
        }

        #endregion

        #region Rotation

        public enum RotationType {
            CLOCKWISE,
            ANTICLOCKWISE,
            NEAR
        }

        /// <summary>
        /// Move <paramref name="toTurn"/> around <paramref name="toTurnAround"/> at <paramref name="angularSpeed"/> in order to reach <paramref name="degrees"/> using the <paramref name="axis"/> and the direction given by <paramref name="type"/>.
        /// </summary>
        /// <param name = "angularSpeed" > 
        /// <br> Value must be in degrees per second</br>
        /// <br> The value "360" is equal to one turn every second</br>
        /// </param>
        public static IEnumerator GoAtDegreeAroundTransform(Transform toTurn, Vector3 toTurnAround, Vector3 axis, float degrees, float angularSpeed, RotationType type) {
            degrees = Modulo360(degrees);
            float currentDegree = GetDegreeBasedOfTarget(toTurnAround, toTurn.position, axis);
            if (currentDegree == degrees) {
                yield break;
            }
            float radius = Vector3.Distance(toTurn.position, toTurnAround);
            float goAtDegree = 0f;
            bool clockwise = true;
            switch (type) {
                case RotationType.CLOCKWISE:
                    if (currentDegree < degrees) {
                        goAtDegree = degrees - 360;
                    } else {
                        goAtDegree = degrees;
                    }
                    clockwise = true;
                    break;
                case RotationType.ANTICLOCKWISE:
                    if (currentDegree > degrees) {
                        goAtDegree = degrees + 360;
                    } else {
                        goAtDegree = degrees;
                    }
                    clockwise = false;
                    break;
                case RotationType.NEAR:
                    if (currentDegree > degrees) {
                        if (Mathf.Abs(currentDegree - degrees) < Mathf.Abs(currentDegree - (degrees + 360))) {
                            goAtDegree = degrees;
                            clockwise = true;
                        } else {
                            goAtDegree = degrees + 360;
                            clockwise = false;
                        }
                    } else {
                        if (Mathf.Abs(currentDegree - degrees) < Mathf.Abs(currentDegree - (degrees - 360))) {
                            goAtDegree = degrees;
                            clockwise = false;
                        } else {
                            goAtDegree = degrees - 360;
                            clockwise = true;
                        }
                    }
                    break;
            }
            if (clockwise) {
                while (currentDegree > goAtDegree) {
                    currentDegree -= Time.deltaTime * angularSpeed;
                    if (currentDegree > goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround, currentDegree, radius, axis);
                    } else {
                        toTurn.position = GetPosOnAxis(toTurnAround, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            } else {
                while (currentDegree < goAtDegree) {
                    currentDegree += Time.deltaTime * angularSpeed;
                    if (currentDegree < goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround, currentDegree, radius, axis);
                    } else {
                        toTurn.position = GetPosOnAxis(toTurnAround, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            }
        }
        /// <summary>
        /// Move <paramref name="toTurn"/> around <paramref name="toTurnAround"/> at <paramref name="angularSpeed"/> during <paramref name="degree"/> using the <paramref name="axis"/>.
        /// </summary>
        /// <param name = "angularSpeed" > 
        /// <br> Value must be in degrees per second</br>
        /// <br> The value "360" is equal to one turn every second</br>
        /// </param>
        public static IEnumerator TurnDegreeAroundTransform(Transform toTurn, Vector3 toTurnAround, Vector3 axis, float degree, float angularSpeed) {
            float currentDegree = GetDegreeBasedOfTarget(toTurnAround, toTurn.position, axis);
            float radius = Vector3.Distance(toTurn.position, toTurnAround);
            int direction = currentDegree < currentDegree + degree ? 1 : -1;
            float goAtDegree = currentDegree + degree;
            if (direction == 1) {
                while (currentDegree < goAtDegree) {
                    currentDegree += Time.deltaTime * angularSpeed;
                    toTurn.position = GetPosOnAxis(toTurnAround, currentDegree, radius, axis);
                    if (currentDegree >= goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            } else {
                while (currentDegree > goAtDegree) {
                    currentDegree -= Time.deltaTime * angularSpeed;
                    toTurn.position = GetPosOnAxis(toTurnAround, currentDegree, radius, axis);
                    if (currentDegree <= goAtDegree) {
                        toTurn.position = GetPosOnAxis(toTurnAround, goAtDegree, radius, axis);
                    }
                    yield return null;
                }
            }
        }
        #endregion
    }
}
