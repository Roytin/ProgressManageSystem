using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

/// <summary>
/// JudgeInfo 的摘要说明
/// </summary>
public class JudgeInfo
{
    public int JudgerId { get; set; }
    public int Result { get; private set; }

    public JudgeResult JudgeResult
    {
        get { return (JudgeResult) Result; }
        set { Result = (int) value; }
    }

    public JudgeInfo()
	{
	}

    public static List<JudgeInfo> Parse(string json)
    {
        return Json.Decode<List<JudgeInfo>>(json);
    }
}

public enum JudgeResult
{
    审核未通过 = -1,
    未审核     = 0,
    审核通过   = 1,
}