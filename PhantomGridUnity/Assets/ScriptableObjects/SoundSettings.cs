using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhantomGrid.ScriptableObjects
{
    public enum SoundType
    {
        CardFlip,
        MatchFail,
        MatchSuccess,
        Streak,
        GameOver,
    }

    [Serializable]
    public struct SoundMap
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
    }
    
    [Serializable]
    public class SoundSettings : ISoundSettings
    {
        [SerializeField]
        private List<SoundMap> _soundMaps;

        public AudioClip GetAudioClip(SoundType soundType)
        {
            return _soundMaps.Find(x => x.SoundType == soundType).AudioClip;
        }
    }

    public interface ISoundSettings
    {
        public AudioClip GetAudioClip(SoundType soundType);
    }
}