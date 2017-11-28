using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class PostBuildHelper
{
	[PostProcessBuild]
	public static void OnBuildComplete(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget != BuildTarget.iOS)
		{
			return;
		}

		IncrementBuildNumber();
	}

	private static void IncrementBuildNumber()
	{
		// Load the PlayerSettings asset.
//		var playerSettings = Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault();

		var currentValue = PlayerSettings.iOS.buildNumber;
		int ver;
		if (int.TryParse(currentValue, out ver))
		{
			// Increment version.
			PlayerSettings.iOS.buildNumber = (ver + 1).ToString();

			// Save player settings.
//			so.ApplyModifiedProperties();
//			AssetDatabase.SaveAssets();
		}
//		EditorApplication.SaveAssets();
//		if (playerSettings != null)
//		{
//			SerializedObject so = new SerializedObject(playerSettings);
//
//			// Find the build number property.
//			var sp = so.FindProperty("iPhoneBuildNumber");
//
//			var currentValue = sp.stringValue;
//			int ver = 0;
//
//			if (int.TryParse(currentValue, out ver))
//			{
//				// Increment version.
//				sp.stringValue = (ver + 1).ToString();
//
//				// Save player settings.
//				so.ApplyModifiedProperties();
//				AssetDatabase.SaveAssets();
//			}
//		}
	}
}