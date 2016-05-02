using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication
{
    public partial class HOME : System.Web.UI.Page
    {
        ExternalData myExternalData = new ExternalData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        private static Boolean updated = false;
        private static DataTable dtView;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
           

        }

        protected void CheckBoxName_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxName.Checked == true)
            {
                RequiredName.Enabled = true;
                RequiredCode.Enabled = false;

                CheckBoxCode.Checked = false;
            }

        }

        protected void CheckBoxCode_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxCode.Checked == true)
            {
                RequiredCode.Enabled = true;
                RequiredName.Enabled = false;
                CheckBoxName.Checked = false;
            }
        }

        

        protected void search_Click(object sender, EventArgs e)
        {

            DataTable dtType = null;
            if (IsValid == false)
            {
                dtType = null;
                gvStock.DataSource = dtType;
                gvStock.DataBind();

                gvStock.Visible = true;
                
                return;
            }
            UnloadGV();
            string securityType = ddlType.SelectedItem.ToString().Trim();
            string securityName = "-1";
            string securityCode = "-1";
            
            if (CheckBoxName.Checked == true)
            {
                securityName = TextBoxName.Text.ToString().Trim();
                string newName = securityName;
                int count = 0;
                securityName = securityName.Replace("'", "''");
                string sql;
                
                if (securityType == "Stock")
                {
                    sql = "select * from[" + securityType + "] where [name] like '%" + securityName + "%' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvStock.DataSource = sortedView;

                    gvStock.DataBind();

                    gvStock.Visible = true;
                    

                }
                else if (securityType == "Bond")
                {
                    sql = "select * from[" + securityType + "] where [name] like '%" + securityName + "%' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvBond.DataSource = sortedView;

                    gvBond.DataBind();

                    gvBond.Visible = true;
                    
                }
                else
                {
                    sql = "select * from[" + securityType + "] where [name] like '%" + securityName + "%' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvUnitTrust.DataSource = sortedView;

                    gvUnitTrust.DataBind();

                    gvUnitTrust.Visible = true;
                    
                }
                
                
            }
            else if (CheckBoxCode.Checked == true)
            {
                securityCode = TextBoxCode.Text.ToString().Trim();
                int count = 0;
                foreach (Char c in securityCode.ToCharArray())
                {
                    if (c == '0')
                    {
                        count++;
                    }
                    else break;
                }
                securityCode = securityCode.Substring(count, securityCode.Length - count);
                string sql;
                
                if (securityType == "Stock")
                {
                    sql = "select * from [" + securityType + "] where [code] = '" + securityCode + "' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvStock.DataSource = sortedView;

                    gvStock.DataBind();

                    gvStock.Visible = true;
                    

                }
                else if (securityType == "Bond")
                {
                    sql = "select * from [" + securityType + "] where [code] = '" + securityCode + "' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvBond.DataSource = sortedView;

                    gvBond.DataBind();

                    gvBond.Visible = true;
                    
                }
                else
                {
                    sql = "select * from [" + securityType + "] where [code] = '" + securityCode + "' ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvUnitTrust.DataSource = sortedView;

                    gvUnitTrust.DataBind();

                    gvUnitTrust.Visible = true;
                    
                }


            }
            else
            {
                string sql;
                //sql = "select * from [" + securityType + "] where ";
                

                if (securityType == "Stock")
                {
                    sql = "select * from [" + securityType + "]";
                    
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvStock.DataSource = sortedView;

                    gvStock.DataBind();

                    gvStock.Visible = true;
                    

                }
                else if (securityType == "Bond")
                {
                    sql = "select * from [" + securityType + "] ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvBond.DataSource = sortedView;

                    gvBond.DataBind();

                    gvBond.Visible = true;
                    
                }
                else
                {
                    sql = "select * from [" + securityType + "] ";
                    dtType = myExternalData.getData(sql);
                    if (dtType == null)
                    {
                        return;
                    }

                    if (dtType.Rows.Count == 0)
                    {
                        return;
                    }
                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "ASC";
                    dtView = dtType.Clone();
                    for (int i = 0; i < dtType.Rows.Count; i++)
                    {
                        dtView.ImportRow(dtType.Rows[i]);
                    }
                    //
                    DataView sortedView = new DataView(dtView);
                    sortedView.Sort = "name" + " " + "Asc";
                    gvUnitTrust.DataSource = sortedView;

                    gvUnitTrust.DataBind();

                    gvUnitTrust.Visible = true;
                    
                }
            }
            
        }
        protected void UnloadGV()
        {
            gvStock.Visible = false;
            gvBond.Visible = false;
            gvUnitTrust.Visible = false;
        }

      

        

        

        

        protected void CustomCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
            string securityCode = TextBoxCode.Text.ToString().Trim();
            int count = 0;
            foreach (Char c in securityCode.ToCharArray())
            {
                if (c == '0')
                {
                    count++;
                }
                else break;
            }
            int length = securityCode.Length - count;
            if (length > 4)
            {
                args.IsValid = false;

            }
            else args.IsValid = true;
            
        }

        

       

        protected void gvStock_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortedView = new DataView(dtView);
            sortedView.Sort = e.SortExpression + " " + "Asc";
            gvStock.DataSource = sortedView;
            gvStock.DataBind();
        }

        protected void gvBond_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortedView = new DataView(dtView);
            sortedView.Sort = e.SortExpression + " " + "Asc";
            gvBond.DataSource = sortedView;
            gvBond.DataBind();
        }

        protected void gvUnitTrust_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortedView = new DataView(dtView);
            sortedView.Sort = e.SortExpression + " " + "Asc";
            gvUnitTrust.DataSource = sortedView;
            gvUnitTrust.DataBind();
        }
    }
}