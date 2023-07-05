using Unity.Robotics.ROSTCPConnector;

using ROS;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
public class ImgPub : MonoBehaviour
{
    GenericPub<ImageMsg> pub = null;
    [SerializeField]
    [Tooltip("rotate camera 180 degree around Z axis")]
    public Camera cam;
    [SerializeField]
    public string topicName = "cam";
    [SerializeField]
    public float publishFrequency = 0.5f;
    private RenderTexture renderTexture;
    private RenderTexture lastTexture;

    private Texture2D mainCameraTexture;
    private Rect frame;


    private int frame_width;
    private int frame_height;
    private const int isBigEndian = 0;
    private uint image_step = 4;

    private ImageMsg image;
    // Start is called before the first frame update
    void Start()
    {
        pub = new GenericPub<ImageMsg>(topicName, publishFrequency);
        InvokeRepeating("publishImg", 1, 0.5f);


    }
    public void publishImg()
    {
        if (!cam)
        {
            Debug.Log("no camera is specified ");
        }
        else
        {
            renderTexture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm);
            renderTexture.Create();

            frame_width = renderTexture.width;
            frame_height = renderTexture.height;

            /*area to be saved from the renderTexture*/
            frame = new Rect(0, 0, frame_width, frame_height);

            /*this is where the renderTexture will be saved */
            mainCameraTexture = new Texture2D(frame_width, frame_height, TextureFormat.RGBA32, false);

            /*getting camera render to renderTexture that we created instead of displaying it on screen  */
            cam.targetTexture = renderTexture;

            /*saving the active render texture in a variable */
            lastTexture = RenderTexture.active;

            /*put the camera texture as the active render */
            RenderTexture.active = renderTexture;

            /*This line tells the camera to render the scene onto the RenderTexture*/
            cam.Render();

            /*saving the render value in the variable that we created*/
            mainCameraTexture.ReadPixels(frame, 0, 0);
            mainCameraTexture.Apply();
            /*restore the active render that was working back*/
            cam.targetTexture = lastTexture;
            /*after rendring finshies get cam render to null */
            cam.targetTexture = null;

            /*data ptr */
            byte[] dataptr = mainCameraTexture.GetRawTextureData();
            
            /*,(uint)frame_height, (uint)frame_width, "rgba8", image_step * (uint)frame_width, dataptr,*/
            image = new ImageMsg();
            image.header.frame_id = "camera";
            image.height = (uint)frame_height;
            image.width = (uint)frame_width;
            image.encoding = "rgba8";
            image.step = image_step * (uint)frame_width;
            image.is_bigendian = isBigEndian;
            image.data = dataptr;
            pub.Publish(image);
            Debug.Log(image);
        }


    }
}
    // Update is called once per frame
  /*  void LateUpdate()
    {
        var imagesenth = GetComponent<ImageSynthesis>();
        imagesenth.Save("ZOZ.png", 1080, 920, "Captures");
    }
}*/
