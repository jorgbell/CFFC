namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(CutoutSettings))]
    public class CutoutEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter cutoutTexture;
        SerializedDataParameter borderColor;
        SerializedDataParameter stretch;
        SerializedDataParameter zoom;
        SerializedDataParameter offset;
        SerializedDataParameter rotation;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<CutoutSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            cutoutTexture = Unpack(o.Find(x => x.cutoutTexture));
            borderColor = Unpack(o.Find(x => x.borderColor));
            stretch = Unpack(o.Find(x => x.stretch));
            zoom = Unpack(o.Find(x => x.zoom));
            offset = Unpack(o.Find(x => x.offset));
            rotation = Unpack(o.Find(x => x.rotation));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Cutout effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(cutoutTexture);
            PropertyField(borderColor);
            PropertyField(stretch);
            PropertyField(zoom);
            PropertyField(offset);
            PropertyField(rotation);
        }

        // Check the Forward Renderer and make sure the Cutout effect is attached.
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
                if (item?.GetType() == typeof(Cutout))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Cutout");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Cutout";
    }
#endif
    }
}
