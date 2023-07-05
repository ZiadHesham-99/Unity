using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using RosMessageTypes.Sensor;
using RosMessageTypes.Geometry;
using ROS;
/// <summary>
///     This script publishes robot stamped twist
///     with repect to the local robot frame
/// </summary>
public class IMUPub : MonoBehaviour
{
    // ROS Connector
    GenericPub<ImuMsg> pub = null;
    [SerializeField]
    public string topicName = "";
    float publishRate = 0.5f;
    //object
    public GameObject IMUSensor;
    //variables to calculate acceleration 
    private Vector3 acceleration;
    private Vector3 lastVelocity = new Vector3(0, 0, 0);
    double[] covariance = new double[3];
    



    void Start()
    {
        // Get ROS connection 
        pub = new GenericPub<ImuMsg>(topicName, publishRate);
        InvokeRepeating("PublishIMU", 1, publishRate);
    }

    private void PublishIMU()
    {
        ImuMsg msg = new ImuMsg();
        msg.header.frame_id = "IMU_link";
        /*quaternion message*/
        msg.orientation.x = (double)IMUSensor.GetComponent<Transform>().rotation.x;
        msg.orientation.y = (double)IMUSensor.GetComponent<Transform>().rotation.y;
        msg.orientation.z = (double)IMUSensor.GetComponent<Transform>().rotation.z;
        msg.orientation.w = (double)IMUSensor.GetComponent<Transform>().rotation.w;
        /* angular velocity*/
        msg.angular_velocity.x = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.x;
        msg.angular_velocity.y = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.y;
        msg.angular_velocity.z = (double)IMUSensor.GetComponent<ArticulationBody>().angularVelocity.z;
        /*calculate linear acceleration */
        acceleration = (IMUSensor.GetComponent<ArticulationBody>().velocity - lastVelocity) / Time.fixedDeltaTime;
        lastVelocity = IMUSensor.GetComponent<ArticulationBody>().velocity;
        /*linear acceleration */
        (msg.linear_acceleration.x, msg.linear_acceleration.y, msg.linear_acceleration.z) = (acceleration.x, acceleration.y, acceleration.z);
        covariance[0] = -1;
        msg.orientation_covariance = covariance;
        msg.angular_velocity_covariance = covariance;
        msg.linear_acceleration_covariance=covariance ;


        pub.Publish(msg);
    }
}