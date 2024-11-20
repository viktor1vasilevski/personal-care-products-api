﻿using Main.Interfaces;

namespace Main.Services;

public class ImageService : IImageService
{
    public byte[] ConvertBase64ToBytes(string base64String)
    {
        if (string.IsNullOrEmpty(base64String)) return null;

        string base64Data = base64String.Contains("base64,")
            ? base64String.Substring(base64String.IndexOf("base64,") + 7)
            : base64String;

        return Convert.FromBase64String(base64Data);
    }
}
