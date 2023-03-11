using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using ROS;
using UnityEngine;
using RosColor = RosMessageTypes.UnityRoboticsDemo.UnityColorMsg;

public class GenericPubSubTest : MonoBehaviour
{
    GenericPub<PosRotMsg> pub = null;
    GenericSub<RosColor> Sub = null;
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        pub = new GenericPub<PosRotMsg>("pos_rot",0.5f);
        InvokeRepeating("publishRotation", 1,0.5f);
        Sub = new GenericSub<RosColor>("color", test);

    }
    void test(RosColor message)
    {
        Debug.Log(message.ToString());
    }
    public void  publishRotation()
    {
        cube.transform.rotation = Random.rotation;

        PosRotMsg cubePos = new PosRotMsg(
            cube.transform.position.x,
            cube.transform.position.y,
            cube.transform.position.z,
            cube.transform.rotation.x,
            cube.transform.rotation.y,
            cube.transform.rotation.z,
            cube.transform.rotation.w
        );
        pub.Publish(cubePos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
