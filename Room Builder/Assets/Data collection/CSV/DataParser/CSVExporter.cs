using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;
namespace VRGait.Data
{
	public class CSVExporter
	{
		public static bool Write(Vector3[] cornerPositions, uint num_subject, uint num_trial, string str_elevation, string str_type, List<FrameData> datas, List<GameObject> trackObjects)
		{
			string dirPath = Application.persistentDataPath + "/Exports";
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}

			string filePath = dirPath + string.Format("/SUB_{0}_{1}_{2}_{3}_({4}).csv", num_subject, num_trial, str_elevation, str_type, DateTime.Now.ToString("dd-MM-yy HH-mm-ss"));
			using (FileStream fs = File.Create(filePath))
			{
			}

			// Write infomation into the .csv file
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				// Write the fix data title in
				sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
					"Subject Number", "Trial Number", "Trial Type", "Elevation",
					"Corner(Bottom Left) Position X", "Corner(Bottom Left) Positio Y", "Corner(Bottom Left) Position Z",
					"Corner(Bottom Right) Position X", "Corner(Bottom Right) Positio Y", "Corner(Bottom Right) Position Z",
					"Corner(Top Right) Position X", "Corner(Top Right) Positio Y", "Corner(Top Right) Position Z",
					"Corner(Top Left) Position X", "Corner(Top Left) Positio Y", "Corner(Top Left) Position Z"
				));
				// Write the title line in.
				sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
							num_subject, num_trial, str_type, str_elevation,
							cornerPositions[0].x.ToString("f3"), cornerPositions[0].y.ToString("f3"), cornerPositions[0].z.ToString("f3"),
							cornerPositions[1].x.ToString("f3"), cornerPositions[1].y.ToString("f3"), cornerPositions[1].z.ToString("f3"),
							cornerPositions[2].x.ToString("f3"), cornerPositions[2].y.ToString("f3"), cornerPositions[2].z.ToString("f3"),
							cornerPositions[3].x.ToString("f3"), cornerPositions[3].y.ToString("f3"), cornerPositions[3].z.ToString("f3")
				));
				// Write the title in.
				StringBuilder sb = new StringBuilder();
				sb.Append("Elapsed Time,Elevator Height,");
				for (int i = 0; i < trackObjects.Count; i++)
				{
					var trackObject = trackObjects[i];
					sb.Append(trackObject.name + " Position X,");
					sb.Append(trackObject.name + " Position Y,");
					sb.Append(trackObject.name + " Position Z,");
					sb.Append(trackObject.name + " Rotation X,");
					sb.Append(trackObject.name + " Rotation Y,");
					sb.Append(trackObject.name + " Rotation Z,");
				}
				sb.Remove(sb.Length - 1, 1);
				sw.WriteLine(sb.ToString());
				sb.Clear();

				// Write the actual data in.
				for (int i = 0; i < datas.Count; ++i)
				{
					var curData = datas[i];
					sb.Append(curData.elapsedTime.ToString("f3") + ",");
					sb.Append(curData.elevator_height.ToString("f3") + ",");
					for (int j = 0; j < curData.trackPositions.Count; j++)
					{
						var curPosition = curData.trackPositions[j];
						var curRotation = curData.trackRotations[j];
						sb.Append(curPosition.x.ToString("f3") + ",");
						sb.Append(curPosition.y.ToString("f3") + ",");
						sb.Append(curPosition.z.ToString("f3") + ",");
						sb.Append(curRotation.x.ToString("f3") + ",");
						sb.Append(curRotation.y.ToString("f3") + ",");
						sb.Append(curRotation.z.ToString("f3") + ",");
					}
					sb.Remove(sb.Length - 1, 1);
					//Remove the last 
					sw.WriteLine(sb.ToString());
					sb.Clear();
				}
				sw.Close();
			}
			return true;
		}
	}
}