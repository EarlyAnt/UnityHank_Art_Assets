using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gululu.Util;
using System.Security;
using System;
using Mono.Xml;
using Hank.Data;
using System.IO;
using Google.Protobuf;

public class TaskXml2Bin : SingletonMono<TaskXml2Bin>
{

    TaskSettingStorage taskSettingStorage = new TaskSettingStorage();
    public void ConvertTaskSettingXml2Bin()
    {
        taskSettingStorage.Tasksettings.Clear();

        string targetPath = Application.dataPath + "/Config/Tasks";

        DirectoryInfo originDirInfo = new DirectoryInfo(targetPath);
        System.IO.FileInfo[] files = (originDirInfo.GetFiles("*.xml", SearchOption.AllDirectories));

        foreach (FileInfo file in files)
        {
            SecurityParser xmlDoc = new SecurityParser();
            StreamReader reader = file.OpenText();

            xmlDoc.LoadXml(reader.ReadToEnd());
            reader.Close();
            SecurityElement rootProject = xmlDoc.ToXml().SearchForChildByTag("Quest");

            TaskSetting taskSetting = new TaskSetting();
            taskSetting.TaskId = Convert.ToUInt32(rootProject.Attribute("Id"));
            taskSetting.QuestType = Convert.ToUInt32(rootProject.Attribute("QuestType"));
            taskSetting.Difficulty = Convert.ToUInt32(rootProject.Attribute("Difficulty"));
            taskSetting.IsRepeat = Convert.ToUInt32(rootProject.Attribute("IsRepeat"));
            taskSetting.CanDisplay = Convert.ToUInt32(rootProject.Attribute("CanDisplay"));
            taskSetting.IsValidity = Convert.ToUInt32(rootProject.Attribute("IsValidity"));
            taskSetting.Description = (rootProject.Attribute("Description"));

            ArrayList nodeList = rootProject.Children;

            foreach (SecurityElement xe in nodeList)
            {
                if (xe.Tag == "Rewards")
                {
                    ArrayList rewardList = xe.Children;
                    foreach (SecurityElement xeReward in rewardList)
                    {
                        if (xeReward.Tag == "Reward")
                        {
                            Reward reward = new Reward();
                            reward.ItemID = Convert.ToUInt32(xeReward.Attribute("ItemId"));
                            reward.Value = Convert.ToSingle(xeReward.Attribute("Value"));
                            taskSetting.Rewards.Add(reward);
                        }
                    }

                }
                else if (xe.Tag == "CommitRequirements")
                {
                    ArrayList rewardList = xe.Children;
                    foreach (SecurityElement xeReward in rewardList)
                    {
                        if (xeReward.Tag == "Requirement")
                        {
                            Requirement requirement = new Requirement();
                            requirement.Type = Convert.ToUInt32(xeReward.Attribute("Type"));
                            requirement.Value = Convert.ToSingle(xeReward.Attribute("Value"));
                            taskSetting.CommitRequirements.Add(requirement);
                        }
                    }

                }
                else if (xe.Tag == "RecieveRequirements")
                {
                    ArrayList rewardList = xe.Children;
                    foreach (SecurityElement xeReward in rewardList)
                    {
                        if (xeReward.Tag == "Requirement")
                        {
                            Requirement requirement = new Requirement();
                            requirement.Type = Convert.ToUInt32(xeReward.Attribute("Type"));
                            requirement.Value = Convert.ToSingle(xeReward.Attribute("Value"));
                            taskSetting.RecieveRequirements.Add(requirement);
                        }
                    }

                }
            }

            taskSettingStorage.Tasksettings.Add(taskSetting);
        }



        string accountPath = Application.dataPath + "/StreamingAssets/Tasks.bytes";
        try
        {
            FileStream fs = File.Open(accountPath, FileMode.Create, FileAccess.Write);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(fs);

                taskSettingStorage.WriteTo(ms);
                ms.Position = 0;
                bw.Write((int)ms.Length);
                bw.Write(ms.GetBuffer(), 0, (int)ms.Length);
                bw.Close();
            }

            fs.Close();
            fs.Dispose();
            Debug.Log("SaveAccountFile:" + accountPath + ", OK!");
        }
        catch (Exception e)
        {
            Debug.Log("SaveAccountFile:" + e.ToString() + accountPath);
        }
    }
}
