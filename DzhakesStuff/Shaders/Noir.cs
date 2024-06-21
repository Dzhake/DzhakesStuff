using System.Collections;
using UnityEngine;

namespace DzhakesStuff
{
    public class Noir : MonoBehaviour
    {
        private Material? material;

        public IEnumerator Start()
        {
            yield return new WaitUntil(() => Core.Shaders != null);
            material = Core.Shaders!.LoadAsset<Material>("Noir");
        }

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (material is not null && material && NoirMutator.Instance!.IsEnabled())
            {
                Graphics.Blit(src, dest, material);
            }
            else Graphics.Blit(src, dest);
        }
    }
}
