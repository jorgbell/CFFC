namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(GlitchSettings))]
    public class GlitchEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter offsetTexture;
        SerializedDataParameter offsetStrength;
        SerializedDataParameter verticalTiling;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<GlitchSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            offsetTexture = Unpack(o.Find(x => x.offsetTexture));
            offsetStrength = Unpack(o.Find(x => x.offsetStrength));
            verticalTiling = Unpack(o.Find(x => x.verticalTiling));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Glitch effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(offsetTexture);
            PropertyField(offsetStrength);
            PropertyField(verticalTiling);
        }

        // Check the Forward Renderer and make sure the Glitch effect is attached.
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
                if (item?.GetType() == typeof(Glitch))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Glitch");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Glitch";
    }
#endif
    }
}
