using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace XPostProcessing
{
    [VolumeComponentMenu(VolumeDefine.Vignette + "快速渐晕 (Rapid Vignette)")]
    public class RapidVignette : VolumeSetting
    {
        public override bool IsActive() => vignetteIndensity.value > 0;

        public VignetteTypeParameter vignetteType = new VignetteTypeParameter(VignetteType.ClassicMode);
        public FloatParameter vignetteIndensity = new ClampedFloatParameter(0f, 0f, 5f);

        public Vector2Parameter vignetteCenter = new Vector2Parameter(new Vector2(0.5f, 0.5f));
        public ColorParameter vignetteColor = new ColorParameter(new Color(0.1f, 0.8f, 1.0f), true, true, true);
    }

    public class RapidVignetteRenderer : VolumeRenderer<RapidVignette>
    {
        private const string PROFILER_TAG = "RapidVignette";
        private Shader shader;
        private Material m_BlitMaterial;


        public override void Init()
        {
            shader = Shader.Find("Hidden/PostProcessing/Vignette/RapidVignette");
            m_BlitMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        static class ShaderIDs
        {
            internal static readonly int VignetteIndensity = Shader.PropertyToID("_VignetteIndensity");
            internal static readonly int VignetteCenter = Shader.PropertyToID("_VignetteCenter");
            internal static readonly int VignetteColor = Shader.PropertyToID("_VignetteColor");
        }


        public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target)
        {
            if (m_BlitMaterial == null)
                return;

            cmd.BeginSample(PROFILER_TAG);

            m_BlitMaterial.SetFloat(ShaderIDs.VignetteIndensity, settings.vignetteIndensity.value);
            m_BlitMaterial.SetVector(ShaderIDs.VignetteCenter, settings.vignetteCenter.value);

            if (settings.vignetteType.value == VignetteType.ColorMode)
            {
                m_BlitMaterial.SetVector(ShaderIDs.VignetteColor, settings.vignetteColor.value);
            }

            cmd.Blit(source, target, m_BlitMaterial, (int)settings.vignetteType.value);

            cmd.EndSample(PROFILER_TAG);
        }
    }

}