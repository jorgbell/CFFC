namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(RadialBlurSettings))]
    public class RadialBlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter focalSize;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<RadialBlurSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            focalSize = Unpack(o.Find(x => x.focalSize));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The RadialBlur effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
            PropertyField(focalSize);
        }

        // Check the Forward Renderer and make sure the Radial Blur effect is attached.
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
                if (item?.GetType() == typeof(RadialBlur))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Radial Blur");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Radial Blur";
    }
#endif
    }
}
