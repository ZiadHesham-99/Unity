using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using ROS;
using RosMessageTypes.Geometry;
using System;

namespace ROS
{
    public class GenericSub<MessageType> where MessageType : Unity.Robotics.ROSTCPConnector.MessageGeneration.Message
    {
        [SerializeField] public string topicName = "--";
        public delegate void RcvCallBackFunc(MessageType message);
        public RcvCallBackFunc callback = null;

        public GenericSub(string topicName ,  RcvCallBackFunc callback)
        {
            this.callback = callback;
            this.topicName = topicName;
            ROSConnection.GetOrCreateInstance().Subscribe<MessageType>(this.topicName, (MessageType message)=> { this.callback(message); });
        }

        public static implicit operator GenericSub<MessageType>(GenericSub<PointMsg> v)
        {
            throw new NotImplementedException();
        }
    }
}
