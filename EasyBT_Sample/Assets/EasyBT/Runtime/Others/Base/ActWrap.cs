using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Runtime
{
    [Serializable]
    public class ActWrap
    {

#if UNITY_EDITOR
        public MonoScript monoScript;
#endif
        [SerializeReference]
        public Act act;

    }
}