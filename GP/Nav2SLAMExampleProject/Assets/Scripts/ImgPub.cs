using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using ROS;
using System.Threading;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class ImgPub : MonoBehaviour
{
    private GenericPub<ImageMsg> pub = null;
    public Camera cam;
    public string topicName = "cam";
    public float publishFrequency = 0.5f;
    private Texture2D mainCameraTexture;
    private Rect frame;
    private int frame_width;
    private int frame_height;
    private const int isBigEndian = 0;
    private uint image_step = 4;
    public string FrameId = "kinect_link";
    private Thread publishThread;
    private bool isPublishing = false;

    private void Start()
    {
        pub = new GenericPub<ImageMsg>(topicName, publishFrequency);
        StartPublishingThread();
    }

    private void StartPublishingThread()
    {
        isPublishing = true;
        publishThread = new Thread(PublishImageThread);
        publishThread.Start();
    }

    private void PublishImageThread()
    {
        while (isPublishing)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(PublishImage);
            Thread.Sleep((int)(1f / publishFrequency * 1000)); // Delay based on publish frequency
        }
    }

    private void PublishImage()
    {
        if (cam == null)
        {
            Debug.Log("No camera is specified.");
            return;
        }

        RenderTexture renderTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm);
        renderTexture.Create();

        frame_width = renderTexture.width;
        frame_height = renderTexture.height;

        frame = new Rect(0, 0, frame_width, frame_height);

        mainCameraTexture = new Texture2D(frame_width, frame_height, TextureFormat.RGBA32, false);

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            cam.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            cam.Render();

            mainCameraTexture.ReadPixels(frame, 0, 0);
            mainCameraTexture.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            byte[] dataptr = mainCameraTexture.GetRawTextureData();

            ImageMsg image = new ImageMsg();
            image.header.frame_id = FrameId;
            image.height = (uint)frame_height;
            image.width = (uint)frame_width;
            image.encoding = "rgba8";
            image.step = image_step * (uint)frame_width;
            image.is_bigendian = isBigEndian;
            image.data = dataptr;

            pub.Publish(image);
            Debug.Log(image);
        });
    }

    private void OnDisable()
    {
        isPublishing = false;
        if (publishThread != null && publishThread.IsAlive)
        {
            publishThread.Join();
        }
    }
}
