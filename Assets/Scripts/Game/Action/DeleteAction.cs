using Hank.Api;
using System;
using UnityEngine;

namespace Hank.Action
{
    public class DeleteAction : ActionBase
    {
        [Inject]
        public IResourceUtils ResourceUtils { get; set; }
        [Inject]
        public IDrinkWaterUIPresentManager UIState { get; set; }
        private HeartBeatDelete data;

        public override void Init(HeartBeatActionBase actionModel)
        {
            Debug.LogFormat("<><DeleteAction.Init>actionModel: {0}", actionModel.todoid);
            data = actionModel as HeartBeatDelete;
        }

        public override void DoAction()
        {
            bool error = true;
            string errorText = "";
            GuLog.Debug(string.Format("<><DeleteAction.DoAction>todoid: {0}, content: {1}", this.data.todoid, this.data.content));
            if (!string.IsNullOrEmpty(data.content))
            {
                string[] parts = data.content.Split('=');
                if (parts != null && parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                {
                    GuLog.Debug(string.Format("<><DeleteAction.DoAction>Type: {0}, Path: {1}", parts[0], parts[1]));
                    try
                    {
                        switch (parts[0].ToUpper())
                        {
                            case "FILE":
                                {
                                    this.ResourceUtils.DeleteFile(parts[1], (result) =>
                                    {
                                        error = false;
                                    }, (resultError) =>
                                    {
                                        errorText = string.Format("<><DeleteAction.DoAction><delete file>Result error: {0}", resultError);
                                    });
                                }
                                break;
                            case "FOLDER":
                                {
                                    this.ResourceUtils.DeleteFolder(parts[1], (result) =>
                                    {
                                        error = false;
                                    }, (resultError) =>
                                    {
                                        errorText = string.Format("<><DeleteAction.DoAction><delete folder>Result error: {0}", resultError);
                                    });
                                }
                                break;
                            case "MODULE":
                                {
                                    this.ResourceUtils.DeleteModule(parts[1], (result) =>
                                    {
                                        error = false;
                                    }, (resultError) =>
                                    {
                                        errorText = string.Format("<><DeleteAction.DoAction><delete module>Result error: {0}", resultError);
                                    });
                                }
                                break;
                            default:
                                errorText = string.Format("<><DeleteAction.DoAction><unknown operate>todoid: {0}, operate: {1}, path: {2}", this.data.todoid, parts[0], parts[1]);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorText = string.Format("<><DeleteAction.DoAction><unknown error>todoid: {0}, content: {1}, message: {0}", this.data.todoid, this.data.content, ex.Message);
                    }
                }
                else errorText = string.Format("<><DeleteAction.DoAction><content format error>todoid: {0}, content: {1}", this.data.todoid, this.data.content);
            }
            else errorText = string.Format("<><DeleteAction.DoAction><content is null>todoid: {0}, content: {1}", this.data.todoid, this.data.content);

            if (error)
            {
                GuLog.Error(errorText);
                BuildBackAckBody(this.data.ack, errorText, "ERROR", this.data.todoid);
            }
            else
            {
                BuildBackAckBody(this.data.ack, "Delete", "OK", this.data.todoid);
            }
            Loom.QueueOnMainThread(() =>
            {
                this.UIState.PushNewState(UIStateEnum.eRefreshMainByData);
            });
        }
    }
}
