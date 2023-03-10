namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(GameBoySettings))]
    public class GameBoyEditor : VolumeComponentEditor
    {
        SerializedDataParameter enabled;
        SerializedDataParameter darkestColor;
        SerializedDataParameter darkColor;
        SerializedDataParameter lightColor;
        SerializedDataParameter lightestColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<GameBoySettings>(serializedObject);
            enabled = Unpack(o.Find(x => x.enabled));
            darkestColor = Unpack(o.Find(x => x.darkestColor));
            darkColor = Unpack(o.Find(x => x.darkColor));
            lightColor = Unpack(o.Find(x => x.lightColor));
            lightestColor = Unpack(o.Find(x => x.lightestColor));
        }

        public override void OnInspectorGUI()
        {
            if (!CheckEffectEnabled())
            {
                EditorGUILayout.HelpBox("The GameBoy effect must be added to your renderer's Renderer Features list.", MessageType.Error);
            }

            PropertyField(enabled);
            PropertyField(darkestColor);
            PropertyField(darkColor);
            PropertyField(lightColor);
            PropertyField(lightestColor);
        }

        // Check the Forward Renderer and make sure the GameBoy effect is attached.
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
                if (item?.GetType() == typeof(GameBoy))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Game Boy");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Game Boy";
    }
#endif
    }
}
