namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(SharpenSettings))]
    public class SharpenEditor : VolumeComponentEditor
    {
        SerializedDataParameter intensity;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SharpenSettings>(serializedObject);
            intensity = Unpack(o.Find(x => x.intensity));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Sharpen effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(intensity);
        }

        // Check the Forward Renderer and make sure the Sharpen effect is attached.
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
                if (item?.GetType() == typeof(Sharpen))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Sharpen");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Sharpen";
    }
#endif
    }
}
