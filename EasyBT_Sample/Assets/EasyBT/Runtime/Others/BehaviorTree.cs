using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace EasyBT.Runtime
{

    [DisallowMultipleComponent]
    public class BehaviorTree : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        public BehaviorData behaviorData;

        [SerializeField]
        private bool isRunning = true;
        private bool isFirstTime = true;
        private Queue<Node> evaluateList = new Queue<Node>();

        private void Evaluating()
        {
            if (isFirstTime)
            {
                behaviorData.nodes[0].Evaluate(evaluateList);
                isFirstTime = false;
                return;
            }

            List<Node> currentEvaluateList = new List<Node>();

            while (evaluateList.Count > 0)
            {
                currentEvaluateList.Add(evaluateList.Dequeue());
            }

            foreach (var item in currentEvaluateList)
            {
                item.Evaluate(evaluateList);
            }
        }

        private void Update()
        {
            if (isRunning)
                Evaluating();
        }

        public void Pause()
        {
            isRunning = false;
        }
    }

}