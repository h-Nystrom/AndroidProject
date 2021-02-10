using UnityEngine;
using UnityEngine.UI;

namespace FootBallKick{
    public class ScoreUiHandler : MonoBehaviour{
        Text text;
        int currentScore;
        void Start(){
            text = GetComponent<Text>();
            text.text = currentScore.ToString("Score: 0");
            MessageHandler.instance.SubscribeTo<HitTargetMessage>(UpdateText);
        }
        void OnDestroy(){
            MessageHandler.instance.UnsubscribeFrom<HitTargetMessage>(UpdateText);
        }
        void UpdateText(HitTargetMessage hitTargetMessage){
            currentScore += 1;
            text.text = currentScore.ToString("Score: 0");
        }
    }
}

