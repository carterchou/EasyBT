using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class RotateToTargetPos : MonoBehaviour
{
    public Transform refPos;
    private void Update()
    {
        transform.position =  Camera.main.WorldToScreenPoint(refPos.position);
    }
}
