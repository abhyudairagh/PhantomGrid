
using PhantomGrid.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace PhantomGrid
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
       [SerializeField]
       AudioSource _sfxSource;

       private ISoundSettings _soundSettings;

       [Inject]
       public void Construct(ISoundSettings soundSettings)
       {
           _soundSettings = soundSettings;
       }
       public void PlaySound(SoundType soundType)
       {
           _sfxSource.PlayOneShot(_soundSettings.GetAudioClip(soundType));
       }
    }

    public interface ISoundManager
    {
        void PlaySound(SoundType soundType);
    }
}
