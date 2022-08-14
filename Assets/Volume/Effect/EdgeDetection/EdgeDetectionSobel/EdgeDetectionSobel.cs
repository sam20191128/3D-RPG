using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace XPostProcessing
{
    [VolumeComponentMenu(VolumeDefine.EdgeDetection + "Sobel")]
    public class EdgeDetectionSobel : VolumeSetting
    {
        public override bool IsActive() => edgeWidth.value > 0;

        public ClampedFloatParameter edgeWidth = new ClampedFloatParameter(0f, 0f, 5f);
        public ColorParameter edgeColor = new ColorParameter(new Color(0.0f, 0.0f, 0.0f, 1), true, true, true);
        public ClampedFloatParameter backgroundFade = new ClampedFloatParameter(0f, 0f, 1f);
        public ColorParameter backgroundColor = new ColorParameter(new Color(1.0f, 1.0f, 1.0f, 1), true, true, true);
    }

    public class EdgeDetectionSobelRenderer : VolumeRenderer<EdgeDetectionSobel>
    {
        private const string PROFILER_TAG = "EdgeDetectionSobel";
        private Shader shader;
        private Material m_BlitMaterial;

        public override void Init()
        {
            shader = Shader.Find("Hidden/PostProcessing/EdgeDetection/Sobel");
            m_BlitMaterial = CoreUtils.CreateEngineMaterial(shader);
        }

        static class ShaderIDs
        {
            internal static readonly int Params = Shader.PropertyToID("_Params");
            internal static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
            internal static readonly int BackgroundColor = Shader.PropertyToID("_BackgroundColor");
        }


        public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier target)
        {
            if (m_BlitMaterial == null)
                return;

            cmd.BeginSample(PROFILER_TAG);


            m_BlitMaterial.SetVector(ShaderIDs.Params, new Vector2(settings.edgeWidth.value, settings.backgroundFade.value));
            m_BlitMaterial.SetColor(ShaderIDs.EdgeColor, settings.edgeColor.value);
            m_BlitMaterial.SetColor(ShaderIDs.BackgroundColor, settings.backgroundColor.value);

            cmd.Blit(source, target, m_BlitMaterial);

            cmd.EndSample(PROFILER_TAG);
        }
    }

}