using PhantomGrid.Events;
using PhantomGrid.Factory;
using PhantomGrid.Managers;
using UnityEngine;
using Zenject;

namespace PhantomGrid.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameManager _gameManager;
        [SerializeField]
        private UIManager _uiManager;
        [SerializeField]
        private SoundManager _soundManager;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameManager>().FromInstance(_gameManager).AsSingle();
            Container.BindInterfacesTo<UIManager>().FromInstance(_uiManager).AsSingle();
            Container.BindInterfacesTo<SoundManager>().FromInstance(_soundManager).AsSingle();

            Container.BindInterfacesTo<EventBus>().AsSingle();
            Container.BindInterfacesTo<CardSpriteRepository>().AsSingle();
            Container.BindInterfacesTo<CardFactory>().AsSingle();
            Container.BindInterfacesTo<ScoreHandler>().AsSingle();
            Container.BindInterfacesTo<PersistantDataModel>().AsSingle();
            Container.BindInterfacesTo<GameSessionManager>().AsSingle();
        }
    }
}