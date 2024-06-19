using System.Collections;
using UnityEngine;

namespace DzhakesStuff
{
    public class BlackWhite : MonoBehaviour
    {
        private float RedRatio = 1f;
        private float GreenRatio = 1f;
        private float BlueRatio = 1f;
        private Material? material;

        public IEnumerator Start()
        {
            AssetBundleCreateRequest req = AssetBundle.LoadFromMemoryAsync(Properties.Resources.ShadersBundle);
            yield return req;
            material = req.assetBundle.LoadAsset<Material>("BlackWhite");
        }

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (material is not null && material && ChronomanticDilation.Casting)
            {
                material.SetFloat("_RedRation", RedRatio);
                material.SetFloat("_GreenRatio", GreenRatio);
                material.SetFloat("_BlueRatio", BlueRatio);
                material.SetFloat("_EffectTime", ChronomanticDilation.Density);
                Graphics.Blit(src, dest, material);
            }
            else Graphics.Blit(src, dest);
        }
    }
}
