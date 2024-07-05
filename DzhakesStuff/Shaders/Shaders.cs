using UnityEngine;

namespace DzhakesStuff
{
    public class NoirShader : CustomShader
    {
        protected override string Name => "Noir";

        public override bool Active()
        {
            return NoirMutator.Instance!.IsEnabled();
        }
    }

    public class TintShader : CustomShader
    {
        public static Color Color = Color.grey;
        protected override string Name => "Tint";

        public override bool Active()
        {
            return Color != Color.grey;
        }

        protected override void SetValues(Material mat)
        {
            mat.SetColor("_Color", Color);
        }
    }

    public class BlackWhiteShader : CustomShader
    {
        protected override string Name => "BlackWhite";

        public override bool Active()
        {
            return ChronomanticDilation.Casting;
        }

        protected override void SetValues(Material mat)
        {
            mat.SetFloat("_EffectTime", ChronomanticDilation.Density);
        }
    }

    public class DisplaceShader : CustomShader
    {
        public static float X;
        public static float Y;
        protected override string Name => "Displace";

        public override bool Active()
        {
            return X != 0f && Y != 0f;
        }

        protected override void SetValues(Material mat)
        {
            X %= 1;
            Y %= 1;
            mat.SetFloat("_X", X);
            mat.SetFloat("_Y", Y);
        }
    }

    public class D3Shader : CustomShader
    {
        public static float Strength = 0f;
        public static float Red = 1f;
        public static float Green = 0f;
        public static float Blue = 0f;
        protected override string Name => "3D";

        public override bool Active()
        {
            return Strength != 0;
        }

        protected override void SetValues(Material mat)
        {
            mat.SetFloat("_Strength", Strength);
            mat.SetFloat("_Red", Red);
            mat.SetFloat("_Green", Green);
            mat.SetFloat("_Blue", Blue);
        }
    }

    public class BlurShader : CustomShader
    {
        public static float BlurSize = 0f;
        protected override string Name => "Blur";

        public override bool Active()
        {
            return BlurSize != 0;
        }

        protected override void SetValues(Material mat)
        {
            mat.SetFloat("_BlurSize", BlurSize);
        }
    }
}
