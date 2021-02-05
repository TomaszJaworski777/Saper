using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //usable camera
    [SerializeField] Camera playerCamera;

    //bomb manager for click tile logic
    BombsManager manager;

    private void Start()
    {
        //sets manager form the same object
        manager = GetComponent<BombsManager>();
    }

    private void Update()
    {
        //if LMB is clicked
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //creates ray from camera
            RaycastHit raycastHit;
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            //cast a raycast from this ray
            if (Physics.Raycast(ray, out raycastHit))
            {
                //get tile component form hited object
                var tile = raycastHit.transform.GetComponent<Tile>();

                //invoke clicked left method from bomb manager
                manager.ClickedLeft(tile);

                //debug values, check this on server
                Debug.Log(manager.BombsValue());
                Debug.Log(manager.CheckWin());
            }
        }

        //if RMB is clicked
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //creates ray from camera
            RaycastHit raycastHit;
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            //cast a raycast from this ray
            if (Physics.Raycast(ray, out raycastHit))
            {
                //get tile component form hited object
                var tile = raycastHit.transform.GetComponent<Tile>();

                //invoke clicked right method from bomb manager
                manager.ClickedRight(tile);

                //debug values, check this on server
                Debug.Log(manager.BombsValue());
                Debug.Log(manager.CheckWin());
            }
        }
    }
}
