using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyBT.Runtime
{
    public class CustomNodeAttibute : Attribute
    {
        public Type type;

        public CustomNodeAttibute(Type type)
        {
            this.type = type;
        }
    }
}

