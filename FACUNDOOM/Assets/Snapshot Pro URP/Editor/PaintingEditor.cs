namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;

    [VolumeComponentEditor(typeof(PaintingSettings))]
    public class PaintingEditor : VolumeComponentEditor
    {
        SerializedDataParameter kernelSize;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<PaintingSettings>(serializedObject);
            kernelSize = Unpack(o.Find(x => x.kernelSize));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Painting effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(kernelSize);
        }

        // Check the Forward Renderer and make sure the Painting effect is attached.
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
                if (item?.GetType() == typeof(Painting))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Painting");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Painting";
    }
#endif
    }
}
