using Unity.Robotics.ROSTCPConnector;

using ROS;
using UnityEngine;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
public class pubRvizTemp : MonoBehaviour
{
    // Start is called before the first frame update
    GenericPub<TemperatureMsg> pub = null;
    [SerializeField]
    public double temp = 37;
    [SerializeField]
    public GameObject TempSensor;
    [SerializeField]
    public string PubTopic = "robot1/sensor_msgs/Temperature";
    [SerializeField]
    public string frame_id = "TempSensor";
    void Start()
    {
        pub = new GenericPub<TemperatureMsg>(PubTopic, 0.5f);
        InvokeRepeating("publishTemp", 1, 0.5f);
    }

    // Update is called once per frame

    public void publishTemp()
    {
        //float time1 = Time.time;
        //Debug.Log(time1);
        TemperatureMsg msg = new TemperatureMsg();
        msg.temperature = this.temp;
        msg.variance = (double)0;
        HeaderMsg header = new HeaderMsg();
        uint sec, nanosec;
        mTime.Now(out sec, out nanosec);
        header.stamp.sec = sec;
        header.stamp.nanosec = nanosec;
        header.frame_id = frame_id;
        msg.header = header;
       // TempMsg temp = new TempMsg( this.temp , (double) 0);
        pub.Publish(msg);
    }
}
