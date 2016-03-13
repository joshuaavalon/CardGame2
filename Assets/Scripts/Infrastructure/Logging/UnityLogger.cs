using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Logging
{
    internal class UnityLogger : MonoBehaviour, ILogger
    {
        public bool EnableVerbose = true;
        public bool EnableWarning = true;
        public bool EnableError = true;
        public bool EnableFilter = true;
        public string[] IncludeTagsFilter = new string[0];
        public string[] ExcludeTagsFilter = new string[0];

        public void Log(LogType type, object message, string messageTag = "")
        {
            var prefix = string.IsNullOrEmpty(messageTag) ? "" : "[" + messageTag + "]";
            switch (type)
            {
                case LogType.Verbose:
                    if (!EnableVerbose)
                        return;
                    if (IsShow(messageTag))
                        Debug.Log(prefix+message);
                    break;
                case LogType.Warning:
                    if (!EnableWarning)
                        return;
                    if (IsShow(messageTag))
                        Debug.LogWarning(prefix + message);
                    break;
                case LogType.Error:
                    if (!EnableError)
                        return;
                    if (IsShow(messageTag))
                        Debug.LogError(prefix + message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        private void OnEnable()
        {
            Logging.Log.AddLogger(this);
        }

        private void OnDisable()
        {
            Logging.Log.RemoveLogger(this);
        }

        private bool IsShow(string messageTag)
        {
            var show = false;
            if (!EnableFilter)
                return true;
            if (IncludeTagsFilter.Length > 0)
                show = IncludeTagsFilter.Contains(messageTag);
            if (ExcludeTagsFilter.Length <= 0) return show;
            if(!IncludeTagsFilter.Contains(messageTag))
                show = false;
            return show;

        }
    }
}
