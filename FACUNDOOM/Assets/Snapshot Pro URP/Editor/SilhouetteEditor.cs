namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(SilhouetteSettings))]
    public class SilhouetteEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter nearColor;
        SerializedDataParameter farColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<SilhouetteSettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            nearColor = Unpack(o.Find(x => x.nearColor));
            farColor = Unpack(o.Find(x => x.farColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Silhouette effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(nearColor);
            PropertyField(farColor);
        }

        // Check the Forward Renderer and make sure the Silhouette effect is attached.
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
                if (item?.GetType() == typeof(Silhouette))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Silhouette");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Silhouette";
    }
#endif
    }
}
