using PhantomGrid.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Installer
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "PhantomGame/SoundSettings", order = 0)]
    public class SoundSettingsInstaller : ScriptableObjectInstaller<SoundSettingsInstaller>
    {
        [SerializeField]
        private SoundSettings _sourceSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundSettings>().FromInstance(_sourceSettings);
        }
        
    }
}