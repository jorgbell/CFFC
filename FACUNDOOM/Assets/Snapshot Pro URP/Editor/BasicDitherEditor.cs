namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(BasicDitherSettings))]
    public class BasicDitherEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter noiseTex;
        SerializedDataParameter noiseSize;
        SerializedDataParameter thresholdOffset;
        SerializedDataParameter darkColor;
        SerializedDataParameter lightColor;
        SerializedDataParameter useSceneColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<BasicDitherSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            noiseTex = Unpack(o.Find(x => x.noiseTex));
            noiseSize = Unpack(o.Find(x => x.noiseSize));
            thresholdOffset = Unpack(o.Find(x => x.thresholdOffset));
            darkColor = Unpack(o.Find(x => x.darkColor));
            lightColor = Unpack(o.Find(x => x.lightColor));
            useSceneColor = Unpack(o.Find(x => x.useSceneColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The BasicDither effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(noiseTex);
            PropertyField(noiseSize);
            PropertyField(thresholdOffset);
            PropertyField(darkColor);
            PropertyField(lightColor);
            PropertyField(useSceneColor);
        }

        // Check the Forward Renderer and make sure the BasicDither effect is attached.
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
                if (item?.GetType() == typeof(BasicDither))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Basic Dither");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Basic Dither";
    }
#endif
    }
}
