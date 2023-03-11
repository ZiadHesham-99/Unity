/*using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
public class TempSensorPub : MonoBehaviour
{
    ROSConnection ros;
    //creating topic
    public string topicName = "temp_sensor";
    //refrance to the temp sensor object
    public GameObject sensor;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        //starting a ros connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TempMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            float reading = 37.0f;

         //   TempMsg temp = new TempMsg( reading );

            // Finally send the message to server_endpoint.py running in ROS
         //   ros.Publish(topicName, temp );

            timeElapsed = 0;
        }
    }
}
*/