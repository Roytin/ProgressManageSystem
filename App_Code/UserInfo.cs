using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

/// <summary>
/// User 的摘要说明
/// </summary>
public class UserInfo
{
    public int Id { get; private set; }
    public string UserName { get; private set; }
    public string NickName { get; private set; }

    public string EMail { get; private set; }
    
    private UserInfo()
	{
	}

    public static bool TryLogin(string username, string password, out UserInfo user)
    {
        user = null;
        List<UserInfo> users = Manager.Data
            .Query<UserInfo>("select Id,UserName,NickName,EMail from data_users where UserName=@UserName and Password=@Password", new
            {
                UserName=username,
                Password=password
            }) as List<UserInfo>;
        if (users != null && users.Count == 1)
        {
            user = users[0];
            return true;
        }
        else
        {
            return false;
        }
    }

    public static UserInfo CheckLogin(HttpRequestBase request, HttpResponseBase response,HttpSessionStateBase session)
    {
        var userInfo = session["UserInfo"] as UserInfo;
        if (userInfo == null)
        {
            var cookieUsername = request.Cookies["pms-username"];
            var cookiePassword = request.Cookies["pms-password"];
            if (cookieUsername != null && cookiePassword != null)
            {
                string username = cookieUsername.Value;
                string password = cookiePassword.Value;

                if (TryLogin(username, password, out userInfo))
                {
                    session["UserInfo"] = userInfo;
                    return userInfo;
                }
                else
                {
                    response.Write("./login.cshtml?cl=1");
                    return null;
                }
            }
            else
            {
                response.Write("./login.cshtml?cl=1");
                return null;
            }
        }
        else
        {
            return userInfo;
        }
    }
}