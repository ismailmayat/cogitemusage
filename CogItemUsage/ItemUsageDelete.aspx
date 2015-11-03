<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../masterpages/umbracoDialog.Master" CodeBehind="ItemUsageDelete.aspx.cs" Inherits="CogItemUsage.ItemUsageDelete" %>
<%@ Register TagPrefix="sm" Src="~/usercontrols/ItemUsageDatatype.ascx" TagName="ItemUsageDatatype"  %>

<asp:Content ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <p>Are you sure you want to delete this page?</p>
    
    <p><asp:Literal runat="server" id="litMessage"></asp:Literal></p>
    
    <sm:ItemUsageDatatype id="usageControl" runat="server" ShowOnLoad="1" ></sm:ItemUsageDatatype>
    <div style="margin-right: 15px; margin-top: 40px;">
	    <input type="submit" id="delete" value="Delete" style="Width: 90px; margin-right: 6px;">
	    <em> or </em>  
	    <a href="#" style="color: blue; margin-left: 6px;" onclick="UmbClientMgr.closeModalWindow()">Cancel</a>
    </div>

<script type="text/javascript">

    $(function () {

        $('#delete').click(function () {

            window.top.$(window.top).trigger("nodeDeleting", []);
            
            window.top.umbraco.presentation.webservices.legacyAjaxCalls.Delete(
                UmbClientMgr.mainTree().getActionNode().nodeId, "",
                UmbClientMgr.mainTree().getActionNode().nodeType,
                function () {
                    UmbClientMgr.closeModalWindow();
                    //raise nodeDeleted event
                    window.top.$(window.top).trigger("nodeDeleted", []);
                },
                function (error) {
                    UmbClientMgr.closeModalWindow();
                    //raise public error event
                    window.top.$(window.top).trigger("publicError", [error]);
                }
            );

            return false;
        });
    });

</script>
</asp:Content>
