namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(SNESSettings))]
    public class SNESEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter bandingValues;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SNESSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            bandingValues = Unpack(o.Find(x => x.bandingValues));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The SNES effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(bandingValues);
        }

        // Check the Forward Renderer and make sure the SNES effect is attached.
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
                if (item?.GetType() == typeof(SNES))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("SNES");
        }
#else
    public override string GetDisplayTitle()
    {
        return "SNES";
    }
#endif
    }
}
