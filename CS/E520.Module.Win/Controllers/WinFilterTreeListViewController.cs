using System;
using DevExpress.ExpressApp;
using DevExpress.XtraTreeList;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base.General;
using DevExpress.Data.Filtering;

namespace E520.Module.Win.Controllers {
    public class WinFilterTreeListViewController : ObjectViewController<ListView, ITreeNode> {
        TreeList treeList;
        public WinFilterTreeListViewController() {
            ParametrizedAction findNodeAction = new ParametrizedAction(this, "FindNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, typeof(String));
            findNodeAction.Caption = "Smart Search";
            findNodeAction.Execute += new ParametrizedActionExecuteEventHandler(findNodeAction_Execute);
            ParametrizedAction focusNodeAction = new ParametrizedAction(this, "FocusNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, typeof(String));
            focusNodeAction.Caption = "Focus";
            focusNodeAction.Execute += new ParametrizedActionExecuteEventHandler(focusNodeAction_Execute);
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            TreeListEditor treeListEditor = View.Editor as TreeListEditor;
            if (treeListEditor != null) {
                treeList = treeListEditor.TreeList;
                treeList.OptionsBehavior.EnableFiltering = true;
                treeList.OptionsFilter.FilterMode = FilterMode.Extended;
            }
        }
        void focusNodeAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            string textToSearch = e.ParameterCurrentValue as String;
            TreeListNode matchingNode = null;
            if (!String.IsNullOrEmpty(textToSearch)) {
                matchingNode = treeList.FindNode(node => {
                    object currentObject = node.Tag;
                    ITypeInfo currentObjectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(currentObject.GetType());
                    string value = currentObjectTypeInfo.DefaultMember.GetValue(currentObject) as string;
                    if (value != null && value.ToLower().Contains(textToSearch.ToLower())) {
                        return true;
                    } else {
                        node.Expanded = true;
                        return false;
                    }
                });
            }
            treeList.FocusedNode = matchingNode ?? treeList.Nodes.FirstNode;
        }
        void findNodeAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            string value = e.ParameterCurrentValue as String;
            if (!String.IsNullOrEmpty(value)) {
                treeList.ExpandAll();
            }
            treeList.ApplyFindFilter(value);
            // Alternatively, use the TreeList.ActiveFilterCriteria property
            // treeList.ActiveFilterCriteria = new DevExpress.Data.Filtering.FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(value)); 
        }

        protected override void OnDeactivated() {
            treeList = null;
            base.OnDeactivated();
        }
    }
}
