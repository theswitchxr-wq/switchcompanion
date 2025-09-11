using UnityEngine;
using System;
using System.Collections.Generic;

namespace Convai
{
    [CreateAssetMenu(fileName = "ConvaiRPMAvatarConfig", menuName = "Convai/RPM Avatar Config")]
    public class ConvaiRPMAvatarConfig : ScriptableObject
    {
        [Header("Avatar Quality Settings")]
        public int Lod = 0;
        public int Pose = 0;
        public int TextureAtlas = 0;
        public int TextureSizeLimit = 1024;
        
        [Header("Texture Channels")]
        public int[] TextureChannel = new int[] { 0, 1, 2, 3, 4 };
        
        [Header("Avatar Features")]
        public bool UseHands = true;
        public bool UseDracoCompression = false;
        public bool UseMeshOptCompression = false;
        
        [Header("Shader Configuration")]
        public Shader Shader;
        public List<ShaderProperty> ShaderProperties = new List<ShaderProperty>();
        
        [Header("Morph Targets")]
        public List<string> MorphTargets = new List<string>
        {
            "Oculus Visemes",
            "eyeBlinkLeft", 
            "eyeBlinkRight",
            "ARKit"
        };
        
        private static ConvaiRPMAvatarConfig _instance;
        
        public static ConvaiRPMAvatarConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ConvaiRPMAvatarConfig>("ConvaiRPMAvatarConfig");
                    if (_instance == null)
                    {
                        Debug.LogWarning("ConvaiRPMAvatarConfig not found in Resources folder. Creating default instance.");
                        _instance = CreateInstance<ConvaiRPMAvatarConfig>();
                        _instance.InitializeDefaults();
                    }
                }
                return _instance;
            }
        }
        
        private void OnEnable()
        {
            if (_instance == null)
                _instance = this;
                
            if (ShaderProperties == null || ShaderProperties.Count == 0)
                InitializeDefaults();
        }
        
        private void InitializeDefaults()
        {
            if (ShaderProperties == null)
                ShaderProperties = new List<ShaderProperty>();
                
            ShaderProperties.Clear();
            ShaderProperties.Add(new ShaderProperty { TextureChannel = 0, PropertyName = "_MainTex" });
            ShaderProperties.Add(new ShaderProperty { TextureChannel = 1, PropertyName = "_BumpMap" });
            ShaderProperties.Add(new ShaderProperty { TextureChannel = 2, PropertyName = "" });
            ShaderProperties.Add(new ShaderProperty { TextureChannel = 3, PropertyName = "" });
            ShaderProperties.Add(new ShaderProperty { TextureChannel = 4, PropertyName = "" });
        }
    }
    
    [Serializable]
    public class ShaderProperty
    {
        public int TextureChannel;
        public string PropertyName;
    }
}