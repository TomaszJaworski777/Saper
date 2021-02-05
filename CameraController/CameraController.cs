using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobaX.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target")]
        //owner of the camera
        public Transform cameraFocus;

        //local transform handler
        Transform transf;

        [Header("Settings")]
        //camera offset
        public Vector3 cameraOffest;

        //camera angle
        public Vector3 cameraAngle;

        //zoom multiplayer
        public float zoom;

        //sensitivity multiplayer
        public float sensitivity;

        void Start()
        {
            //setting transform in local variable
            transf = transform;

            //initialize camera position
            SetCameraZoom();

            if(cameraFocus != null)
                SetOnTarget();
        }

        /// <summary>
        /// Use this on zoom value change and on start to initialize camera position.
        /// </summary>
        void SetCameraZoom()
        {
            //setting camera offest
            transf.position = cameraOffest;

            //setting camera angle
            transf.eulerAngles = cameraAngle;
        }

        /// <summary>
        /// Sets camera center on a target.
        /// </summary>
        void SetOnTarget()
        {
            //add offset to target position to set camera on it
            transf.position = cameraFocus.position + cameraOffest;
        }

        /// <summary>
        /// Move camera by a vector.
        /// </summary>
        /// <param name="direction">Vector of the camera movement.</param>
        void MoveCamera(Vector2 direction)
        {
            //move camera by direction vector multiplayed by sensitivity
            transf.position += (Vector3)direction * sensitivity;
        }
    }
}
