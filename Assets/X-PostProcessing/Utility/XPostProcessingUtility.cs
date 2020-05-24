using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace XPostProcessing
{
    public class XPostProcessingUtility
    {

        #region Instance

        private static XPostProcessingUtility _instance;
        public static XPostProcessingUtility Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new XPostProcessingUtility();
                }
                return _instance;
            }
        }

        #endregion



        //-----------------------------------------------------------------------------------------------------
        static int resetFrameCount = 0;
        static Color srcColor;
        static Color dstColor;
        public static Color GetRandomLerpColor(int RandomFrameCount, float lerpSpeed)
        {
            // Color version
            if (resetFrameCount == 0)
            {
                srcColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            }
            float lerp = lerpSpeed;

            dstColor = Color.Lerp(dstColor, srcColor, lerp);
            resetFrameCount++;
            if (resetFrameCount > RandomFrameCount)
            {
                resetFrameCount = 0;
            }

            return dstColor;
        }

        public static Color RandomColor()
        {
            return new Color(Random.value, Random.value, Random.value, Random.value);
        }




        public int LastSelectedCategory;
        public int ThumbWidth;
        public int ThumbHeight;
        public int cache_ThumbWidth;
        public int cache_ThumbHeight;
        public bool cache_IsLinear;
        public RenderTexture PreviewRT;


        public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
        {
            var oldRT = RenderTexture.active;

            var tex = new Texture2D(rt.width, rt.height);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
            RenderTexture.active = oldRT;
        }


        static string TypePreFix = "XPostProcessing.";


        public static System.Type GetSettingByName(string typeFullName)
        {
            var type = System.Type.GetType(TypePreFix + typeFullName);
            //Debug.Log("Utllity Type : " + type);
            return type;
        }



    }

}