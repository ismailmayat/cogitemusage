<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemUsageDatatype.ascx.cs" Inherits="CogItemUsage.ItemUsageDatatype" %>
<%@ Import Namespace="Examine" %>
<style type="text/css">
    table.itemUsage
    {
        border-color: #BBB;
        border-style: solid;
        border-width: 1px;
        border-collapse: collapse;
     }
     .itemUsage th
     {
         background: #CCC;
     }
     .itemUsage th, .itemUsage td{
          border-color : #BBB;
          border-style: solid;
          border-width: 1px;
          
     }
    .itemUsage{margin-top:10px;}
    .itemUsage tr.even{background: #E5E5E5;height:30px;}
    .itemUsage tr:hover{background: #BBB;}
    .itemUsage th{font-weight:bold;height:30px;text-align:left;padding-left:5px;}
    .itemUsage th.pageName{width:50%;}
    .itemUsage th.reviewDate{width:10%;}
    .itemUsage th.status{width:5%;text-align:center;}
    .itemUsage th.action{width:30%;}
    .itemUsage td{text-align:left;padding-left:5px;}
    .itemUsage td.trafficImage{text-align:center;} 
</style>
<input type="hidden" runat="server" id="queryGenerated" />
<p><asp:Literal runat="server" id="litMessage"></asp:Literal></p>
<asp:Repeater runat="server" ID="rptItemUsage">
    <HeaderTemplate>
        <table width="90%" class="itemUsage">
    <thead>
        <th class="pageName">
           Page name
        </th>     
        <th class="status">
            Published
        </th>
    </thead>
    </HeaderTemplate>
    <ItemTemplate>
       <tr class="odd">
        <td><a href='/umbraco/editContent.aspx?id=<%#((SearchResult)Container.DataItem).Fields["id"]%>'> <%#((SearchResult)Container.DataItem).Fields["nodeName"]%></a></td>       
        <td class="trafficImage"><%#((SearchResult)Container.DataItem).Fields["IsPublished"]%></td> 
       </tr>

    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr class="even">
        <td><a href='/umbraco/editContent.aspx?id=<%#((SearchResult)Container.DataItem).Fields["id"]%>'> <%#((SearchResult)Container.DataItem).Fields["nodeName"]%></a></td>       
        <td class="trafficImage"><%#((SearchResult)Container.DataItem).Fields["IsPublished"]%></td> 
       </tr>
    </AlternatingItemTemplate>
    
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Button runat="server" Text="Find usage" id="findUsage" onclick="findUsage_Click" Visible="false"/>
