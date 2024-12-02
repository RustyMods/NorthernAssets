using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Managers;

public class MusicManager
{
    private static readonly List<MusicEnvironment> m_musicEnvironments = new();
    private static readonly List<Music> m_musics = new();
    
    static MusicManager()
    {
        Harmony harmony = new("org.bepinex.helpers.MusicManager");
        harmony.Patch(AccessTools.DeclaredMethod(typeof(MusicMan), nameof(MusicMan.Awake)), prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(MusicManager), nameof(Prefix_MusicMan_Awake))));
    }

    internal static void Prefix_MusicMan_Awake(MusicMan __instance)
    {
        foreach (var music in m_musics)
        {
            MusicMan.NamedMusic namedMusic = new MusicMan.NamedMusic
            {
                m_name = music.m_name,
                m_clips = new[] { music.m_clip },
                m_volume = music.m_volume,
                m_fadeInTime = music.m_fadeInTime,
                m_alwaysFadeout = music.m_alwaysFadeOut,
                m_loop = music.m_loop,
                m_resume = music.m_resume,
                m_ambientMusic = music.m_ambientMusic,
            };
            __instance.m_music.Add(namedMusic);
        }
        
        foreach (MusicEnvironment? music in m_musicEnvironments)
        {
            MusicMan.NamedMusic tune = new MusicMan.NamedMusic()
            {
                m_name = music.m_name,
                m_clips = new[] { music.m_clip },
                m_volume = music.m_volume,
                m_fadeInTime = music.m_fadeInTime,
                m_alwaysFadeout = music.m_alwaysFadeOut,
                m_loop = music.m_loop,
                m_resume = music.m_resume,
                m_ambientMusic = music.m_ambientMusic
            };
            __instance.m_music.Add(tune);
        }
    }

    public static void RegisterEnvironments()
    {
        if (!EnvMan.instance) return;

        foreach (MusicEnvironment? music in m_musicEnvironments)
        {
            if (EnvMan.instance.GetEnv(music.m_clonedEnvironment) is { } env)
            {
                EnvSetup? clone = env.Clone();
                clone.m_name = music.m_name;
                clone.m_musicDay = music.m_name;
                clone.m_musicEvening = music.m_name;
                clone.m_musicMorning = music.m_name;
                clone.m_musicNight = music.m_name;
                // clone.m_fogDensityDay = music.FogDensity;
                // clone.m_fogDensityEvening = music.FogDensity;
                // clone.m_fogDensityMorning = music.FogDensity;
                // clone.m_fogDensityNight = music.FogDensity;
                // clone.m_rainCloudAlpha = music.CloudDensity;
                EnvMan.instance.m_environments.Add(clone);
            }
            else
            {
                Debug.LogWarning(music.m_name + " " + music.m_clonedEnvironment + " is null");
            }
        }
    }

    public class Music
    {
        public readonly string m_name = null!;
        public readonly AudioClip m_clip = null!;
        public float m_volume = 1f;
        public float m_fadeInTime = 3f;
        public bool m_alwaysFadeOut = false;
        public bool m_loop = true;
        public bool m_resume = false;
        public bool m_ambientMusic = true;

        public Music(AssetBundle assetBundle, string assetName)
        {
            if (assetBundle.LoadAsset<AudioClip>(assetName) is { } audioClip)
            {
                m_clip = audioClip;
                m_name = assetName;
                m_musics.Add(this);
            }
            else
            {
                Debug.LogWarning(assetName + " is null");
            }
        }
    }
    
    public class MusicEnvironment
    {
        public readonly string m_name = null!;
        public readonly AudioClip m_clip = null!;
        public string m_clonedEnvironment = "Clear";
        public float m_volume = 1f;
        public float m_fadeInTime = 3f;
        public bool m_alwaysFadeOut = false;
        public bool m_loop = true;
        public bool m_resume = false;
        public bool m_ambientMusic = true;
        public float FogDensity;
        public float CloudDensity;

        public MusicEnvironment(string name, string searchName, AssetBundle bundle)
        {
            if (bundle.LoadAsset<AudioClip>(name) is { } clip)
            {
                m_name = searchName;
                m_clip = clip;
                m_musicEnvironments.Add(this);
            }
            else
            {
                Debug.LogWarning(name + " is null");
            }
        }

        public void SetEnvironmentCopy(string env) => m_clonedEnvironment = env;
    }
}