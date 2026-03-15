using PhantomGrid.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Installer
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "PhantomGame/GameSettings", order = 0)]
    public class GameSettingsInstaller :  ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameSettings gameSettings;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(gameSettings);
        }
    }
}