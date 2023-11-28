
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using UnityEditor;

namespace PT.Garden
{
    public class PercentageChecker : MonoBehaviour
    {

        [Title("New")]
        public static event Action OnFirstStarUnlock;
        public static event Action OnSecondStarUnlock;
        public GameObject unlockedStar1;
        public GameObject unlockedStar2;
        bool UnlockOnce1;
        bool UnlockOnce2;
        [Space]
        //==============================================



        [SerializeField] private float percentage;
        [SerializeField] private float[] results;

        [SerializeField] private ComputeShader _cs;
        //[SerializeField] private Color _refree;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private int _matIndex = 0;
        [SerializeField] private string _mainTexName = "_NoGrassTex";
        //[SerializeField] private float _betweenReads = 0.5f;
        //[SerializeField] private bool _run;
        private int _txid;
        private Material _mainMaterial;
        private Texture _texture;
        //private bool _isChecking = true, _checkSig = false;
        //private int width, height;
        //private Color[] colors;
        [SerializeField] int kernelindex;
        [SerializeField] private Image _filledSlider;
        private ComputeBuffer sumBuffer;

        private void Start()
        {
            _mainMaterial = _meshRenderer.materials[_matIndex];
            _txid = Shader.PropertyToID(_mainTexName);
            kernelindex = _cs.FindKernel("CSMain");
            _texture = _mainMaterial.GetTexture(_txid);
            results = new float[_texture.width * _texture.height];
            sumBuffer = new ComputeBuffer(results.Length, sizeof(float));
        }
        public float trp;
        private void Update()
        {
            //_run = false;



            // get the texture
            _texture = _mainMaterial.GetTexture(_txid);

            RenderTexture t = (RenderTexture)_texture;

            RenderTexture rt = new(t.width, t.height, t.depth, t.format)
            {
                enableRandomWrite = true
            };
            rt.Create();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = t;

            // copy the texture
            Graphics.Blit(t, rt);

            RenderTexture.active = currentRT;

            _cs.SetBuffer(kernelindex, "diffSum", sumBuffer);
            _cs.SetFloat("resolution", rt.width);
            _cs.SetTexture(kernelindex, "Input", rt);
            _cs.Dispatch(kernelindex, _texture.width / 8, _texture.height / 8, 1);
            sumBuffer.GetData(results);

            float sum = 0;
            for (int i = 0; i < results.Length; i++)
            {
                sum += results[i];
            }

            if (percentage >= -trp && percentage < 0.2)
            {
                percentage = (sum / results.Length) - trp;
            }
            else if (percentage > 0.2 && percentage < 0.6)
            {
                percentage = (sum / results.Length) - trp / 2;
            }
            else if (percentage > 0.6 && percentage < 0.8)
            {
                percentage = (sum / results.Length) - trp / 4;
            }
            else if (percentage > 0.8)
            {
                percentage = (sum / results.Length);
            }
            _filledSlider.fillAmount = percentage;
            CheckStarUnlock();

        }

        private void CheckStarUnlock()
        {
            if (percentage > 0.5)
            {
                unlockedStar1.SetActive(true);
                if (!UnlockOnce1)
                {
                    OnFirstStarUnlock?.Invoke();
                    UnlockOnce1 = true;

                }

            }
            if (percentage > 0.95)
            {
                unlockedStar2.SetActive(true);
                if (!UnlockOnce2)
                {
                    OnSecondStarUnlock?.Invoke();
                    UnlockOnce2 = true;

                }
                Debug.LogError("Win");
            }
        }


        

        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Debug.LogError("=================================");
                SaveRenderTextureToPersistentDataPath((RenderTexture)_texture, "hasnat" + PlayerPrefs.GetInt("CurrentPlayingLevel") + ".png");

            }

        }

        //public void SaveTheTexture()
        //{
        //    SaveRenderTextureToPersistentDataPath((RenderTexture)_texture, "hasnat.png");
        //    Debug.LogError("Texture Saved===============");
        //}

        //private void OnApplicationQuit()
        //{
        //    SaveRenderTextureToResourcesFolder((RenderTexture)_texture, "hasnat" + PlayerPrefs.GetInt("CurrentPlayingLevel") + ".png");
        //    Debug.LogError("=================================");
        //}
        //        private void SaveRenderTextureToResourcesFolder(RenderTexture renderTexture, string filename)
        //        {
        //            RenderTexture tempRT = new RenderTexture(renderTexture.width, renderTexture.height, 0);
        //            Graphics.Blit(renderTexture, tempRT);

        //            Texture2D texture2D = new Texture2D(tempRT.width, tempRT.height);
        //            RenderTexture.active = tempRT;
        //            texture2D.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
        //            texture2D.Apply();
        //            byte[] bytes = texture2D.EncodeToPNG();
        //            string filePath = Path.Combine(Application.dataPath, "Resources", filename);
        //            File.WriteAllBytes(filePath, bytes);
        //            DestroyImmediate(texture2D);
        //            DestroyImmediate(tempRT);

        //#if UNITY_EDITOR

        //            UnityEditor.AssetDatabase.Refresh();
        //#endif

        //        }

        private void SaveRenderTextureToPersistentDataPath(RenderTexture renderTexture, string filename)
        {
            RenderTexture tempRT = new RenderTexture(renderTexture.width, renderTexture.height, 0);
            Graphics.Blit(renderTexture, tempRT);

            Texture2D texture2D = new Texture2D(tempRT.width, tempRT.height);
            RenderTexture.active = tempRT;
            texture2D.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            texture2D.Apply();

            byte[] bytes = texture2D.EncodeToPNG();

            string filePath = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllBytes(filePath, bytes);

            DestroyImmediate(texture2D);
            DestroyImmediate(tempRT);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            Debug.LogError("Path++++++" + filePath);
        }
        private void OnDestroy()
        {
            sumBuffer?.Dispose();
        }
    }
}
