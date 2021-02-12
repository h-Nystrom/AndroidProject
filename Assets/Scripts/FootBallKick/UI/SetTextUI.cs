using System;
using UnityEngine;
using UnityEngine.UI;

namespace FootBallKick.UI{
    public class SetTextUI : MonoBehaviour{
        Text text;
        string info;
        void Start(){
            text = GetComponent<Text>();
            info = text.text;
        }
        public void UpdateText(float value){
            text.text = value.ToString($"{info}: 0.0");
        }
    }
}
