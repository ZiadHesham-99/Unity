using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using ROS;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
public class DepthPub : MonoBehaviour
{
    GenericPub<ImageMsg> pub = null;
    [SerializeField]
    [Tooltip("rotate camera 180 degree around Z axis")]
    public Camera cam;
    [SerializeField]
    public string topicName = "";
    [SerializeField]
    public float publishFrequency = 0.5f;
    private int frame_width;
    private int frame_height;
    private const int isBigEndian = 0;
    private uint image_step = 1;
    private ImageMsg image;
    byte[] alphaBytes;
    // Start is called before the first frame update
    void Start()
    {
        pub = new GenericPub<ImageMsg>(topicName, publishFrequency);

        InvokeRepeating("publishImg", 1, 0.5f);
    }

    // Update is called once per frame
    public void publishImg()
    {
        if (!cam)
        {
            Debug.Log("no camera is specified ");
        }
        else
        {
            image = new ImageMsg();
            image.header.frame_id = "camera";
            image.height = (uint)frame_height;
            image.width = (uint)frame_width;
            image.encoding = "16UC1";
            image.step = image_step * (uint)frame_width;
            image.is_bigendian = isBigEndian;
            GetDepthImageInBytes();
            image.data = alphaBytes;
            pub.Publish(image);
            Debug.Log(image);
        }
    }
    public void GetDepthImageInBytes()
    {
        var depthTexture = cam.targetTexture;
        image.height = (uint)depthTexture.height;
        image.width = (uint)depthTexture.width;

        // Convert depthTexture to Texture2D
        var depthImage = new Texture2D(depthTexture.width, depthTexture.height);
        RenderTexture.active = depthTexture;
        depthImage.ReadPixels(new Rect(0, 0, depthTexture.width, depthTexture.height), 0, 0);
        depthImage.Apply();

        // Create a new texture to store only the alpha component
        //var alphaTexture = new Texture2D(depthImage.width, depthImage.height, TextureFormat.RGBA64, false);
        var pixels = depthImage.GetPixels();
         alphaBytes = new byte[pixels.Length*2];

        for (int i = 0; i < pixels.Length; i+=2)
        {
            var pixelalpha = (int)pixels[i].a;
            alphaBytes[i] = (byte)pixelalpha;
            alphaBytes[i + 1] = (byte)(pixelalpha >> 8);
        }

       // alphaTexture.SetPixels(pixels);
        //alphaTexture.Apply();

        // Convert alphaTexture to byte[]
        Debug.Log(alphaBytes.Length);

        
    }
}
