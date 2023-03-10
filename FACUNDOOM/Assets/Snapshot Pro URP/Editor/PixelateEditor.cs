namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(PixelateSettings))]
    public class PixelateEditor : VolumeComponentEditor
    {
        SerializedDataParameter pixelSize;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<PixelateSettings>(serializedObject);
            pixelSize = Unpack(o.Find(x => x.pixelSize));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Pixelate effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(pixelSize);
        }

        // Check the Forward Renderer and make sure the Pixelate effect is attached.
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
                if (item?.GetType() == typeof(Pixelate))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Pixelate");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Pixelate";
    }
#endif
    }
}
