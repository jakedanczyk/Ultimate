using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLook : MonoBehaviour
{
    public Animator anim;
    public Transform cam;
    public Transform bone;
    public Transform look;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //anim.SetBoneLocalRotation(HumanBodyBones.Spine, q);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float lookAngle = cam.rotation.eulerAngles.x;//degrees above/below horrizon camera is looking
        //var dir = look.position - cam.position;
        var q = Quaternion.AngleAxis(lookAngle, Vector3.forward);
        //q *= Quaternion.Euler(Vector3.forward * 90);
        anim.SetBoneLocalRotation(HumanBodyBones.Spine, q);
    }
}
