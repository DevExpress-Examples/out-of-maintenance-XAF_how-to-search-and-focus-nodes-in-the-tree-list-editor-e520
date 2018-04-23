using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.TreeListEditors.Web;
using System.Web.UI.WebControls;

namespace E520.Module.Web.Controllers {
    public class WebFilterTreeListViewController : ObjectViewController<ListView, ITreeNode> {
        ASPxTreeList treeList;
        ParametrizedAction findNodeAction;
        public WebFilterTreeListViewController() {
            findNodeAction = new ParametrizedAction(this, "FindNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, typeof(String));
            findNodeAction.Caption = "Smart Search";
            findNodeAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            findNodeAction.Execute += new ParametrizedActionExecuteEventHandler(findNodeAction_Execute);
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            ASPxTreeListEditor treeListEditor = View.Editor as ASPxTreeListEditor;
            if (treeListEditor != null) {
                treeList = treeListEditor.TreeList;
                treeList.HtmlDataCellPrepared += treeList_HtmlDataCellPrepared;
            }
        }
        void treeList_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e) {
            string textToSearch = findNodeAction.Value as String;
            if (!String.IsNullOrEmpty(textToSearch) && e.CellValue != null) {
                string propertyValue = e.CellValue.ToString();
                int textIndex = propertyValue.ToLower().IndexOf(textToSearch.ToLower());
                if (textIndex >= 0) {
                    int spanLength = ("<span class='highlight'>").Length;
                    propertyValue = propertyValue.Insert(textIndex, "<span class='highlight'>");
                    propertyValue = propertyValue.Insert(textIndex + spanLength + textToSearch.Length, "</span>");
                    Label label = new Label();
                    label.Text = propertyValue;
                    e.Cell.Controls.Clear();
                    e.Cell.Controls.Add(label);
                }
            }
        }
        void findNodeAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            string searchText = e.ParameterCurrentValue as String;
            if (!String.IsNullOrEmpty(searchText)) {
                treeList.ExpandAll();
            }
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (treeList != null) {
                treeList.HtmlDataCellPrepared -= treeList_HtmlDataCellPrepared;
                treeList = null;
            }
        }
    }
}
