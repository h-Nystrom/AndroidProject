using UnityEngine;

namespace FootBallKick.UI{
    public class EnableDisable : MonoBehaviour{

        bool active;
        public void SwitchEnable(){
            active = !active;
            gameObject.SetActive(active);    
        }
    }
}