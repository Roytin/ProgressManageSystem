using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Dapper;

/// <summary>
/// LogInfo 的摘要说明
/// </summary>
public class LogInfo
{
    public DateTime LogTime { get; set; }
    public string Behavior { get; set; }

    public string IP { get; set; }
    public string Data { get; set; }
    public string UserNickName { get; set; }
    public string ModelName { get; set; }
    public string NodeName { get; set; }
	public LogInfo()
	{
	}

    public LogInfo(LogBehavior behavior, HttpRequestBase request,string nodeName, string modelName, string userNickName, string data="")
    {
        Behavior = behavior.ToString("G");
        Data = data;
        LogTime = DateTime.Now;
        IP = request.UserHostAddress;
        UserNickName = userNickName;
        NodeName = nodeName;
        ModelName = modelName;
    }

    public static void Log(LogInfo logInfo)
    {
        Manager.Data.Execute("INSERT INTO log_behavior(`LogTime`,`Behavior`,`Data`,`IP`,`UserNickName`,`NodeName`,`ModelName`) " +
                             "VALUES(@LogTime,@Behavior,@Data,@IP,@UserNickName,@NodeName,@ModelName);",
            logInfo);
    }

}

public enum LogBehavior
{
    新建功能,
    保存成果,
    修改成果,
    提交审核,
    审核通过,
    审核驳回,
    跳过审核,
}