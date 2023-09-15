using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class IconMaker : MonoBehaviour
{
    public bool create;
    public RenderTexture renderTexture;
    public Camera baker;
    private static int iconCreated = 0;
    public string spriteName;

    private void Update()
    {
        if (create)
        {
            create = false;
            CreateIcon();  
        }
    }

    private void CreateIcon()
    {
        var name = spriteName;
        spriteName += $"{iconCreated}";
        string locationPath = SaveLocation();
        locationPath += spriteName;

        baker.targetTexture = renderTexture;

        var current = RenderTexture.active;
        baker.targetTexture.Release();
        RenderTexture.active = baker.targetTexture;
        baker.Render();

        var texture = new Texture2D(baker.targetTexture.width, baker.targetTexture.height, TextureFormat.ARGB32,false);
        texture.ReadPixels(new Rect(0,0, baker.targetTexture.width, baker.targetTexture.height), 0,0);
        texture.Apply();
        RenderTexture.active = current;
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(locationPath + ".png", bytes);

        Debug.Log("Created" + " " + locationPath);
        spriteName = name;

        iconCreated++;
    }

    private string SaveLocation()
    {
        string path = Application.streamingAssetsPath + "/Icons";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
}
