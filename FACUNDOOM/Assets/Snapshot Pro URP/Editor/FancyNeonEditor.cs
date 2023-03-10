namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(FancyNeonSettings))]
    public class FancyNeonEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter colorSensitivity;
        SerializedDataParameter colorStrength;
        SerializedDataParameter depthSensitivity;
        SerializedDataParameter depthStrength;
        SerializedDataParameter normalSensitivity;
        SerializedDataParameter normalStrength;
        SerializedDataParameter depthThreshold;
        SerializedDataParameter saturationFloor;
        SerializedDataParameter lightnessFloor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<FancyNeonSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            colorSensitivity = Unpack(o.Find(x => x.colorSensitivity));
            colorStrength = Unpack(o.Find(x => x.colorStrength));
            depthSensitivity = Unpack(o.Find(x => x.depthSensitivity));
            depthStrength = Unpack(o.Find(x => x.depthStrength));
            normalSensitivity = Unpack(o.Find(x => x.normalSensitivity));
            normalStrength = Unpack(o.Find(x => x.normalStrength));
            depthThreshold = Unpack(o.Find(x => x.depthThreshold));
            saturationFloor = Unpack(o.Find(x => x.saturationFloor));
            lightnessFloor = Unpack(o.Find(x => x.lightnessFloor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The FancyNeon effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(colorSensitivity);
            PropertyField(colorStrength);
            PropertyField(depthSensitivity);
            PropertyField(depthStrength);
            PropertyField(normalSensitivity);
            PropertyField(normalStrength);
            PropertyField(depthThreshold);
            PropertyField(saturationFloor);
            PropertyField(lightnessFloor);
        }

        // Check the Forward Renderer and make sure the FancyNeon effect is attached.
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
                if (item?.GetType() == typeof(FancyNeon))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Fancy Neon");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Neon (Fancy)";
    }
#endif
    }
}
