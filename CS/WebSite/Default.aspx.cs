using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxClasses;

public partial class _Default : Page {
    private int LastIndex {
        get {
            if (Session["LastIndex"] == null) Session["LastIndex"] = 0;
            return (int)Session["LastIndex"];
        }
        set { Session["LastIndex"] = value; }
    }

    private SortedDictionary<int, string> OpenTabPagesCollection {
        get {
            if (Session["TabPages"] == null) Session["TabPages"] = new SortedDictionary<int, string>();
            return (SortedDictionary<int, string>)Session["TabPages"];
        }
        set { Session["TabPages"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!Page.IsPostBack) {
            Session.Clear();
            OpenTabPagesCollection.Add(0, "Tab0");
        }
        CreatePages();
        ASPxCallbackPanel1.Controls.Add(page_ctrl);
    }

    private void CreatePages() {
        foreach (KeyValuePair<int, string> tabPage in OpenTabPagesCollection)
            CreateTabPage(tabPage.Key, tabPage.Value);
    }

    private void CreateTabPage(int index, string name) {
        TabPage tp = new TabPage();
        page_ctrl.TabPages.Add(tp);
        tp.Name = name;
        tp.TabTemplate = new TabTemplate(tp.Index, OpenTabPagesCollection.Keys.Count);
        CreateContent(tp);
    }

    private void CreateContent(TabPage tp) {
        Literal literal = new Literal();
        literal.ID = "literal" + tp.Name;
        literal.Text = tp.Name;
        tp.Controls.Add(literal);
    }

    private void DeleteTab(int index) {
        int dicIndex = 0;
        string tabName = page_ctrl.TabPages[index].Name;
        dicIndex = OpenTabPagesCollection.FirstOrDefault(x => x.Value == tabName).Key;
        OpenTabPagesCollection.Remove(dicIndex);
        page_ctrl.TabPages.RemoveAt(index);
    }

    protected void Callback_panel_Callback(object sender, CallbackEventArgsBase e) {
        if (e.Parameter == "new") {
            int newIndex = LastIndex + 1;
            string newTabName = "Tab" + newIndex;
            OpenTabPagesCollection.Add(newIndex, newTabName);
            page_ctrl.TabPages.Clear();
            CreatePages();
            LastIndex++;
        }
        else
            DeleteTab(Convert.ToInt32(e.Parameter));
    }
}