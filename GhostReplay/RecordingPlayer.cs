using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingPlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The id of the recording to play. Should be unique for the current map.")]
    private string recordingId;

    private bool isPlaying = false;
    private Recording recording;
    private int playingIndex = 0;

    private void Start()
    {
        recording = RecordingStore.LoadRecording(recordingId);
        if (recording == null) this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts replaying the recording from the start. If called again (even if it is currently being replayed), it will replay the recording from the start again.
    /// </summary>
    public void StartReplay()
    {
        isPlaying = true;
        playingIndex = 0;
    }

    /// <summary>
    /// Paused the replay, so it can be continued after <see cref="ResumeReplay"/> is called.
    /// </summary>
    public void PauseReplay() => isPlaying = false;

    /// <summary>
    /// Resumes a paused replay.
    /// </summary>
    public void ResumeReplay() => isPlaying = true;

    private void FixedUpdate()
    {
        if (isPlaying && playingIndex + 1 < recording.transformStates.Count)
        {
            TransformState ts = recording.transformStates[playingIndex];
            transform.SetPositionAndRotation(
                new Vector3 (ts.position.x, ts.position.y, ts.position.z), 
                new Quaternion(ts.rotation.x, ts.rotation.y, ts.rotation.z, ts.rotation.w)
            );
            transform.localScale = new Vector3(ts.scale.x, ts.scale.y, ts.scale.z);
            playingIndex++;
        }
    }
}
