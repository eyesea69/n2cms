<%@ Page MasterPageFile="..\Framed.Master" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="N2.Edit.Versions.Default" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="Head" runat="server">
   <link rel="stylesheet" href="Css/Versions.css" type="text/css" />
	<script src="../Js/plugins.ashx" type="text/javascript" ></script>
    <script type="text/javascript">
        $(document).ready(function(){
			toolbarSelect("versions");
		});
	</script>
</asp:Content>
<asp:Content ID="ContentToolbar" ContentPlaceHolderID="Toolbar" runat="server">
    <asp:HyperLink ID="hlCancel" runat="server" CssClass="cancel command" meta:resourceKey="hlCancel">cancel</asp:HyperLink>
</asp:Content>
<asp:Content ID="ContentContent" ContentPlaceHolderID="Content" runat="server">
	<h1>Versions</h1>
    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" DataKeyNames="ID" CssClass="gv" AlternatingRowStyle-CssClass="alt" UseAccessibleHeader="true" BorderWidth="0" OnRowCommand="gvHistory_RowCommand" OnRowDeleting="gvHistory_RowDeleting">
		<Columns>
            <asp:TemplateField HeaderText="V" meta:resourceKey="v" ItemStyle-CssClass="Version">
				<ItemTemplate>
				    <%# IsPublished(Container.DataItem) ? "<img src='img/bullet_star.gif' alt='published' />" : string.Empty %>
				    <%# Container.DataItemIndex + 1 %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Title" meta:resourceKey="title" >
				<ItemTemplate><a href="<%# GetUrl((N2.ContentItem)Container.DataItem) %>" title="<%# Eval("ID") %>"><img alt="icon" src='<%# VirtualPathUtility.ToAbsolute((string)Eval("IconUrl")) %>'/><%# string.IsNullOrEmpty(((N2.ContentItem)Container.DataItem).Title) ? "(untitled)" : ((N2.ContentItem)Container.DataItem).Title %></a></ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField HeaderText="Published" DataField="Published" meta:resourceKey="published" />
			<asp:BoundField HeaderText="Expired" DataField="Expires" meta:resourceKey="expires" />
			<asp:BoundField HeaderText="Saved by" DataField="SavedBy" meta:resourceKey="savedBy" />
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink runat="server" ID="hlEdit" meta:resourceKey="hlEdit" Text="Edit" NavigateUrl='<%# Engine.EditManager.GetEditExistingItemUrl((N2.ContentItem)Container.DataItem) %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:LinkButton runat="server" ID="btnPublish" meta:resourceKey="btnPublish" Text="Publish" CommandName="Publish" CommandArgument='<%# Eval("ID") %>' Visible="<%# !IsPublished(Container.DataItem) %>" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:LinkButton runat="server" ID="btnDelete" meta:resourceKey="btnDelete" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' Visible="<%# !IsPublished(Container.DataItem) %>" />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>