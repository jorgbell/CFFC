namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(HalftoneSettings))]
    public class HalftoneEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter halftoneTexture;
        SerializedDataParameter softness;
        SerializedDataParameter textureSize;
        SerializedDataParameter minMaxLuminance;
        SerializedDataParameter darkColor;
        SerializedDataParameter lightColor;
        SerializedDataParameter useSceneColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<HalftoneSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            halftoneTexture = Unpack(o.Find(x => x.halftoneTexture));
            softness = Unpack(o.Find(x => x.softness));
            textureSize = Unpack(o.Find(x => x.textureSize));
            minMaxLuminance = Unpack(o.Find(x => x.minMaxLuminance));
            darkColor = Unpack(o.Find(x => x.darkColor));
            lightColor = Unpack(o.Find(x => x.lightColor));
            useSceneColor = Unpack(o.Find(x => x.useSceneColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Halftone effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(halftoneTexture);
            PropertyField(softness);
            PropertyField(textureSize);
            PropertyField(minMaxLuminance);
            PropertyField(darkColor);
            PropertyField(lightColor);
            PropertyField(useSceneColor);
        }

        // Check the Forward Renderer and make sure the Halftone effect is attached.
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
                if (item?.GetType() == typeof(Halftone))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Halftone");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Halftone";
    }
#endif
    }
}
