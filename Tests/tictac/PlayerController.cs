using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Tests.Networking
{
    public class PlayerController : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        public override void OnStartLocalPlayer()
        {
            Destroy(Camera.current.gameObject);
            gameObject.GetComponentInChildren<Camera>().enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (hasAuthority)
            {
                // Get the horizontal and vertical axis.
                // By default they are mapped to the arrow keys.
                // The value is in the range -1 to 1
                float translation = Input.GetAxis("Vertical") * 10.0f;
                float rotation = Input.GetAxis("Horizontal") * 100.0f;

                // Make it move 10 meters per second instead of 10 meters per frame...
                translation *= Time.deltaTime;
                rotation *= Time.deltaTime;

                // Move translation along the object's z-axis
                transform.Translate(0, 0, translation);

                // Rotate around our y-axis
                transform.Rotate(0, rotation, 0);
            }
        }
    }
}
