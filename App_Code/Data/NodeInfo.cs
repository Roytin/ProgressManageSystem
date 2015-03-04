using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Dapper;

public class NodeInfo
{
    //private readonly static Dictionary<int, NodeInfo>  Nodes = new Dictionary<int, NodeInfo>();

    //public static NodeInfo Get(int id)
    //{
    //    return Nodes[id];
    //}

    public int Id { get; private set; }

    public string ConfigId { get; private set; }

    public NodeConfig Config
    {
        get { return Manager.NodeConfigs.First(x => x.Id == ConfigId); }
    }

    public int ModelId { get; private set; }

    public DateTime SubmitTime { get; private set; }

    public int State { get; private set; }

    public NodeState NodeState
    {
        get { return (NodeState)State; }
        set { State = (int) value; }
    }

    public string JudgeInfo { get; set; }


    private List<JudgeInfo> _infos = null;
    public List<JudgeInfo> JudgeInfos
    {
        get
        {
            if (_infos == null)
            {
                _infos = global::JudgeInfo.Parse(JudgeInfo) ?? new List<JudgeInfo>();
            }
            return _infos;
        }
        set
        {
            _infos = value;
            JudgeInfo = Json.Encode(_infos);
        }
    }

    public int VersionId { get; private set; }
    
    public NodeInfo()
    {
    }

    public static void Load()
    {
        //var models = Manager.Data.Query<NodeInfo>("select * from data_models;");
        //foreach (NodeInfo m in models)
        //{
        //    Nodes.Add(m.Id, m);
        //}

    }
}