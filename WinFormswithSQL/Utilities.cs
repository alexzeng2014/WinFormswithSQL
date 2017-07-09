using System;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Utilities 的摘要说明
/// </summary>
public class Utilities
{
    public Utilities()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public static void SendMail(string from, string to, string subject, string body)
    {
        SmtpClient mailClient = new SmtpClient("smtp.qq.com");
        mailClient.Credentials = new NetworkCredential("hi@alexzeng.net", "*******");
        MailMessage mailMessage = new MailMessage(from, to, subject, body);
        mailClient.Send(mailMessage);
    }

    public static void LogError(Exception ex)
    {
        string datatime = DateTime.Now.ToString() + ",at " + DateTime.Now.ToShortTimeString();
        string errorMessage = "错误发生在" + datatime;
       
        errorMessage += "\n\n 错误信息：" + ex.Message;
        errorMessage += "\n\n 错误源：" + ex.Source;
        errorMessage += "\n\n 目标网站：" + ex.TargetSite;
        errorMessage += "\n\n 堆栈跟踪：" + ex.StackTrace;

       
            string from = "hi@alexzeng.net";
            string to = "alexzeng@msn.com";
            string subject = "程序错误返回信息";
            string body = errorMessage;
            SendMail(from, to, subject, body);
        
    }
}