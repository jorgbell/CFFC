namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(MosaicSettings))]
    public class MosaicEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter overlayTexture;
        SerializedDataParameter overlayColor;
        SerializedDataParameter xTileCount;
        SerializedDataParameter usePointFiltering;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<MosaicSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            overlayTexture = Unpack(o.Find(x => x.overlayTexture));
            overlayColor = Unpack(o.Find(x => x.overlayColor));
            xTileCount = Unpack(o.Find(x => x.xTileCount));
            usePointFiltering = Unpack(o.Find(x => x.usePointFiltering));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Mosaic effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(overlayTexture);
            PropertyField(overlayColor);
            PropertyField(xTileCount);
            PropertyField(usePointFiltering);
        }

        // Check the Forward Renderer and make sure the Mosaic effect is attached.
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
                if (item?.GetType() == typeof(Mosaic))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Mosaic");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Mosaic";
    }
#endif
    }
}
