using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROS;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
public class IMUPub : MonoBehaviour
{
    // ROS Connector
    GenericPub<ImuMsg> pub = null;
    [SerializeField]
    public string topicName = "/robot1/imu";
    float publishRate = 0.5f;
    //object
    public GameObject IMUSensor;
    //variables to calculate acceleration 
    private Vector3 acceleration;
    private Vector3 lastVelocity = new Vector3(0, 0, 0);
    double[] covariance = new double[3];
    [SerializeField]
    public string FrameId = "IMUSensor";


    void Start()
    {
        // Get ROS connection 
        pub = new GenericPub<ImuMsg>(topicName, publishRate);
        InvokeRepeating("testz", 1, 0.5f);
    }

    public void testz()
    {
        ImuMsg msg = new ImuMsg();
        HeaderMsg header = new HeaderMsg();
        uint sec, nanosec;
        mTime.Now(out sec, out nanosec);
        header.stamp.sec = sec;
        header.stamp.nanosec = nanosec;
        header.frame_id = FrameId;
        msg.header= header;
        /*quaternion message*/
        msg.orientation.x = (double)IMUSensor.transform.rotation.x;
        msg.orientation.y = (double)IMUSensor.transform.rotation.y;
        msg.orientation.z = (double)IMUSensor.transform.rotation.z;
        msg.orientation.w = (double)IMUSensor.transform.rotation.w;
        /* angular velocity*/
        msg.angular_velocity.x = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.x;
        msg.angular_velocity.y = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.y;
        msg.angular_velocity.z = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.z;
        /*calculate linear acceleration */
        acceleration = (IMUSensor.GetComponent<ArticulationBody>().velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = IMUSensor.GetComponent<ArticulationBody>().velocity;
        (msg.linear_acceleration.x, msg.linear_acceleration.y, msg.linear_acceleration.z) = (acceleration.x, acceleration.y, acceleration.z);
        msg.orientation_covariance[0] = -1;
        msg.angular_velocity_covariance[0] = -1;
        msg.linear_acceleration_covariance[0] = -1;
        pub.Publish(msg);
    }
}
