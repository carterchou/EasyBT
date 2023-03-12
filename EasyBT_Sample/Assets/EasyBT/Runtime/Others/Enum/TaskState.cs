using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    public enum TaskState
    {
        None = 0,
        Success = 1,
        Failure = 2,
        Running = 3,
    }

    public enum NodeType
    {
        None = 0,
        NoScript = 1,
    }
}

