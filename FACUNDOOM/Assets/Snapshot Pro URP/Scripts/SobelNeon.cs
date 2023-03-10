﻿namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SobelNeon : ScriptableRendererFeature
    {
        class SobelNeonRenderPass : ScriptableRenderPass
        {
            private Material material;
            private SobelNeonSettings settings;

            private RenderTargetIdentifier source;
            private RenderTargetHandle mainTex;
            private string profilerTag;

            public SobelNeonRenderPass(string profilerTag)
            {
                this.profilerTag = profilerTag;
            }

            public void Setup(ScriptableRenderer renderer)
            {
                source = renderer.cameraColorTarget;
                settings = VolumeManager.instance.stack.GetComponent<SobelNeonSettings>();
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

                if (settings != null && settings.IsActive())
                {
                    renderer.EnqueuePass(this);
                    material = new Material(Shader.Find("SnapshotProURP/SobelNeon"));
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

                // Set SobelNeon effect properties.
                cmd.SetGlobalFloat("_SaturationFloor", settings.saturationFloor.value);
                cmd.SetGlobalFloat("_LightnessFloor", settings.lightnessFloor.value);

                // Execute effect using effect material.
                cmd.Blit(mainTex.id, source, material);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }
        }

        SobelNeonRenderPass pass;

        public override void Create()
        {
            pass = new SobelNeonRenderPass("SobelNeon");
            name = "Sobel Neon";
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            pass.Setup(renderer);
        }
    }
}
