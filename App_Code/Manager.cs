using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Dapper;
using MySql.Data.MySqlClient;

/// <summary>
/// DB 的摘要说明
/// </summary>
public static class Manager
{
    private static string _ConfigConnStr;
    private static string _DataConnStr;
    public static MySqlConnection Config { get{return new MySqlConnection(_ConfigConnStr);} }
    public static MySqlConnection Data { get{return new MySqlConnection(_DataConnStr);} }

    public static IEnumerable<ItemConfig> ItemConfigs = null;
    public static IEnumerable<NodeConfig> NodeConfigs = null;
    public static IEnumerable<UserInfo> AllUserInfos = null;
    public static IEnumerable<VersionConfig> VersionConfigs = null;

	public static void Init()
	{
	    _ConfigConnStr = WebConfigurationManager.ConnectionStrings["ConfigDB"].ConnectionString;
	    _DataConnStr = WebConfigurationManager.ConnectionStrings["DataDB"].ConnectionString;
        //Config.Open();

        //加载配置数据
	    NodeConfigs  = Config.Query<NodeConfig>("select * from config_nodes;");
        ItemConfigs  = Config.Query<ItemConfig>("select * from config_items;");
        AllUserInfos = Config.Query<UserInfo>("select * from data_users;");
        VersionConfigs = Config.Query<VersionConfig>("select * from config_versions;");

        //加载实例数据
        //ModelInfo.Load();
        //NodeInfo.Load();
        //ItemInfo.Load();

        Console.WriteLine("load configs over! ");
	}
}

public class ItemConfig
{
    public string Id { get; private set; }
    public string Name { get; private set; }

    public ItemConfig()
    {

    }
}

public class NodeConfig
{
    public static NodeConfig TopNode
    {
        get { return Manager.NodeConfigs.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Parents)); }
    }

    public string Id { get; private set; }
    public string Name { get; private set; }
    protected string Parents { get; private set; }
    protected string Manifest { get; private set; }
    protected int PurviewBit { get; private set; }
    protected string Judgers { get; private set; }

    private List<string> _JudgerNames;
    public IEnumerable<string> JudgerNames
    {
        get
        {
            if (_JudgerNames == null)
            {
                _JudgerNames = new List<string>();
                foreach (string p in Judgers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _JudgerNames.Add(p);
                }
            }
            return _JudgerNames;
        }
    }

    private List<NodeConfig> _parentNodes;
    public IEnumerable<NodeConfig> ParentNodes
    {
        get
        {
            if (_parentNodes == null)
            {
                _parentNodes = new List<NodeConfig>();
                foreach (string p in Parents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _parentNodes.Add(Manager.NodeConfigs.First(x => x.Id == p));
                }
            }
            return _parentNodes;
        }
    }

    private List<NodeConfig> _childrenNodes;
    public IEnumerable<NodeConfig> ChildrenNodes
    {
        get
        {
            if (_childrenNodes == null)
            {
                _childrenNodes = Manager.NodeConfigs.Where(x => x.Parents.Contains(Id)).ToList();

            }
            return _childrenNodes;
        }
    }

    private List<ItemConfig> _items;
    public IEnumerable<ItemConfig> ManifestItems
    {
        get
        {
            if (_items == null)
            {
                _items = new List<ItemConfig>();
                foreach (string p in Manifest.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    _items.Add(Manager.ItemConfigs.First(x => x.Id == p));
                }
            }
            return _items;
        }
    }

    public NodeConfig()
    {
    }
}

public class VersionConfig
{
    public string Name { get; private set; }
    public int Id { get; private set; }
}