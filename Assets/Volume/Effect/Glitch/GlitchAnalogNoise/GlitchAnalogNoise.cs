using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace XPostProcessing
{
    [VolumeComponentMenu(VolumeDefine.Glitch + "模拟噪点故障 (Analog Noise Glitch)")]
    public class GlitchAnalogNoise : VolumeSetting
    {
        public override bool IsActive() => NoiseFading.value > 0;

        public FloatParameter NoiseFading = new ClampedFloatParameter(0f, 0f, 1f);
        public FloatParameter NoiseSpeed = new ClampedFloatParameter(0.5f, 0f, 1f);
        public FloatParameter LuminanceJitterThreshold = new ClampedFloatParameter(0.8f, 0f, 1f);
    }


    public class GlitchAnalogNoiseRenderer : VolumeRenderer<GlitchAnalogNoise>
    {
        private const string PROFILER_TAG = "GlitchAnalogNoise";
        private Shader shader;
        private Material m_BlitMaterial;

        private float TimeX = 1.0f;


        public override void Init()
        {
            shader = Shader.Find("Hidden/PostProcessing/Glitch/AnalogNoise");
            m_BlitMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
        }


        public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target)
        {
            if (m_BlitMaterial == null)
                return;


            cmd.BeginSample(PROFILER_TAG);

            TimeX += Time.deltaTime;
            if (TimeX > 100)
            {
                TimeX = 0;
            }

            m_BlitMaterial.SetVector(ShaderIDs.Params, new Vector4(settings.NoiseSpeed.value, settings.NoiseFading.value, settings.LuminanceJitterThreshold.value, TimeX));

            cmd.Blit(source, target, m_BlitMaterial);

            cmd.EndSample(PROFILER_TAG);
        }


    }

}