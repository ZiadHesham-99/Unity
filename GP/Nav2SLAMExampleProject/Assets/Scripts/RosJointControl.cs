using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;
using ROS;
using RosMessageTypes;
public class RosJointControl : MonoBehaviour
{
    [SerializeField]
    GenericSub<RosMessageTypes.Std.Int32MultiArrayMsg> Sub = null;
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    [SerializeField]
    Controller controller;
    public RotationDirection Ldirection;
    public RotationDirection Rdirection;
    [SerializeField]
    float newLTargetDelta;
    [SerializeField]
    float newRTargetDelta;
    [SerializeField]
    string SubTopic = "Movement";
    [SerializeField]
    string PubTopic = "";
    private int prevLeft, Prev_right;
    public ArticulationBody Ljoint;
    public ArticulationBody Rjoint;

    public bool update = false;
    [SerializeField]
    public int L_Percentatge;
    [SerializeField]
    public float ratio = 30f;
    [SerializeField]
    public int R_Percentatge;
    void Start()
    {
        controller.UpdateControlType(Ljoint);
        controller.UpdateControlType(Rjoint);
        Sub = new GenericSub<RosMessageTypes.Std.Int32MultiArrayMsg>(SubTopic, UpdateMovVals);
    }

    
    void FixedUpdate()
    {
        ArticulationDrive currentLDrive = Ljoint.xDrive;
        ArticulationDrive currentRDrive = Rjoint.xDrive;
        currentLDrive.target += newLTargetDelta * Time.deltaTime * ((update) ? 1 : 0);
        currentRDrive.target += newRTargetDelta * Time.deltaTime * ((update) ? 1 : 0);
       // Debug.Log(currentRDrive.target);
        //Debug.Log(currentLDrive.target+update.ToString());
        Ljoint.xDrive = currentLDrive;
        Rjoint.xDrive = currentRDrive;
        update = false;
    }

    void PWM_Update()
    {

        newLTargetDelta = (L_Percentatge * ratio) * ((int)Ldirection);
        newRTargetDelta = (R_Percentatge * ratio) * (int)Rdirection;
        update = true;
    }
    public void UpdateMovVals(RosMessageTypes.Std.Int32MultiArrayMsg data)
    {
      /*  if(prevLeft == (int)data.data[0] && Prev_right == (int)data.data[2])
        {
          //  return;
        }*/
        L_Percentatge = (int)data.data[0];
        // prevLeft = L_Percentatge;
        if (data.data[1] == 1) { Ldirection = RotationDirection.Positive; }
        else { Ldirection = RotationDirection.Negative; }
        R_Percentatge = (int)data.data[2];
        //Prev_right = R_Percentatge;
        if (data.data[3] == 1) { Rdirection = RotationDirection.Positive; }
        else { Rdirection = RotationDirection.Negative; }
        PWM_Update();
    }
}
