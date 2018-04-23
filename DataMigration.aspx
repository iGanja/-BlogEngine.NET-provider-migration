<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="DataMigration.aspx.cs" Inherits="BlogEngine.NET.admin.Pages.DataMigration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
    <p>
        The Blog Migration tool is used to copy data from your current data provider to
        another provider you already have configured in your web.config.  It is important
        to make sure the provider you copy your data to is already configured properly and
        has be cleared of data.  (The Blog Migration tool doesn't erase data first.)
    </p>
    <p>
        If you are sure you have everything configured properly, then go ahead and select
        the provider you wish to copy your data to and click <b>Copy</b>.  A completion message
        will appear when it has finished the copy.
    </p>
    <p>
        After your copy has been made, you may optionally change the default provider in your
        web.config to use this new default data source for your blog.
    </p>
    <p>
        Note: This only copies your blog posts, pages, settings, and ping services which 
        includes your extension and widget data.  
    </p>
    <p>May the force be with you.</p>
    <hr />
    Copy Data From: <asp:DropDownList ID="DdlBlogs" runat="server" /><br /><br />
    Copy Data to: <asp:DropDownList ID="DdlProviders" runat="server" /><br /><br />
    <asp:Button ID="BtnConvert" runat="server" Text="Copy" OnClick="BtnConvert_OnClick" /><br />
    <asp:Label ID="LblStatus" Text="" ForeColor="Red" runat="server" />
</asp:Content>
