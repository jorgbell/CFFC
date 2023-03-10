namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(SobelOutlineSettings))]
    public class SobelOutlineEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter threshold;
        SerializedDataParameter outlineColor;
        SerializedDataParameter backgroundColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SobelOutlineSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            threshold = Unpack(o.Find(x => x.threshold));
            outlineColor = Unpack(o.Find(x => x.outlineColor));
            backgroundColor = Unpack(o.Find(x => x.backgroundColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The SobelOutline effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(threshold);
            PropertyField(outlineColor);
            PropertyField(backgroundColor);
        }

        // Check the Forward Renderer and make sure the SobelOutline effect is attached.
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
                if (item?.GetType() == typeof(SobelOutline))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Outline (Sobel)");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Outline (Sobel)";
    }
#endif
    }
}
