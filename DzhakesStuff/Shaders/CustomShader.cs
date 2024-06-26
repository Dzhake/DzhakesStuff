using System.Collections;
using UnityEngine;

namespace DzhakesStuff
{
    public abstract class CustomShader : MonoBehaviour
    {
        protected Material? material;
        public abstract bool Active();
        protected virtual void SetValues(Material mat) {}
        protected abstract string Name { get; }

        public IEnumerator Start()
        {
            yield return new WaitUntil(() => Core.Shaders != null);
            material = Core.Shaders!.LoadAsset<Material>(Name);
        }

        public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (material is not null && Active())
            {
                SetValues(material);
                Graphics.Blit(src, dest, material);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }
    }
}
