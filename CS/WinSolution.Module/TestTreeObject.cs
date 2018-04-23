using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    [DefaultClassOptions]
    public class TestTreeObject : HCategory {
        public TestTreeObject(Session session) : base(session) { }
    }
}
