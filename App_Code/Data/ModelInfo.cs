using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;

public class ModelInfo
{
    //private readonly static Dictionary<int,ModelInfo> Models = new Dictionary<int, ModelInfo>();

    //public static ModelInfo Get(int id)
    //{
    //    return Models[id];
    //}

    //public static IEnumerable<ModelInfo> All
    //{
    //    get { return Models.Values; }
    //}

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public int CreatorId { get; private set; }

    public DateTime CreateTime { get; private set; }

    public DateTime CompleteTime { get; private set; }

    public int VersionId { get; private set; }

    public ModelInfo()
    {
        
    }
    public static void Load()
    {
        //var models = Manager.Data.Query<ModelInfo>("select * from data_models;");
        //foreach (ModelInfo m in models)
        //{
        //    Models.Add(m.Id,m);
        //}
    }

    public static ModelInfo CreateNew(int versionId, int userId, string name, string description)
    {
        ModelInfo newModel = new ModelInfo()
        {
            VersionId = versionId,
            Name = name,
            CreatorId = userId,
            CreateTime = DateTime.Now,
            CompleteTime = DateTime.MaxValue,
            Description = description
        };

        if (Manager.Data.Execute(
            "INSERT INTO data_models(`Name`,`CreatorId`,`CreateTime`,`CompleteTime`,`Description`,`VersionId`) VALUES(@Name,@CreatorId,@CreateTime,@CompleteTime,@Description,@VersionId)",
            newModel) > 0)
        {
            var first = Manager.Data.Query<ModelInfo>("select * from data_models where Name=@Name and VersionId=@VersionId", newModel).First();
            //Models.Add(first.Id, first);
            //一些初始化工作, 创建第一级节点
            StringBuilder sbSql = new StringBuilder();
            foreach (var nodeConfig in Manager.NodeConfigs)
            {
                int state = (int)NodeState.没开始;
                if (!nodeConfig.ParentNodes.Any())
                {
                    state = (int) NodeState.进行中;
                }
                sbSql.AppendFormat("INSERT INTO data_nodes(`ConfigId`,`ModelId`,`SubmitTime`,`State`,`VersionId`) " +
                             "VALUES('{0}','{1}','{2}','{3}','{4}');", 
                             nodeConfig.Id, 
                             first.Id,
                             DateTime.MinValue, 
                             state,
                             versionId);
            }
            Manager.Data.Execute(sbSql.ToString());
            return first;
        }
        else
        {
            return null;
        }
    }
}

public enum NodeState
{
    没开始 = 0,
    进行中 = 1,
    审核中 = 2,
    已审核 = 3,
}