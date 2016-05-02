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

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class Track : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalData myExternalData = new ExternalData();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected string ConvertName(string type)
        {
            string result=String.Empty;
            if (type == "stock")
            {
                result = "Stock";
            }
            else if (type == "bond")
            {
                result = "Bond";
            }
            else
            {
                result = "UnitTrust";
            }
            return result;
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountNumber = "";
            string userName = Context.User.Identity.GetUserName();
            string AccountSql = "select [accountNumber] from [Account] where [userName]='" + userName + "'";
            decimal allBuyingAmount = 0;
            decimal allSellingAmount = 0;
            decimal allFee = 0;
            decimal allProfitLoss = 0;

            decimal stockBuyingAmount = 0;
            decimal stockSellingAmount = 0;
            decimal stockFee = 0;
            decimal stockProfitLoss = 0;

            decimal bondBuyingAmount = 0;
            decimal bondSellingAmount = 0;
            decimal bondFee = 0;
            decimal bondProfitLoss = 0;

            decimal unitTrustBuyingAmount = 0;
            decimal unitTrustSellingAmount = 0;
            decimal unitTrustFee = 0;
            decimal unitTrustProfitLoss = 0;

            DataTable dtAll = new DataTable();
            dtAll.Columns.Add("Type", typeof(string));
            dtAll.Columns.Add("buyingAmount", typeof(string));
            dtAll.Columns.Add("sellingAmount", typeof(string));
            dtAll.Columns.Add("fee", typeof(decimal));
            dtAll.Columns.Add("profitLoss", typeof(decimal));

            DataTable dTaccountNumberOfClient = myHKeInvestData.getData(AccountSql);
            if (dTaccountNumberOfClient == null) { return; } // If the DataSet is null, a SQL error occurred.
          
            foreach (DataRow row in dTaccountNumberOfClient.Rows)
            {
                
               
                    accountNumber = row["accountNumber"].ToString().Trim();
                  
                
           }
            string sql = "select * from [SecurityHolding] where [accountNumber] = '"+accountNumber+"'";
            
            decimal price = 0;
            decimal shares = 0;
            DataTable dtType = myHKeInvestData.getData(sql);
            DataTable dtTrack = new DataTable();
            dtTrack.Columns.Add("type", typeof(string));
            dtTrack.Columns.Add("code", typeof(string));
            dtTrack.Columns.Add("name", typeof(string));
            dtTrack.Columns.Add("shares", typeof(decimal));
            dtTrack.Columns.Add("buyingAmount", typeof(decimal));
            dtTrack.Columns.Add("sellingAmount", typeof(decimal));
            dtTrack.Columns.Add("fees", typeof(decimal));
            dtTrack.Columns.Add("profitLoss", typeof(decimal));
            foreach (DataRow rows in dtType.Rows)
            {
                string name = rows["name"].ToString().Trim();
                string code = rows["code"].ToString().Trim();
                string type = rows["type"].ToString().Trim();
                decimal currentAsset = 0;
                decimal fees = 0;
                string referenceNumber = String.Empty;
                string buyOrSell;
                decimal buyingAmount = 0;
                decimal sellingAmount = 0;
                decimal profitLoss = 0;
                string sqlPrice = "select * from [" + ConvertName(type) + "] where [code]='" + code + "'";
                DataTable dtPrice = myExternalData.getData(sqlPrice);
                foreach (DataRow r in dtPrice.Rows)
                {
                    price = Convert.ToDecimal(r["close"].ToString().Trim());
                }
                shares = Convert.ToDecimal(rows["shares"]);
                currentAsset = price * shares;
                string sqlFee = "select * from [Order] where [accountNumber] = '" + accountNumber + "' and [securityType]='" + ConvertName(type) + "' and [securityCode]='" + code + "'";
                DataTable dtReference = myHKeInvestData.getData(sqlFee);
                if (dtReference == null || dtReference.Rows.Count == 0)
                {
                    foreach (DataRow row in dtReference.Rows)
                    {
                        referenceNumber = row["referenceNumber"].ToString().Trim();
                        fees += Convert.ToDecimal(row["fee"].ToString().Trim());
                        buyOrSell = row["buyOrSell"].ToString().Trim();
                        string sqlAmount = "select * from [Transaction] where [referenceNumber] = '" + referenceNumber + "'";
                        DataTable dtAmount = myHKeInvestData.getData(sqlAmount);
                        foreach (DataRow r in dtAmount.Rows)
                        {
                            if (buyOrSell == "buy")
                            {
                                buyingAmount += Convert.ToDecimal(r["executePrice"]) * Convert.ToDecimal(r["executeShares"]);
                            }
                            else
                            {
                                sellingAmount += Convert.ToDecimal(r["executePrice"]) * Convert.ToDecimal(r["executeShares"]);
                            }
                        }
                    }



                }
                else continue;
                profitLoss = currentAsset + sellingAmount - buyingAmount - fees;
                dtTrack.Rows.Add(type, code, name, shares, buyingAmount, sellingAmount, fees, profitLoss);

            }
            if (ddlType.SelectedItem.ToString().Trim() == "All")
            {
                
                gvTrack.DataSource = dtTrack;
                gvTrack.DataBind();
            }
            else if (ddlType.SelectedItem.ToString().Trim() == "Stock")
            {
                DataTable dtStock = dtTrack.Clone();
                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    if (dtType.Rows[i]["type"].ToString().Trim() == "stock")
                    {
                        dtStock.ImportRow(dtType.Rows[i]);
                    }
                    
                }
                gvTrack.DataSource = dtStock;
                gvTrack.DataBind();
            }
            else if (ddlType.SelectedItem.ToString().Trim() == "Bond")
            {
                DataTable dtBond = dtTrack.Clone();
                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    if (dtType.Rows[i]["type"].ToString().Trim() == "bond")
                    {
                        dtBond.ImportRow(dtType.Rows[i]);
                    }

                }
                gvTrack.DataSource = dtBond;
                gvTrack.DataBind();
            }
            else 
            {
                DataTable dtUnitTrust = dtTrack.Clone();
                for (int i = 0; i < dtType.Rows.Count; i++)
                {
                    if (dtType.Rows[i]["type"].ToString().Trim() == "unit trust")
                    {
                        dtUnitTrust.ImportRow(dtType.Rows[i]);
                    }

                }
                gvTrack.DataSource = dtUnitTrust;
                gvTrack.DataBind();
            }

            foreach(DataRow r in dtTrack.Rows)
            {
                allBuyingAmount += Convert.ToDecimal(r["buyingAmount"]);
                allSellingAmount += Convert.ToDecimal(r["sellingAmount"]);
                allFee += Convert.ToDecimal(r["fees"]);
                allProfitLoss += Convert.ToDecimal(r["profitLoss"]);
                if (r["type"].ToString().Trim() == "stock")
                {
                    stockBuyingAmount += Convert.ToDecimal(r["buyingAmount"]);
                    stockSellingAmount += Convert.ToDecimal(r["sellingAmount"]);
                    stockFee += Convert.ToDecimal(r["fees"]);
                    stockProfitLoss += Convert.ToDecimal(r["profitLoss"]);
                }
                else if (r["type"].ToString().Trim() == "bond")
                {
                    bondBuyingAmount += Convert.ToDecimal(r["buyingAmount"]);
                    bondSellingAmount += Convert.ToDecimal(r["sellingAmount"]);
                    bondFee += Convert.ToDecimal(r["fees"]);
                    bondProfitLoss += Convert.ToDecimal(r["profitLoss"]);
                }
                else
                {
                    unitTrustBuyingAmount += Convert.ToDecimal(r["buyingAmount"]);
                    unitTrustSellingAmount += Convert.ToDecimal(r["sellingAmount"]);
                    unitTrustFee += Convert.ToDecimal(r["fees"]);
                    unitTrustProfitLoss += Convert.ToDecimal(r["profitLoss"]);
                }
                
            }
            dtAll.Rows.Add(allBuyingAmount, allSellingAmount, allFee, allProfitLoss);
            dtAll.Rows.Add(stockBuyingAmount, stockSellingAmount, stockFee, stockProfitLoss);
            dtAll.Rows.Add(bondBuyingAmount, bondSellingAmount, bondFee, bondProfitLoss);
            dtAll.Rows.Add(unitTrustBuyingAmount, unitTrustSellingAmount, unitTrustFee, unitTrustProfitLoss);

            gvAll.DataSource = dtAll;
            gvAll.DataBind();
           
            

        }

        protected void gvAll_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}