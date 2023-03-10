namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(KaleidoscopeSettings))]
    public class KaleidoscopeEditor : VolumeComponentEditor
    {
        SerializedDataParameter segmentCount;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<KaleidoscopeSettings>(serializedObject);
            segmentCount = Unpack(o.Find(x => x.segmentCount));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Kaleidoscope effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(segmentCount);
        }

        // Check the Forward Renderer and make sure the Kaleidoscope effect is attached.
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
                if (item?.GetType() == typeof(Kaleidoscope))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Kaleidoscope");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Kaleidoscope";
    }
#endif
    }
}
