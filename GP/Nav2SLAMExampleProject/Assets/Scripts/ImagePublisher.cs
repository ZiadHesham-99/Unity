
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using ROS;
/// <summary>
///     This script publishes camera view
///     as compressed image 
/// </summary>
public class ImagePublisher : MonoBehaviour
{
    // ROS Connectors
    GenericPub<CompressedImageMsg> pub = null;
    // Variables required for ROS communication
    [SerializeField]
    public string cameraTopicName = "camera/color/image_raw/compressed";

    // Sensor
    public Camera imageCamera;
    public int resolutionWidth = 1280;
    public int resolutionHeight = 720;
    public int qualityLevel = 50;

    // Message
    private CompressedImageMsg compressedImage;
    [SerializeField]
    public string frameID = "camera";

    private Texture2D texture2D;
    private Rect rect;

    void Start()
    {
        // Get ROS connection static instance
        pub = new GenericPub<CompressedImageMsg>(cameraTopicName, 0.5f);
        

        // Initialize renderer
        texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.ARGB32, false);
        rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
        RenderTexture renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24,
                                                        RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        imageCamera.targetTexture = renderTexture;

        // Initialize messages
        compressedImage = new CompressedImageMsg();
        compressedImage.header.frame_id = frameID;
        compressedImage.format = "jpeg";

        // Call back
        Camera.onPostRender += UpdateImage;
    }

    private void UpdateImage(Camera cameraObject)
    {
        if (texture2D != null && cameraObject == imageCamera)
        {
            CompressedImageMsg msg = new CompressedImageMsg();
            msg.header.frame_id = frameID;
            msg.format = "jpeg";
            texture2D.ReadPixels(rect, 0, 0);
            msg.data = texture2D.EncodeToJPG(qualityLevel);

            pub.Publish(msg );
        }
    }

}
