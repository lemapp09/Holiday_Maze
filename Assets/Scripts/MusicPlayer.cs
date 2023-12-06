using UnityEngine;

public class MusicPlayer : MonoSingleton<MusicPlayer>
{
    public AudioClip[] playlist1;
    public AudioClip[] playlist2;
    private AudioClip[] currentPlaylist;

    private AudioSource audioSource;
    private int currentTrackIndex = -1; // Start before the first index
    private HolidayMazeInput input;

    void Start() {
        RandomizeClips(playlist1);
        RandomizeClips(playlist2);
        audioSource = GetComponent<AudioSource>();
        // Set the default playlist
        currentPlaylist = playlist1;
        PlayNextSong();
        input = new HolidayMazeInput();
        input.SwitchMusic.Enable();
    }

    void Update() {
        // Check if the song has finished playing
        if (!audioSource.isPlaying) {
            PlayNextSong();
        }

        if (input.SwitchMusic.Switch.IsPressed()) {
            SwitchPlaylist();
        }
    }

    // Call this method to switch between playlists
    public void SwitchPlaylist()
    {
        if (currentPlaylist == playlist1)
        {
            currentPlaylist = playlist2;
        }
        else
        {
            currentPlaylist = playlist1;
        }
        // Reset the current track index and play the next song
        currentTrackIndex = -1;
        PlayNextSong();
    }

    // Plays the next song in the current playlist
    private void PlayNextSong()
    {
        currentTrackIndex++;
        // If we reached the end of the playlist, loop back to the start
        if (currentTrackIndex >= currentPlaylist.Length)
        {
            RandomizeClips(currentPlaylist);
            currentTrackIndex = 0;
        }
        // Assign the next song and play it
        audioSource.clip = currentPlaylist[currentTrackIndex];
        audioSource.Play();
    }
    
    public void RandomizeClips(AudioClip[] clips) {
        for (int i = clips.Length - 1; i > 0; i--) {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            // Swap elements
            (clips[i], clips[randomIndex]) = (clips[randomIndex], clips[i]);
        }
    }
    private void OnDisable() {
        input.SwitchMusic.Disable();
    }
}