using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;

using ROS;
using UnityEngine;
//using velSub = RosMessageTypes.UnityRoboticsDemo.VelMsgMsg;


public class velSub : MonoBehaviour
{
    GenericSub<VelMsgMsg> Sub = null;
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        Sub = new GenericSub<VelMsgMsg>("vel", test);
    }

    void test(VelMsgMsg message)
    {
       //Debug.Log("I recirved smthing.....");
       cube.transform.Translate(message.LinearToVector3());
        cube.transform.Rotate(message.AngularToVector3());
      // cube.transform.rotation = cube.transform.rotation + message.angular;
        Debug.Log(message.ToString());
    }
}
