function itemUsageDelete() {
    var actionNode = UmbClientMgr.mainTree().getActionNode();
    if (actionNode.nodeType != '') {
        if (actionNode.nodeType == "content") {
            UmbClientMgr.openModalWindow("/umbraco/dialogs/ItemUsageDelete.aspx?id=" + actionNode.nodeId + '&rnd=' +  Umbraco.Utils.generateRandom(), 'delete', true, 600, 425);
        }
    }
}