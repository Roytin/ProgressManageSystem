using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

/// <summary>
/// Item 的摘要说明
/// </summary>
public class ItemInfo
{
    //private readonly static Dictionary<int,ItemInfo>  Items = new Dictionary<int, ItemInfo>();

    //public static ItemInfo Get(int id)
    //{
    //    return Items[id];
    //}

    public int Id { get; private set; }

    public int NodeId { get; private set; }

    public string ConfigId { get; private set; }

    public ItemConfig Config
    {
        get { return Manager.ItemConfigs.First(x => x.Id == ConfigId); }
    }

    public int ProviderId { get; set; }

    public DateTime ProvideTime { get; set; }

    public string Detail { get; set; }

    public ItemInfo()
    {
        
    }

	public ItemInfo(int nodeId, string configId, int providerId, string detail)
	{
	    NodeId = nodeId;
	    ConfigId = configId;
	    ProviderId = providerId;
	    Detail = detail;
	    ProvideTime = DateTime.Now;
	}

    public static void Load()
    {
        //var models = Manager.Data.Query<ItemInfo>("select * from data_models;");
        //foreach (ItemInfo m in models)
        //{
        //    Items.Add(m.Id, m);
        //}
    }
}