using PhantomGrid.Events;
using PhantomGrid.Factory;
using PhantomGrid.Managers;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Installer
{
    public class GameContext : MonoInstaller
    {
        [SerializeField]
        private GameManager _gameManager;
        [SerializeField]
        private UIManager _uiManager;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameManager>().FromInstance(_gameManager).AsSingle();
            Container.BindInterfacesTo<UIManager>().FromInstance(_uiManager).AsSingle();
            Container.BindInterfacesTo<EventBus>().AsSingle();
            Container.BindInterfacesTo<CardSpriteRepository>().AsSingle();
            Container.BindInterfacesTo<CardFactory>().AsSingle();
        }
    }
}