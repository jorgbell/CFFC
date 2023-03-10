namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(ColorizeSettings))]
    public class ColorizeEditor : VolumeComponentEditor
    {
        SerializedDataParameter tintColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<ColorizeSettings>(serializedObject);
            tintColor = Unpack(o.Find(x => x.tintColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Colorize effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(tintColor);
        }

        // Check the Forward Renderer and make sure the Colorize effect is attached.
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
                if (item?.GetType() == typeof(Colorize))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Colorize");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Colorize";
    }
#endif
    }
}
