﻿namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SobelOutline : ScriptableRendererFeature
    {
        class SobelOutlineRenderPass : ScriptableRenderPass
        {
            private Material material;
            private SobelOutlineSettings settings;

            private RenderTargetIdentifier source;
            private RenderTargetHandle mainTex;
            private string profilerTag;

            public SobelOutlineRenderPass(string profilerTag)
            {
                this.profilerTag = profilerTag;
            }

            public void Setup(ScriptableRenderer renderer)
            {
                source = renderer.cameraColorTarget;
                settings = VolumeManager.instance.stack.GetComponent<SobelOutlineSettings>();
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

                if (settings != null && settings.IsActive())
                {
                    renderer.EnqueuePass(this);
                    material = new Material(Shader.Find("SnapshotProURP/SobelOutline"));
                }
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                if (settings == null)
                {
                    return;
                }

                mainTex = new RenderTargetHandle();
                mainTex.id = Shader.PropertyToID("MainTex");
                cmd.GetTemporaryRT(mainTex.id, cameraTextureDescriptor);

                base.Configure(cmd, cameraTextureDescriptor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (!settings.IsActive())
                {
                    return;
                }

                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

                // Copy camera color texture to MainTex.
                cmd.Blit(source, mainTex.id);

                // Set SobelOutline effect properties.
                cmd.SetGlobalFloat("_Threshold", settings.threshold.value);
                cmd.SetGlobalColor("_OutlineColor", settings.outlineColor.value);
                cmd.SetGlobalColor("_BackgroundColor", settings.backgroundColor.value);

                // Execute effect using effect material.
                cmd.Blit(mainTex.id, source, material);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }
        }

        SobelOutlineRenderPass pass;

        public override void Create()
        {
            pass = new SobelOutlineRenderPass("SobelOutline");
            name = "Sobel Outline";
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            pass.Setup(renderer);
        }
    }
}
