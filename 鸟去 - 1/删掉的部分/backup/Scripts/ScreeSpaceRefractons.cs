using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ScreeSpaceRefractons : MonoBehaviour {

    [HideInInspector]
    [SerializeField]
    Camera _camera;
    int _downResFactor = 1;
    string _globalTextureName = "_reflect";
    private void Start()
    {
        _camera = GetComponent<Camera>();
        GenerateRT();
    }
    void GenerateRT() {
        if (_camera.targetTexture != null) {
            RenderTexture temp = _camera.targetTexture;
            _camera.targetTexture = null;
            DestroyImmediate(temp);
        }
        _camera.targetTexture = new RenderTexture(_camera.pixelWidth>>_downResFactor,_camera.pixelHeight>>_downResFactor,16);
        _camera.targetTexture.filterMode = FilterMode.Point;
        Shader.SetGlobalTexture(_globalTextureName,_camera.targetTexture);
    }
}
