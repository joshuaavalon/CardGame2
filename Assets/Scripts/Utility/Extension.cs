using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Utility
{
    public static class Extension
    {
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            return
                (from Transform tr in parent.transform where tr.tag == tag select tr.GetComponent<T>()).FirstOrDefault();
        }

        public static GameObject FindChildWithTag(this GameObject parent, string tag)
        {
            return (from Transform tr in parent.transform where tr.tag == tag select tr.gameObject).FirstOrDefault();
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof (T)).Cast<T>();
        }

        public static Vector3 Add(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 Minus(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 Add(this Vector2 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v2.z);
        }

        public static void Swap<T>(ref T source, ref T target)
        {
            var temp = source;
            source = target;
            target = temp;
        }

        public static void Swap<T>(this IList<T> list, int source, int target)
        {
            var temp = list[source];
            list[source] = list[target];
            list[target] = temp;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var count = list.Count;
            while (count > 1)
            {
                var index = Random.Range(0, count);
                count--;
                list.Swap(index, count);
            }
        }


        /// <summary>
        ///     Get the Opposite of PlayerType.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PlayerType Opposite(this PlayerType player)
        {
            return player == PlayerType.Player ? PlayerType.Opponent : PlayerType.Player;
        }

        /// <summary>
        ///     Move a GameObject from parent to another.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        public static void MoveToParent(this GameObject source, GameObject parent)
        {
            source.transform.SetParent(parent.transform);
            source.transform.localScale = Vector3.one;
        }

        public static T GetInterface<T>(this GameObject gameObject) where T : class
        {
            return typeof (T).IsInterface ? gameObject.GetComponents<Component>().OfType<T>().FirstOrDefault() : null;
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject gameObject) where T : class
        {
            return !typeof(T).IsInterface ? Enumerable.Empty<T>() : gameObject.GetComponents<Component>().OfType<T>();
        }

        public static T GetInterface<T>(this MonoBehaviour monoBehaviour) where T : class
        {
            return GetInterface<T>(monoBehaviour.gameObject);
        }

        public static IEnumerable<T> GetInterfaces<T>(this MonoBehaviour monoBehaviour) where T : class
        {
            return GetInterfaces<T>(monoBehaviour.gameObject);
        }

        public static float ClampAngle(this float angle, float min, float max)
        {
            angle = angle.NormalizeAngle(180);
            min = min.NormalizeAngle(180);
            max = max.NormalizeAngle(180);
            Debug.Log(angle+":"+ min+":"+max);
            angle = Mathf.Clamp(angle, min, max);
            Debug.Log(angle + ":" + min + ":" + max);
            return angle.NormalizeAngle(180);
        }

        public static float NormalizeAngle(this float angle , float boundary=360)
        {
            var newAngle = angle;
            if (boundary > 360 || boundary < 0)
                boundary = NormalizeAngle(boundary);
            while (newAngle <= -(360 - boundary)) newAngle += 360;
            while (newAngle > boundary) newAngle -= 360;
            return newAngle;
        }
    }
}