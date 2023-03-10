﻿namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(FancyDitherSettings))]
    public class FancyDitherEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter noiseTex;
        SerializedDataParameter noiseSize;
        SerializedDataParameter thresholdOffset;
        SerializedDataParameter blendAmount;
        SerializedDataParameter darkColor;
        SerializedDataParameter lightColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<FancyDitherSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            noiseTex = Unpack(o.Find(x => x.noiseTex));
            noiseSize = Unpack(o.Find(x => x.noiseSize));
            thresholdOffset = Unpack(o.Find(x => x.thresholdOffset));
            blendAmount = Unpack(o.Find(x => x.blendAmount));
            darkColor = Unpack(o.Find(x => x.darkColor));
            lightColor = Unpack(o.Find(x => x.lightColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Fancy Dither effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(noiseTex);
            PropertyField(noiseSize);
            PropertyField(thresholdOffset);
            PropertyField(blendAmount);
            PropertyField(darkColor);
            PropertyField(lightColor);
        }

        // Check the Forward Renderer and make sure the FancyDither effect is attached.
        private bool CheckEffectEnabled()
        {
            if (UniversalRenderPipeline.asset == null)
            {
                return false;
            }

            ScriptableRendererData forwardRenderer =
                ((ScriptableRendererData[])typeof(UniversalRenderPipelineAsset)
                .GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(UniversalRenderPipeline.asset))[0];

            foreach (ScriptableRendererFeature item in forwardRenderer.rendererFeatures)
            {
                if (item?.GetType() == typeof(FancyDither))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Fancy Dither");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Fancy Dither";
    }
#endif
    }
}
