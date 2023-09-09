using System.Collections;
using UnityEngine;
namespace JoyWay.Core.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopAllCoroutines();
    }
}
