using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.XtraTreeList;
using DevExpress.ExpressApp.Win.Controls;

namespace WinSolution.Module.Win {
    public partial class FilterTreeListViewController : ViewController {
        public FilterTreeListViewController() {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "TestTreeObject_ListView";
        }
        protected override void OnActivated() {
            base.OnActivated();
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }
        void View_ControlsCreated(object sender, EventArgs e) {
            TreeListEditor treeListEditor = (TreeListEditor)((ListView)View).Editor;
            treeList = treeListEditor.TreeList;
            treeList.KeyDown += new System.Windows.Forms.KeyEventHandler(treeList_KeyDown);
        }

        void treeList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == System.Windows.Forms.Keys.F3) {
                Search(parametrizedAction1.Value as string);
            }
        }
        private void Search(string value) {
            if (!string.IsNullOrEmpty(value)) {
                DoSearchInternal(value);
            }
        }
        private void parametrizedAction1_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
            if (startNode == null) {
                startNode = treeList.Nodes.FirstNode;
                currentNode = startNode;
                treeList.FocusedNode = null;
            }
            Search(e.ParameterCurrentValue as string);
        }
        TreeList treeList;
        TreeListNode startNode;
        TreeListNode currentNode;
        public void DoSearchInternal(string textToSearch) {
            while (currentNode != null) {
                string value = (string)currentNode["Name"];
                if (value.Contains(textToSearch)) {
                    treeList.FocusedNode = currentNode;
                    currentNode = GetNextNode(currentNode);
                    if (currentNode == null)
                        currentNode = startNode;
                    return;
                }
                currentNode = GetNextNode(currentNode);
                if (currentNode == null) {
                    currentNode = startNode;
                    break;
                }
            }
        }
        protected TreeListNode GetNextNode(TreeListNode node) {
            if (node == null) return null;
            ObjectTreeListNode onode = node as ObjectTreeListNode;
            if (onode != null)
                ((ObjectTreeList)onode.TreeList).BuildChildNodes(onode);
            if (node.Nodes.Count > 0) return node.Nodes[0];
            TreeList treeList = node.TreeList;
            if (node.ParentNode != null) {
                TreeListNodes owner = node.ParentNode.Nodes;
                while (node == owner.LastNode) {
                    if (owner == treeList.Nodes) return null;
                    if (node.ParentNode == null) return null;
                    node = node.ParentNode;

                    owner = node.ParentNode == null ? treeList.Nodes : node.ParentNode.Nodes;
                }
                int index = owner.IndexOf(node);
                return owner[index + 1];
            }
            else {
                if (treeList.Nodes.LastNode == node) return null;
                else {
                    int index = treeList.Nodes.IndexOf(node);
                    return treeList.Nodes[index + 1];
                }
            }
        }
    }
}
