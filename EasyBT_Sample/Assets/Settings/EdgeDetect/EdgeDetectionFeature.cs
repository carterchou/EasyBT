using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Rendering.LWRP
{
    public class EdgeDetectionFeature : ScriptableRendererFeature
    {
        [Serializable]
        public class EdgeDetectionSettings
        {
            public Material material = null;
            public RenderPassEvent @event = RenderPassEvent.AfterRendering;
        }

        private EdgeDetectionFeaturePass m_EdgeDetectionFeaturePass;
        public EdgeDetectionSettings settings = new EdgeDetectionSettings();


        public override void Create()
        {
            m_EdgeDetectionFeaturePass = new EdgeDetectionFeaturePass(settings.@event, settings.material);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {            
            var src = renderer.cameraColorTarget;

            if (settings.material == null)
                return;

            m_EdgeDetectionFeaturePass.Setup(src);
            renderer.EnqueuePass(m_EdgeDetectionFeaturePass);
        }
    }
}