namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(LightStreaksSettings))]
    public class LightStreaksEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter luminanceThreshold;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<LightStreaksSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            luminanceThreshold = Unpack(o.Find(x => x.luminanceThreshold));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Light Streaks effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
            PropertyField(luminanceThreshold);
        }

        // Check the Forward Renderer and make sure the Light Streaks effect is attached.
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
                if (item?.GetType() == typeof(LightStreaks))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Light Streaks");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Light Streaks";
    }
#endif
    }
}
