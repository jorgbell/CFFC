namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(BarrelDistortionSettings))]
    public class BarrelDistortionEditor : VolumeComponentEditor
    {
        SerializedDataParameter strength;
        SerializedDataParameter backgroundColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<BarrelDistortionSettings>(serializedObject);
            strength = Unpack(o.Find(x => x.strength));
            backgroundColor = Unpack(o.Find(x => x.backgroundColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Barrel Distortion effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(strength);
            PropertyField(backgroundColor);
        }

        // Check the Forward Renderer and make sure the BarrelDistortion effect is attached.
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
                if (item?.GetType() == typeof(BarrelDistortion))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Barrel Distortion");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Barrel Distortion";
    }
#endif
    }
}
