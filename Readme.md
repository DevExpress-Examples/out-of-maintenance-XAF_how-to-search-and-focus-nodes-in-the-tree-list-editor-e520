<!-- default file list -->
*Files to look at*:

* [FilterTreeListViewController.cs](./CS/WinSolution.Module.Win/FilterTreeListViewController.cs) (VB: [FilterTreeListViewController.vb](./VB/WinSolution.Module.Win/FilterTreeListViewController.vb))
* [TestTreeObject.cs](./CS/WinSolution.Module/TestTreeObject.cs) (VB: [TestTreeObject.vb](./VB/WinSolution.Module/TestTreeObject.vb))
<!-- default file list end -->
# How to search and focus nodes in the Tree List editor


<p><strong>Scenario:</strong><br>By default, XAF provides the following capabilities to filter List Views: <a href="https://documentation.devexpress.com/#Xaf/CustomDocument2722">Filter List Views</a>. In some cases, these filtering techniques may be inappropriate for a Tree List, since they filter records at the data source level without taking into account their tree structure. They also do not filter node's children, since child nodes are obtained from a persistent object's Children collection independently on a Tree List data source.<br>To overcome these limitations, it is possible to use the native TreeList filtering in WinForms (see <a href="https://documentation.devexpress.com/#WindowsForms/CustomDocument5551">Filtering</a>) and cell templates customization in ASP.NET.<br><br><strong>Steps to implement:</strong><br>1. Create a ViewController for your Tree List ListView (<strong>WinFilterTreeListViewController</strong> and <strong>WebFilterTreeListViewController</strong> in this example).<br>2. Add actions allowing your users to apply filters to this ListView.<br>3. Handle the Execute event of these actions to <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3165">customize the underlying TreeList control</a> according to the required result.<br><br>In this example, the following functions are implemented:<br><br><strong><br>WinForms:</strong><br> - Finding a node through the <strong>TreeList.FindNode</strong> method and focusing it. To test this function, enter a value in the FocusNode action and click the <strong>Focus</strong> button.<br> - Applying a full text filter to the TreeList control through the <strong>TreeList.ApplyFindFilter</strong> method. To test this function, enter a value in the FindNode action and click the <strong>Smart Search </strong>button.<br>The Search button displayed near the Focus and Smart Search buttons is provided by the built-in FilterController and is not customized. Use it to compare the functionality implemented by the example with a simple data source-level filtering.</p>
<p><br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-search-and-focus-nodes-in-the-tree-list-editor-e520/8.2.4+/media/d6a7c293-0965-11e5-80bf-00155d62480c.png"><br><br></p>
<p>It is also possible to use a native TreeList Find Panel instead of a ParametrizedAction for the full text filtering. Note that nodes will be filtered only if you set the <strong>TreeList.OptionsBehavior.EnableFiltering</strong> property to true. Otherwise, they will be only highlighted.<br>To change the way filtering is applied to the tree hierarchy, use the <strong>TreeList.OptionsFilter.FilterMode</strong> property.<br>If it is necessary to apply a filter only to certain columns, use the <strong>TreeList.ActiveFilterCriteria</strong> property instead of the TreeList.ApplyFindFilter method (see the commented code in the WinFilterTreeListViewController.findNodeAction_Execute event handler).<br>Note that since XAF loads child nodes dynamically when their parent is expanded, filters are not applied to collapsed children. That is why nodes are expanded before filtering in this example.<br><br><br><strong>ASP.NET:</strong><br>Since the ASP.NET TreeList control (ASPxTreeList) does not provide native filtering capabilities, this example demonstrates how to highlight nodes containing the specified text. This functionality is implemented through the <strong>ASPxTreeList.HtmlDataCellPrepared</strong> event, very similar to what is done in the non-XAF <a href="https://www.devexpress.com/Support/Center/p/E4029">ASPxTreeList - How to create an external filter with the ASPxTextBox and highlighting search text</a> example.  In the event handler, cell controls provided by XAF property editors are replaced with labels containing a highlighted text. Note that to highlight a text, a custom style is used. It is declared in the Default.aspx page through the <style> tag. If you do not add this style to your application, nodes will not be highlighted.</p>
<p><br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-search-and-focus-nodes-in-the-tree-list-editor-e520/8.2.4+/media/6d04a53e-0967-11e5-80bf-00155d62480c.png"></p>

<br/>


