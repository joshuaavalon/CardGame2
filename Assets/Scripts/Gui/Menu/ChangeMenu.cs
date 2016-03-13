using System.Collections;
using UnityEngine;

// ChangeMenu Class triggers parent's Animator to "Disappear" and triggers another menu's Animator to "Appear"

namespace Assets.Scripts.Gui.Menu
{
    public class ChangeMenu : MonoBehaviour
    {
        // New menu to be appeared
        public MoveCamera CameraControl;
        public Transform MenuCamera;
        public GameObject Menu;
        public GameObject Parent;

        // Trigger change menu
        public void Change()
        {
            CameraControl.Destination = MenuCamera;
            Parent.SetActive(false);
            Menu.SetActive(true);
        }
        
    }
}