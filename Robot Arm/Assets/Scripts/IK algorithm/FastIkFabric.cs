using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FastIkFabric : MonoBehaviour
{
    [SerializeField] public int chainLength = 2;
    [SerializeField] public Transform target ;
    private Transform[] Bones;
    private Vector3[] Positions;
    [SerializeField] public int iterations=10 ;
    [SerializeField] public float delta = 0.01f;
    public float[] BoneLength;
    private float FullLength;

    // Start is called before the first frame update
    private void Awake()
    {
        Init();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Ik_Calculations();
    }
    void Update()
    {
    }
    void OnDrawGizmos() {
        var current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++) {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color= Color.blue;
            Handles.DrawWireCube(Vector3.up*0.5f,Vector3.one);
            current = current.parent;
        }
    }
    void Init()
    {
        Bones = new Transform[chainLength + 1];
        Positions = new Vector3[chainLength + 1];
        BoneLength = new float[chainLength];
        FullLength = 0f;
        var current = this.transform;
        for( int i = chainLength-1; i >= 0; i--)
        {
            Bones[i] = current;
            if (i == chainLength - 1) { 
            }
            else
            {
                BoneLength[i] = (Bones[i+1].position - Bones[i].position).magnitude;
                FullLength += BoneLength[i];
            }

            current = current.parent;

        }

    }
    void Ik_Calculations() {
        if (target != null)
        {
            if (BoneLength.Length != chainLength)
                Init();
            for (int i = 0; i < chainLength; i++)
                Positions[i] = Bones[i].position;
            if ((target.position - Positions[0]).sqrMagnitude >= FullLength * FullLength) {
                var direction = (target.position - Positions[0]).normalized;
                for (int i = 1; i < chainLength; i++) 
                    Positions[i] = Positions[i - 1] + direction * BoneLength[i-1];
            
            }
            for (int i = 0; i < chainLength; i++)
                Bones[i].position = Positions[i];
                
        }
        else
        {
            for (int iteration =0; iteration< iterations; iteration++)
            {
                for(int i = Positions.Length -1; i>0; i--)
                {
                    // back propagation 
                    if ( i== Positions.Length-1)
                    {
                        Positions[i] = target.position;
                    }
                    else
                    {
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BoneLength[i ]; 
                    }

                    
                }
                //forward propagation
                for (int i = 1; i < Positions.Length; i++)
                {
                    Positions[i] = Positions[i-1] + (Positions[i] - Positions[i - 1]).normalized * BoneLength[i-1];
                }
                if((target.position - Positions[Positions.Length-1]).sqrMagnitude <= delta * delta)
                {
                    break;
                }
            }
        }
    }
}
