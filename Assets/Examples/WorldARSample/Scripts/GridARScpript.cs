namespace Preview
{
    using UnityEngine;
    using System.Collections.Generic;
    using HuaweiARUnitySDK;
    using System.Collections;
    using System;
    using HuaweiARInternal;
    using Common;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GridARScpript : MonoBehaviour
    {
        private bool m_bshowing;
        private MeshRenderer m_MeshRenderer;
        private ARAnchor m_beginAnchor;
        Vector2 m_touchScreenPos;
        void Start()
        {
            var mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.RecalculateNormals();// 网格自动计算法线向量
            m_bshowing = false;

            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_beginAnchor = null;
        }

        public void setBeginAnchor(ARAnchor ch)
        {
            m_beginAnchor = ch;
            m_touchScreenPos = Camera.main.WorldToScreenPoint(ch.GetPose().position);
            ARDebug.LogInfo("GridARScript:m_touchScreenPos {0}", m_touchScreenPos);
        }

        public void Update()
        {
            trackTest();
        }

        private void renderMesh (Vector3 beginPos, Vector3 endPos)
        {
            var mat = gameObject.GetComponent<MeshRenderer>().material;
            
            float distance = Vector3.Distance(beginPos, endPos);
            
            float tilingW = distance/(float)mat.mainTexture.width;
            tilingW *= 100.0f;
            
            mat.SetTextureScale("_MainTex", new Vector2(tilingW, 1));
            

            Vector3 cro = Vector3.Cross(endPos-beginPos, new Vector3(0.0f, 1.0f, 0.0f));
            cro.Normalize();
            ARDebug.LogInfo("GridARScript: tilingW:{0},{1},{2},{3}", tilingW, beginPos, endPos, cro);
            cro*=mat.mainTexture.height;

            Vector3[] vertices = new Vector3[4];
            vertices [0] = beginPos;
            vertices [1] = endPos;
            vertices [2] = beginPos+cro;
            vertices [3] = endPos+cro;

            Vector2[] uv = new Vector2[vertices.Length];
            uv [0] = new Vector2 (0, 0);
            uv [1] = new Vector2 (1, 0);
            uv [2] = new Vector2 (0, 1);
            uv [3] = new Vector2 (1, 1);
        
            var mesh = GetComponent<MeshFilter>().mesh;
            if(null != mesh)
            {
                mesh.Clear();
            }
            
            //mesh.name = "Procedural Grid";

            mesh.vertices = vertices;
            mesh.uv = uv;

            int[] triangles = new int[6];
            triangles [0] = 0;
            triangles [1] = 2;
            triangles [2] = 1;

            triangles [3] = 2;
            triangles [4] = 3;
            triangles [5] = 1;

            mesh.triangles = triangles;

            

            
        }
        
        private void trackTest()
        {
            if(null == m_beginAnchor)
            {
                m_MeshRenderer.enabled = false;
                return;
            }
                
            
            switch (m_beginAnchor.GetTrackingState())
            {
                case ARTrackable.TrackingState.TRACKING:
                    m_MeshRenderer.enabled = true;
                    break;

                case ARTrackable.TrackingState.PAUSED:
                    m_MeshRenderer.enabled = false;
                    return;
                    
                case ARTrackable.TrackingState.STOPPED:
                default:
                    m_MeshRenderer.enabled = false;
                    m_beginAnchor = null;
                    return;
            }

            List<ARHitResult> hitResults = ARFrame.HitTest(m_touchScreenPos.x, m_touchScreenPos.y);
            ARDebug.LogInfo("GridARScript:trackTest hitResults count {0}", hitResults.Count);
            foreach (ARHitResult singleHit in hitResults)
            {
                ARTrackable trackable = singleHit.GetTrackable();
                ARDebug.LogInfo("GridARScript:trackTest GetTrackable {0}", singleHit.GetTrackable());
                if((trackable is ARPlane && ((ARPlane)trackable).IsPoseInPolygon(singleHit.HitPose)) ||
                    (trackable is ARPoint))
                {
                    ARAnchor anchor = singleHit.CreateAnchor();
                    //ARDebug.LogInfo("GridARScript:trackTest anchor world position {0}", anchor.GetPose().position);
                    
                    renderMesh(m_beginAnchor.GetPose().position, anchor.GetPose().position);
                    break;
                }
            }
        }
    }
}
