using System;
using UnityEngine;

namespace PlayableNodes
{
    public abstract class EditorGUIScope : IDisposable
    {
        private bool _disposed;

        protected abstract void CloseScope();

        ~EditorGUIScope()
        {
            if (_disposed)
                return;
            Debug.LogError("Scope was not disposed! You should use the 'using' keyword or manually call Dispose.");
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            CloseScope();
        }
    }
}