namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(ScanlinesSettings))]
    public class ScanlinesEditor : VolumeComponentEditor
    {
        SerializedDataParameter scanlineTex;
        SerializedDataParameter strength;
        SerializedDataParameter size;
        SerializedDataParameter scrollSpeed;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<ScanlinesSettings>(serializedObject);
            scanlineTex = Unpack(o.Find(x => x.scanlineTex));
            strength = Unpack(o.Find(x => x.strength));
            size = Unpack(o.Find(x => x.size));
            scrollSpeed = Unpack(o.Find(x => x.scrollSpeed));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Scanlines effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(scanlineTex);
            PropertyField(strength);
            PropertyField(size);
            PropertyField(scrollSpeed);
        }

        // Check the Forward Renderer and make sure the Scanlines effect is attached.
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
                if (item?.GetType() == typeof(Scanlines))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Scanlines");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Scanlines";
    }
#endif
    }
}
