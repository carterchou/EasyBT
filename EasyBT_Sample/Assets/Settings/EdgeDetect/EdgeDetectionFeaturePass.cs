namespace UnityEngine.Rendering.Universal
{
    public class EdgeDetectionFeaturePass : ScriptableRenderPass
    {
        public Material blitMaterial = null;

        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
            tempTexture.Init("_TempTexture");
        }

        public EdgeDetectionFeaturePass(RenderPassEvent renderPassEvent, Material blitMaterial)
        {
            this.renderPassEvent = renderPassEvent;
            this.blitMaterial = blitMaterial;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(name: "EdgeDetection");
            var ct = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(tempTexture.id, ct, FilterMode.Bilinear);
            Blit(cmd, source, tempTexture.Identifier());
            Blit(cmd, tempTexture.Identifier(),source, blitMaterial, 0);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }
}

