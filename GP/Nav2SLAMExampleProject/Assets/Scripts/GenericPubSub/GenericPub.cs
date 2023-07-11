using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Robotics.ROSTCPConnector;

using ROS;
namespace ROS {

    public class GenericPub <MessageType> where MessageType : Unity.Robotics.ROSTCPConnector.MessageGeneration.Message
    {
        [SerializeField] public string topicName="--" ;
        [SerializeField] public float publishMessageFrequency = 0.5f;
  

        public GenericPub(string topicName ,  float publishMessageFrequency )
        {
            this.topicName = topicName;
            this.publishMessageFrequency = publishMessageFrequency;
            if (ROSConnection.GetOrCreateInstance().HasConnectionError)
            {
                Debug.Log("Error in creating ROS connection");
            }
            try
            {
                ROSConnection.GetOrCreateInstance().RegisterPublisher<MessageType>(topicName);
            }
            catch
            {
                Debug.Log("Couldn't Add/Register Message type ");
            }
        }
        
        public void Publish(MessageType msg)
        {
            ROSConnection.GetOrCreateInstance().Publish(topicName, msg );
        }

    }
    public class mTime : MonoBehaviour
    {
        public static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public  static void Now(out uint sec, out uint nanosec)
        {
            TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - UNIX_EPOCH;
            double msecs = timeSpan.TotalMilliseconds;
            sec = (uint)(msecs / 1000);
            nanosec = (uint)((msecs / 1000 - sec) * 1e+9);
        }
    }


}
