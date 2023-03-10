namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(DrawingSettings))]
    public class DrawingEditor : VolumeComponentEditor
    {
        SerializedDataParameter drawingTex;
        SerializedDataParameter animCycleTime;
        SerializedDataParameter strength;
        SerializedDataParameter tiling;
        SerializedDataParameter smudge;
        SerializedDataParameter depthThreshold;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<DrawingSettings>(serializedObject);
            drawingTex = Unpack(o.Find(x => x.drawingTex));
            animCycleTime = Unpack(o.Find(x => x.animCycleTime));
            strength = Unpack(o.Find(x => x.strength));
            tiling = Unpack(o.Find(x => x.tiling));
            smudge = Unpack(o.Find(x => x.smudge));
            depthThreshold = Unpack(o.Find(x => x.depthThreshold));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The Drawing effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(drawingTex);
            PropertyField(animCycleTime);
            PropertyField(strength);
            PropertyField(tiling);
            PropertyField(smudge);
            PropertyField(depthThreshold);
        }

        // Check the Forward Renderer and make sure the Drawing effect is attached.
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
                if (item?.GetType() == typeof(Drawing))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Drawing");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Drawing";
    }
#endif
    }
}
