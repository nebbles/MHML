using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.NativePlugins
{
    using DesignPatterns;

    public class UnityThreadDispatcher : SingletonPattern<UnityThreadDispatcher>
    {
        #region Fields

        private Queue<Action> m_queue = new Queue<Action>();

        #endregion

        #region Static Block

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            // Initialise
            Instance.Init();
        }

        #endregion

        #region Unity Lifecycle

        private void Update()
        {
            lock (instanceLock)
            {
                while (m_queue.Count > 0)
                {
                    Action action = m_queue.Dequeue();
                    action();
                }
            }
        }

        #endregion

        #region Public

        public void Enqueue(Action action)
        {
            m_queue.Enqueue(action);
        }

        #endregion

    }
}
