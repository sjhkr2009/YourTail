using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SoundManager
{
    private readonly AudioSource[] audios = new AudioSource[(int)Define.SoundType.Count];
    
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
            root = new GameObject("@Sound");

        Object.DontDestroyOnLoad(root);

        for (int i = 0; i < audios.Length; i++)
        {
            GameObject _audio = new GameObject($"Audio_{(Define.SoundType)i}");
            audios[i] = _audio.GetOrAddComponent<AudioSource>();
            audios[i].playOnAwake = false;
            _audio.transform.parent = root.transform;
        }
        audios[(int)Define.SoundType.BGM].loop = true;
        audios[(int)Define.SoundType.FX].loop = false;
        audios[(int)Define.SoundType.LoopFX].loop = true;
    }

    private bool LoadAudioAndClip(Define.SoundType type, string path, out AudioSource audio, out AudioClip clip) {
        audio = null; clip = null;
        
        if ((int) type >= audios.Length) {
            Debug.LogError($"유효하지 않은 타입입니다: {type}");
            return false;
        }

        audio = audios[(int) type];
        clip = LoadAudioClip(type, path);
        
        if (clip == null)
        {
            Debug.LogError($"'{type}' 타입의 '{path}' 사운드 정보를 찾을 수 없습니다. Resources/SoundSources/ 폴더를 확인해주세요.");
            return false;
        }

        return true;
    }

    public void Play(Define.SoundType type, string path, float volume = 1f)
    {
        if (LoadAudioAndClip(type, path, out var audio, out var source) == false) return;

        switch (type)
        {
            case Define.SoundType.BGM:
            case Define.SoundType.LoopFX:
                audio.clip = source;
                audio.volume = volume;
                audio.Play();
                break;

            case Define.SoundType.FX:
                audio.PlayOneShot(source, volume);
                break;
        }
    }

    private AudioClip LoadAudioClip(Define.SoundType type, string path) {
        switch (type)
        {
            case Define.SoundType.BGM:
                return GameManager.Resource.Load<AudioClip>($"SoundSources/BGM/{path}");
            case Define.SoundType.FX:
                return GameManager.Resource.Load<AudioClip>($"SoundSources/FX/{path}");
            case Define.SoundType.LoopFX:
                return  GameManager.Resource.Load<AudioClip>($"SoundSources/FX/{path}");
        }

        return null;
    }

    public async Task PlayAsync(Define.SoundType type, string path, float duration, float volume = 1f) {
        if (LoadAudioAndClip(type, path, out var audio, out var source) == false) return;

        if (audio.isPlaying) await DoTransition(audio, source, duration, volume);
        else await DoPlay(audio, source, duration, volume);
    }

    private async Task DoTransition(AudioSource audio, AudioClip clip, float duration, float volume) {
        bool done = false;
        audio.DOFade(0f, duration / 2f).OnComplete(() => {
            audio.clip = clip;
            audio.Play();
            audio.DOFade(volume, duration / 2f).OnComplete(() => {
                done = true;
            });
        });
        
        while (!done) {
            await Task.Yield();
        }
    }

    private async Task DoPlay(AudioSource audio, AudioClip clip, float duration, float volume) {
        audio.volume = 0f;
        audio.clip = clip;
        audio.Play();
        
        bool done = false;
        audio.DOFade(volume, duration).OnComplete(() => done = true);
        while (!done) {
            await Task.Yield();
        }
    }
    
    public void Stop(Define.SoundType type)
    {
        audios[(int)type].Stop();
    }
}
