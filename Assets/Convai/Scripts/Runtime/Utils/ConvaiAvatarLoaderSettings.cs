using UnityEngine;

namespace Convai
{
    [CreateAssetMenu(fileName = "ConvaiAvatarLoaderSettings", menuName = "Convai/Avatar Loader Settings")]
    public class ConvaiAvatarLoaderSettings : ScriptableObject
    {
        [Header("Avatar Caching Settings")]
        public bool AvatarCachingEnabled = true;
        
        [Header("Avatar Configuration")]
        public ConvaiRPMAvatarConfig AvatarConfig;
        
        [Header("GLTF Settings")]
        public GameObject GLTFDeferAgent;
        
        private static ConvaiAvatarLoaderSettings _instance;
        
        public static ConvaiAvatarLoaderSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<ConvaiAvatarLoaderSettings>("ConvaiAvatarLoaderSettings");
                    if (_instance == null)
                    {
                        Debug.LogWarning("ConvaiAvatarLoaderSettings not found in Resources folder. Creating default instance.");
                        _instance = CreateInstance<ConvaiAvatarLoaderSettings>();
                    }
                }
                return _instance;
            }
        }
        
        private void OnEnable()
        {
            if (_instance == null)
                _instance = this;
        }
    }
}