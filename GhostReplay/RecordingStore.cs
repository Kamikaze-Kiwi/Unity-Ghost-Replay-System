using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        Recording previousRecording = LoadRecording(recording.id);
        if (previousRecording == null || previousRecording.time > recording.time)
        {
            string filepath = Path.Combine(Application.persistentDataPath + recordingLocation, recording.id + ".json");
            Debug.Log("Saved recording to " + filepath);

            string json = JsonConvert.SerializeObject(recording, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText(filepath, json);

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
            string json = File.ReadAllText(Path.Combine(Application.persistentDataPath + recordingLocation, id + ".json"));
            return JsonConvert.DeserializeObject<Recording>(json);
        }
        catch
        {
            return null;
        }
    }
}
