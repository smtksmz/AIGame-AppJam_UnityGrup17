using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Models 
{
    #region - Player - 
    [Serializable]
    public class PlayerSettingsModel
    {

        [Header("View Settings")]
        public float ViewXSensivity;
        public float ViewYSensivity;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement - Running")]
        public float RunningForwardSpeed;
        public float RunningStrafeSpeed;

        [Header("Movement - Walking")]
        public float WalkingForwardSpeed;
        public float WalkingBackwardSpeed;
        public float WalkingStrafeSpeed;


        [Header("Jumping")]
        public float JumpingHeight;
        public float JumpingFalloff;

    }
    #endregion
}