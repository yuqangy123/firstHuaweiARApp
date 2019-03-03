namespace Preview
{
    using UnityEngine;
    using System.Collections.Generic;
    using HuaweiARUnitySDK;
    using System.Collections;
    using System;
    using HuaweiARInternal;
    using Common;

    
    public class distanceToolScpript : MonoBehaviour
    {
        public GameObject m_grid;
        public GameObject m_touchModelPrefabs;
        private GameObject m_touchBeginModel, m_touchEndModel;
        private int m_touchIndex;

        void Start()
        {
            m_touchIndex = 0;

            m_touchBeginModel = Instantiate(m_touchModelPrefabs);
            m_touchBeginModel.SetActive(false);
            m_touchEndModel = Instantiate(m_touchModelPrefabs);
            m_touchEndModel.SetActive(false);
        }

        public void Update()
        {
            Touch touch;
            if (ARFrame.GetTrackingState() != ARTrackable.TrackingState.TRACKING
                || Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                
            }
            else
            {
                trackTest(touch);
            }
        }

        private void trackTest(Touch touch)
        {
            List<ARHitResult> hitResults = ARFrame.HitTest(touch);
            foreach (ARHitResult singleHit in hitResults)
            {
                ARTrackable trackable = singleHit.GetTrackable();
                if((trackable is ARPlane && ((ARPlane)trackable).IsPoseInPolygon(singleHit.HitPose)) ||
                    (trackable is ARPoint))
                {
                    ARAnchor anchor = singleHit.CreateAnchor();
                    ARDebug.LogInfo("GridARScript:trackTest anchor world position {0}", anchor.GetPose().position);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(anchor.GetPose().position); 
                    ARDebug.LogInfo("GridARScript:trackTest anchor screen position {0}", screenPos);


                    if(m_touchIndex%2 == 0)
                    {
                        m_touchBeginModel.GetComponent<disToolLogoVisualizer>().setAnchor(anchor);

                        var script = m_grid.GetComponent<GridARScpript>();
                        if(script)
                        {
                            script.setBeginAnchor(anchor);
                        }
                            
                    }
                    else
                    {
                        m_touchEndModel.GetComponent<disToolLogoVisualizer>().setAnchor(anchor);
                    }
                    ++m_touchIndex;
                    break;
                }
            }
        }
        
        void OnGUI()
        {
            GUIStyle bb = new GUIStyle();
            bb.normal.background = null;
            bb.normal.textColor = new Color(1, 0, 0);
            bb.fontSize = 50;
            
            GUI.Label(new Rect(10, 50, 400, 200), string.Format("disntance:{0},{1:F3}", m_touchIndex%2, Vector3.Distance(m_touchBeginModel.transform.position, m_touchEndModel.transform.position)), bb);
        }
    }
}
