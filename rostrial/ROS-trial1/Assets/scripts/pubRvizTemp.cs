using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using ROS;
using UnityEngine;
public class pubRvizTemp : MonoBehaviour
{
    // Start is called before the first frame update
    GenericPub<TempMsg> pub = null;
    [SerializeField]
    public double temp = 37;
    public GameObject cube;
    void Start()
    {
        pub = new GenericPub<TempMsg>("sensor_msgs/Temperature", 0.5f);
        InvokeRepeating("publishTemp", 1, 0.5f);
    }

    // Update is called once per frame

    public void publishTemp()
    {
        //float time1 = Time.time;
        //Debug.Log(time1);
        TempMsg temp = new TempMsg( this.temp , (double) 0);
        pub.Publish(temp);
    }
}
