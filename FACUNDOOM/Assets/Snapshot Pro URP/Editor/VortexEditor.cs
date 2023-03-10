namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(VortexSettings))]
    public class VortexEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter center;
        SerializedDataParameter offset;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<VortexSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            center = Unpack(o.Find(x => x.center));
            offset = Unpack(o.Find(x => x.offset));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Vortex effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
            PropertyField(center);
            PropertyField(offset);
        }

        // Check the Forward Renderer and make sure the Vortex effect is attached.
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
                if (item?.GetType() == typeof(Vortex))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Vortex");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Vortex";
    }
#endif
    }
}
