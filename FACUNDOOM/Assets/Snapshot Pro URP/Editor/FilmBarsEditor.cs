namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(FilmBarsSettings))]
    public class FilmBarsEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter aspect;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<FilmBarsSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            aspect = Unpack(o.Find(x => x.aspect));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The FilmBars effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(aspect);
        }

        // Check the Forward Renderer and make sure the FilmBars effect is attached.
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
                if (item?.GetType() == typeof(FilmBars))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Film Bars");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Film Bars";
    }
#endif
    }
}
