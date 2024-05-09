using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class RecordingStore
{
    /// <summary>
    /// The default (relative) location to store and load files.
    /// Location is as follows:
    /// C:/Users/[user]/AppData/LocalLow/[CompanyName]/[ProjectName]/[recordingLocation as set below]/
    /// </summary>
    private static readonly string recordingLocation = "/ghostrecordings/";

    /// <summary>
    /// Saves the recording to the filesystem if this time is better than the currently saved recording
    /// </summary>
    /// <param name="recording">The recording to save. Make sure to include the time and ID in the recording object.</param>
    /// <returns>Whether or not this recording was saved (and thus was faster than the currently saved recording)</returns>
    public static bool SaveRecording(Recording recording)
    {
        if (!Directory.Exists(Application.persistentDataPath + recordingLocation))
        {
            Directory.CreateDirectory(Application.persistentDataPath + recordingLocation);
        }

        if (GetRecordingTime(recording.id) > recording.time)
        {
            string filepath = Path.Combine(Application.persistentDataPath + recordingLocation, recording.id + ".grs");

            string filetext = RecordingtoString(recording);

            File.WriteAllText(filepath, filetext);
            Debug.Log("Saved recording to " + filepath);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Loads a recording with the supplied id.
    /// </summary>
    /// <param name="id">The id of the recording. This is the same id that was used to create the recording.</param>
    /// <returns>The recording if it exists, otherwise null</returns>
    public static Recording LoadRecording(string id)
    {
        try
        {
            string text = File.ReadAllText(Path.Combine(Application.persistentDataPath + recordingLocation, id + ".grs"));
            return StringToRecording(text);
        }
        catch (FileNotFoundException) { return null; }
    }

    /// <summary>
    /// Converts a recording to a string, optimized for size. See <see cref="StringToRecording(string)"/> to get retrieve recording from this string.
    /// </summary>
    /// <param name="recording">The recording to convert</param>
    /// <returns>A string that represents the recording</returns>
    private static string RecordingtoString(Recording recording)
    {
        string metaText = $"{recording.id}:{recording.time};";

        string stateText = "";

        foreach(TransformState ts in recording.transformStates)
        {
            stateText += $"{ts.position.x}:{ts.position.y}:{ts.position.z}/{ts.rotation.x}:{ts.rotation.y}:{ts.rotation.z}:{ts.rotation.w}/{ts.scale.x}:{ts.scale.y}:{ts.scale.z};";
        }

        return metaText + stateText;
    }

    /// <summary>
    /// Converts a string to a recording, optimized for size. See <see cref="RecordingtoString(Recording)"/> to get this string in the first place.
    /// </summary>
    /// <param name="text">The string to convert</param>
    /// <returns>A string that represents the recording</returns>
    private static Recording StringToRecording(string text)
    {
        Recording recording = new();

        string[] lines = text.Split(';');
        string[] metaText = lines[0].Split(':');
        recording.id = metaText[0];
        recording.time = float.Parse(metaText[1]);

        //rest of lines
        foreach (string line in lines[1..^1])
        {
            string[] states = line.Split('/');

            recording.transformStates.Add(new TransformState()
            {
                position = new TransformState.Position(states[0]),
                rotation = new TransformState.Rotation(states[1]),
                scale = new TransformState.Scale(states[2])
            });
        }

        return recording;
    }

    /// <summary>
    /// Gets the total time for a specified recording.
    /// </summary>
    private static float GetRecordingTime(string id)
    {
        try
        {
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath + recordingLocation, id + ".grs"));
            return float.Parse(json.Split(';')[0].Split(':')[1]);
        }
        catch
        {
            return int.MaxValue;
        }
    }
}
