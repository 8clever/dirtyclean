using System;
using System.Collections.Generic;

namespace UnityEngine.GameGrowth
{
    [HideInInspector]
    [AddComponentMenu("")]
    class UnityThreadUtil : MonoBehaviour
    {
        private static List<Action> s_Callbacks = new List<Action>();
        private static volatile bool s_CallbacksPending;

        internal void RunOnMainThread(Action runnable)
        {
            lock (s_Callbacks)
            {
                s_Callbacks.Add(runnable);
                s_CallbacksPending = true;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!s_CallbacksPending)
            {
                return;
            }
            // We copy our actions to another array to avoid
            // locking the queue whilst we process them.
            Action[] copy;
            lock (s_Callbacks)
            {
                if (s_Callbacks.Count == 0)
                {
                    return;
                }

                copy = new Action[s_Callbacks.Count];
                s_Callbacks.CopyTo(copy);
                s_Callbacks.Clear();
                s_CallbacksPending = false;
            }

            foreach (var action in copy)
            {
                action();
            }
        }
    }
}