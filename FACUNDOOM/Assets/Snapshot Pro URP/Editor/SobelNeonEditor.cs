namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(SobelNeonSettings))]
    public class SobelNeonEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter saturationFloor;
        SerializedDataParameter lightnessFloor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SobelNeonSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            saturationFloor = Unpack(o.Find(x => x.saturationFloor));
            lightnessFloor = Unpack(o.Find(x => x.lightnessFloor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The SobelNeon effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(saturationFloor);
            PropertyField(lightnessFloor);
        }

        // Check the Forward Renderer and make sure the SobelNeon effect is attached.
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
                if (item?.GetType() == typeof(SobelNeon))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Neon (Sobel)");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Neon (Sobel)";
    }
#endif
    }
}
