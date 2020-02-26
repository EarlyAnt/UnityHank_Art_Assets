using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hank
{
    public class FixShader : MonoBehaviour
    {
        private List<Material> materials;
        private List<string> shaders;

        private void Start()
        {
            materials = new List<Material>();
            shaders = new List<string>();

            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();
            int length = meshRenderer.Length;

            for (int i = 0; i < length; i++)
            {
                int count = meshRenderer[i].materials.Length;
                for (int j = 0; j < count; j++)
                {
                    Material _mater = meshRenderer[i].materials[j];
                    materials.Add(_mater);
                    shaders.Add(_mater.shader.name);
                }
            }

            SkinnedMeshRenderer[] meshSkinRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
            length = meshSkinRenderer.Length;

            for (int i = 0; i < length; i++)
            {
                int count = meshSkinRenderer[i].materials.Length;
                for (int j = 0; j < count; j++)
                {
                    Material _mater = meshSkinRenderer[i].materials[j];
                    materials.Add(_mater);
                    shaders.Add(_mater.shader.name);
                }
            }

            ParticleSystem[] particleSystemObjects = GetComponentsInChildren<ParticleSystem>();
            length = particleSystemObjects.Length;

            for (int i = 0; i < length; i++)
            {
                ParticleSystemRenderer renderer = particleSystemObjects[i].GetComponent<ParticleSystemRenderer>();
                if (renderer.material != null)
                {
                    materials.Add(renderer.material);
                    shaders.Add(renderer.material.shader.name);
                }

                if (renderer.trailMaterial != null)
                {
                    materials.Add(renderer.trailMaterial);
                    shaders.Add(renderer.trailMaterial.shader.name);
                }
            }

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].shader = Shader.Find(shaders[i]);
            }
        }
    }
}
