namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(GreyscaleSettings))]
    public class GreyscaleEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<GreyscaleSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Greyscale effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
        }

        // Check the Forward Renderer and make sure the Greyscale effect is attached.
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
                if (item?.GetType() == typeof(Greyscale))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Greyscale");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Greyscale";
    }
#endif
    }
}
