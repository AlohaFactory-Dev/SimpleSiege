using System;
using UnityEngine;
using Spine.Unity;

public class KingSkinController : MonoBehaviour
{
    // SkeletonMecanim 컴포넌트 참조
    private SkeletonMecanim skeletonMecanim;

    void Awake()
    {
        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
    }

    public void ChangeSkin(string skinName)
    {
        if (skinName == String.Empty) return;
        skeletonMecanim.Skeleton.SetSkin(skinName);
        skeletonMecanim.Skeleton.SetSlotsToSetupPose();
    }
}