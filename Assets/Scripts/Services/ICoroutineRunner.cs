using System.Collections;
using UnityEngine;

namespace JoyWay.UI
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopAllCoroutines();
    }
}
