namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable, VolumeComponentMenu("Snapshot Shaders Pro/SobelOutline")]
    public sealed class SobelOutlineSettings : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("Is the effect active?")]
        public BoolParameter enabled = new BoolParameter(false);

        [Tooltip("Edge-detection threshold.")]
        public ClampedFloatParameter threshold = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

        [Tooltip("Outline color.")]
        public ColorParameter outlineColor = new ColorParameter(Color.white);

        [Tooltip("Background color.")]
        public ColorParameter backgroundColor = new ColorParameter(Color.black);

        public bool IsActive()
        {
            return enabled.value && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}
