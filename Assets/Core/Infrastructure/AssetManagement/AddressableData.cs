using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JoyWay.Core.Infrastructure.AssetManagement
{
    public class AddressableData
    {
        public bool Ready;
        public List<AsyncOperationHandle> Handles;
    }
}
