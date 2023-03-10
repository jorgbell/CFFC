namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(UnderwaterSettings))]
    public class UnderwaterEditor : VolumeComponentEditor
    {
        SerializedDataParameter bumpMap;
        SerializedDataParameter strength;
        SerializedDataParameter waterFogColor;
        SerializedDataParameter fogStrength;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<UnderwaterSettings>(serializedObject);
            bumpMap = Unpack(o.Find(x => x.bumpMap));
            strength = Unpack(o.Find(x => x.strength));
            waterFogColor = Unpack(o.Find(x => x.waterFogColor));
            fogStrength = Unpack(o.Find(x => x.fogStrength));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Underwater effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(bumpMap);
            PropertyField(strength);
            PropertyField(waterFogColor);
            PropertyField(fogStrength);
        }

        // Check the Forward Renderer and make sure the Underwater effect is attached.
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
                if (item?.GetType() == typeof(Underwater))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Underwater");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Underwater";
    }
#endif
    }
}
