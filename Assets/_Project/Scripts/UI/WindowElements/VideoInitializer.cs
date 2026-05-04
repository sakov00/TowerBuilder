using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoInitializer : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private int _width = 256;
    [SerializeField] private int _height = 256;

    [SerializeField] private VideoPlayer _videoPlayer;
    private RenderTexture _renderTexture;

    private void Awake()
    {
        _renderTexture = new RenderTexture(_width, _height, 16, RenderTextureFormat.ARGB32);
        _renderTexture.name = "Reflection_RT";
        _renderTexture.Create();

        _videoPlayer.targetTexture = _renderTexture;
        _rawImage.texture = _renderTexture;
    }

    private void Start()
    {
        _videoPlayer.Play();
    }

    private void OnDestroy()
    {
        if (_renderTexture != null)
        {
            _renderTexture.Release();
            Destroy(_renderTexture);
        }
    }
}