using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics;
using Unity.Robotics.UrdfImporter.Control;
public class RosJointControl : MonoBehaviour
{
    [SerializeField]
    
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
    public ArticulationBody Ljoint;
    [SerializeField]
    public ArticulationBody Rjoint;

    public bool update = false;
    [SerializeField]
    public int L_Percentatge;
    [SerializeField]
    public int R_Percentatge;
    void Start()
    {
        controller.UpdateControlType(Ljoint);
        controller.UpdateControlType(Rjoint);
    }


    void FixedUpdate()
    {
        ArticulationDrive currentLDrive = Ljoint.xDrive;
        ArticulationDrive currentRDrive = Rjoint.xDrive;
        currentLDrive.target += newLTargetDelta * Time.deltaTime * ((update) ? 1f : 0f);
        currentRDrive.target += newRTargetDelta * Time.deltaTime * ((update) ? 1f : 0f);
        Debug.Log(currentRDrive.target);
        Debug.Log(currentLDrive.target+update.ToString());
        Ljoint.xDrive = currentLDrive;
        Rjoint.xDrive = currentRDrive;
        update = false;
    }

    void PWM_Update(int L_Percentatge, int Ldirection, int R_Percentatge, int Rdirection)
    {

        newLTargetDelta = (L_Percentatge * 50f) * Ldirection;
        newRTargetDelta = (R_Percentatge * 50f) * Rdirection;
        update = true;
    }
    public void test()
    {
        PWM_Update(L_Percentatge, (int)RotationDirection.Positive, R_Percentatge, (int)RotationDirection.Positive);
    }
}
