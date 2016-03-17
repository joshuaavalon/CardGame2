using System;
using Assets.Scripts.Core;
using Assets.Scripts.Metadata;
using UnityEngine;

namespace Assets.Scripts
{
    public class AudioControl : MonoBehaviour
    {
        public AudioSource BackgroundMusic;
        public AudioSource SoundEffect;
        public AudioClip ButtonClick;
        public AudioClip AccessMainFrame;
        public AudioClip LoginAuthorized;
        public AudioClip AccessArchives;
        public AudioClip ProgramActivated;
        public AudioClip ProgramTerminated;
        public AudioClip LaunchingActivated;
        public AudioClip LaunchingTerminated;
        public AudioClip AccessFiles;
        public AudioClip Synchronizing;
        public AudioClip PrepareHyperDrive;
        public AudioClip AccessDenied;
        public AudioClip ActiveHyperDrive;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (GameObject.FindGameObjectsWithTag(Tag.Audio).Length > 1)
                DestroyImmediate(gameObject);
        }

        public AudioSource GetAudioSource(SoundType type)
        {
            switch (type)
            {
                case SoundType.ButtonClick:
                    SoundEffect.clip = ButtonClick;
                    break;
                case SoundType.AccessMainFrame:
                    SoundEffect.clip = AccessMainFrame;
                    break;
                case SoundType.LoginAuthorized:
                    SoundEffect.clip = LoginAuthorized;
                    break;
                case SoundType.AccessArchives:
                    SoundEffect.clip = AccessArchives;
                    break;
                case SoundType.ProgramActivated:
                    SoundEffect.clip = ProgramActivated;
                    break;
                case SoundType.ProgramTerminated:
                    SoundEffect.clip = ProgramTerminated;
                    break;
                case SoundType.LaunchingActivated:
                    SoundEffect.clip = LaunchingActivated;
                    break;
                case SoundType.LaunchingTerminated:
                    SoundEffect.clip = LaunchingTerminated;
                    break;
                case SoundType.AccessFiles:
                    SoundEffect.clip = AccessFiles;
                    break;
                case SoundType.Synchronizing:
                    SoundEffect.clip = Synchronizing;
                    break;
                case SoundType.PrepareHyperDrive:
                    SoundEffect.clip = PrepareHyperDrive;
                    break;
                case SoundType.AccessDenied:
                    SoundEffect.clip = AccessDenied;
                    break;
                case SoundType.ActiveHyperDrive:
                    SoundEffect.clip = ActiveHyperDrive;
                    break;
                case SoundType.BackgroundMusic:
                    return BackgroundMusic;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
            return SoundEffect;
        }
    }
}