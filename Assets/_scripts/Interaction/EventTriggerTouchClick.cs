using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
    public class EventTriggerTouchClick : MonoBehaviour
    {

        private OpenCVManager openCVManager;
        private GameVisualManager gameManagerNotOpenCV;

        // private InferenceNNfomDATABASE inferenceNNfomDATABASE;
        // private InferenceCompositionML inferenceCompositionML;
        // private InferenceScoreFeatures inferenceScoreFeatures;
        // private InferenceFinalOut inferenceFinalOut;

        private BarracudaCNNModel barracudaCNNModel;
        private BarracudaNNfromDatabase barracudaNNfromDatabase;
        private BarracudaOpenCvFeature barracudaOpenCvFeature;
        private BarracudaFinalOut barracudaFinalOut;


        protected virtual void OnEnable()
        {
            // Hook into the events we need
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerSet += OnFingerSet;
            LeanTouch.OnFingerUp += OnFingerUp;
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnFingerSwipe += OnFingerSwipe;
            LeanTouch.OnGesture += OnGesture;
        }

        private void Start()
        {
            openCVManager = FindObjectOfType<OpenCVManager>();
            gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
            // inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();
            // inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
            // inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
            // inferenceFinalOut = FindObjectOfType<InferenceFinalOut>();

            barracudaCNNModel =  FindObjectOfType<BarracudaCNNModel>();
            barracudaNNfromDatabase = FindObjectOfType<BarracudaNNfromDatabase>();
            barracudaOpenCvFeature = FindObjectOfType<BarracudaOpenCvFeature>();
            barracudaFinalOut = FindObjectOfType<BarracudaFinalOut>();
        }

        protected virtual void OnDisable()
        {
            // Unhook the events
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerSet -= OnFingerSet;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerTap -= OnFingerTap;
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
            LeanTouch.OnGesture -= OnGesture;
        }

        public void OnFingerDown(LeanFinger finger)
        {
            //openCVManager.CallTOCalculateAreaLeftRight();
            //Debug.Log("Finger " + finger.Index + " began touching the screen");
        }

        public void OnFingerSet(LeanFinger finger)
        {
            //openCVManager.CallForOpenCVCalculationUpdates();
            //gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();

            //// inferenceScoreFeatures.CallTOCalculateFeaturesAllScores();
            //// inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
            //// inferenceCompositionML.CallTOCalculateMobileNetScore();
            //// inferenceFinalOut.CallTOCalculateFinalOutScore();

            //barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
            //barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
            //barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
            //barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();
            ////Debug.Log("Finger " + finger.Index + " is still touching the screen");
        }

        public void OnFingerUp(LeanFinger finger) ///
        {

            //Debug.Log("Finger " + finger.Index + " finished touching the screen");

            openCVManager.CallForOpenCVCalculationUpdates();
            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();



            barracudaCNNModel.CallTOCalculateBarracudaCNNScore();
            barracudaNNfromDatabase.CallTOCalculateBarracudaNNFrontTopcore();
            barracudaOpenCvFeature.BarracudaCallTOCalculateOpencvFeaturesScore();
            barracudaFinalOut.BarracudaCallTOCalculateFinalOutScore();


        }

        public void OnFingerTap(LeanFinger finger)
        {
            //Debug.Log("Finger " + finger.Index + " tapped the screen");
        }

        public void OnFingerSwipe(LeanFinger finger)
        {
            //Debug.Log("Finger " + finger.Index + " swiped the screen");
        }

        public void OnGesture(List<LeanFinger> fingers)
        {
            //Debug.Log("Gesture with " + fingers.Count + " finger(s)");
            //Debug.Log("    pinch scale: " + LeanGesture.GetPinchScale(fingers));
            //Debug.Log("    twist degrees: " + LeanGesture.GetTwistDegrees(fingers));
            //Debug.Log("    twist radians: " + LeanGesture.GetTwistRadians(fingers));
            //Debug.Log("    screen delta: " + LeanGesture.GetScreenDelta(fingers));
        }
    }
}