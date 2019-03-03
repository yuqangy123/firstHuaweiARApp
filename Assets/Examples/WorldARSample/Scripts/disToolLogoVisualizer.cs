namespace Common
{
    using UnityEngine;
    using HuaweiARUnitySDK;
    using HuaweiARInternal;
    public class disToolLogoVisualizer:MonoBehaviour
    {
        private ARAnchor m_anchor;
        
        public void Awake()
        {
           
        }
        public void setAnchor(ARAnchor anchor)
        {
            m_anchor = anchor;
            Update();
        }
        public void Update()
        {
            if (null == m_anchor)
            {
                gameObject.SetActive(false);
                return;
            }
            switch (m_anchor.GetTrackingState())
            {
                case ARTrackable.TrackingState.TRACKING:
                    Pose p = m_anchor.GetPose();
                    gameObject.transform.position = p.position;
                    gameObject.transform.rotation = p.rotation;
                    gameObject.transform.Rotate(0f, 225f, 0f, Space.Self);
                    gameObject.SetActive(true);
                    break;
                case ARTrackable.TrackingState.PAUSED:
                    gameObject.SetActive(false);
                    break;
                case ARTrackable.TrackingState.STOPPED:
                default:
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
