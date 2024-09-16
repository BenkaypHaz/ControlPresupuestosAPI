using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;

public class ReportViewerCredentials : IReportServerCredentials
{
    private readonly string _username;
    private readonly string _password;
    private readonly string _domain;

    public ReportViewerCredentials(string username, string password, string domain)
    {
        _username = username;
        _password = password;
        _domain = domain;
    }

    public WindowsIdentity ImpersonationUser
    {
        get { return null; } // Use default Windows identity
    }

    public ICredentials NetworkCredentials
    {
        get { return new NetworkCredential(_username, _password, _domain); }
    }

    public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
    {
        authCookie = null;
        user = null;
        password = null;
        authority = null;
        return false; // Not using forms credentials
    }
}
