using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectEffect : EffectBase {
    public float intensity;
    public float xspeed;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _Material.SetFloat("_Intensity", intensity);
        _Material.SetFloat("_XSpeed", xspeed);
        Graphics.Blit(source, destination, _Material);
    }
}
