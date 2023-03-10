namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;

    [VolumeComponentEditor(typeof(BlurSettings))]
    public class BlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<BlurSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Blur effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
        }

        // Check the Forward Renderer and make sure the Blur effect is attached.
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
                if (item?.GetType() == typeof(Blur))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Blur");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Blur";
    }
#endif
    }
}
