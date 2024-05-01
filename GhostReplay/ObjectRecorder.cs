using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecorder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The id of the recording to record. Should be unique for the current map.")] 
    private string recordingId;

    private bool isRecording;

    //Temporarely disabled until logic rewrite to facilitate more modules (and be able to make them optional)
    //[Header("Transform")]
    //[SerializeField] private bool recordPosition = true;
    //[SerializeField] private bool recordRotation = true;
    //[SerializeField] private bool recordScale = false;

    public Recording recording { get; private set; } = new();

    /// <summary>
    /// Starts recording the objects' states. If called while this object is already being recorded, discards the current recording and restarts recording from the start.
    /// </summary>
    public void StartRecording()
    {
        isRecording = true;
        recording = new Recording() { id = recordingId };
    }
    /// <summary>
    /// Paused the replay, so it can be continued after <see cref="ResumeRecording"/> is called.
    /// </summary>
    public void PauseRecording() => isRecording = false;

    /// <summary>
    /// Resumes a paused recording.
    /// </summary>
    public void ResumeRecording() => isRecording = true;

    /// <summary>
    /// Stops recording the objects' states and optionally saves the recording to a file.
    /// </summary>
    /// <param name="save">Whether to save the recording to a file. Note that it only saves if this recording was faster than the currently saved one.</param>
    public void StopRecording(bool save)
    {
        isRecording = false;

        if (save)
        {
            RecordingStore.SaveRecording(recording);
        }
    }

    private void FixedUpdate()
    {
        if (isRecording)
        {
            // TIME
            recording.time += Time.fixedDeltaTime;

            // TRANSFORM
            TransformState transformState = new()
            {
                position = new TransformState.Position(transform.position),
                rotation = new TransformState.Rotation(transform.rotation),
                scale = new TransformState.Scale(transform.localScale)
            };

            recording.transformStates.Add(transformState);
        }
    }
}
