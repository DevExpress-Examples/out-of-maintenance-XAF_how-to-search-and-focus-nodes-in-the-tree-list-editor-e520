using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace E520.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            HCategory parent1 = CreateNode("Parent 1", null);
            CreateNode("Child 1", parent1);
            CreateNode("Child 2", parent1);
            CreateNode("Parent 2", null);
        }
        private HCategory CreateNode(string name, HCategory parent) {
            HCategory obj = ObjectSpace.FindObject<HCategory>(new BinaryOperator("Name", name));
            if (obj == null) {
                obj = ObjectSpace.CreateObject<HCategory>();
                obj.Name = name;
                obj.Parent = parent;
            }
            return obj;
        }
    }
}
