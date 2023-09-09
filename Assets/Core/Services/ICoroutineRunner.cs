using System.Collections;
using UnityEngine;
namespace Core.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopAllCoroutines();
    }
}
