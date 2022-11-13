using System;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class ViewBase : MonoBehaviour
    {
        protected readonly List<IDisposable> _disposables = new();

        protected virtual void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }
}