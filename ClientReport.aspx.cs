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
    public partial class ClientReport : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        ExternalData myExternalData = new ExternalData();
        private static Boolean updated = false;
        private static Boolean dateChecked = false;
        private static DataTable dtSortOrder;
        private static DataTable dtSortTransaction;
        private static DataTable dtSortHistory;
        private static DataTable dtSummaryConvert;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the available currencies to populate the DropDownList.
            //ddlCurrency.Items.Clear();
            string userName = Context.User.Identity.GetUserName();
            string AccountSql = "select [accountNumber] from [Account] where [userName]='" + userName + "'";
            DataTable dTaccountNumberOfClient = myHKeInvestData.getData(AccountSql);
            if (dTaccountNumberOfClient == null) { return; } // If the DataSet is null, a SQL error occurred.

            DataTable dtCurrency = myExternalFunctions.getCurrencyData();

            if (updated == false)
            {
                foreach (DataRow row in dtCurrency.Rows)
                {
                    ddlCurrency.Items.Add(row["currency"].ToString().Trim());
                    Session.Add(row["currency"].ToString().Trim(), row["rate"].ToString().Trim());
                }
                updated = true;
            }

            // If no result is returned by the SQL statement, then display a message.
            if (dTaccountNumberOfClient.Rows.Count == 0)
            {
                lblResultMessage.Text = "Error.";
                lblResultMessage.Visible = true;
                lblClientName.Visible = false;
                gvSecurityHolding.Visible = false;
                return;
            }

            // Show the client name(s) on the web page.

            int j = 1;
            foreach (DataRow row in dTaccountNumberOfClient.Rows)
            {
                if (j > 1)
                {
                    lblResultMessage.Text = "Error.";
                    lblResultMessage.Visible = true;
                    lblClientName.Visible = false;
                    gvSecurityHolding.Visible = false;
                    return;
                }
                else
                {
                    accountNumber = row["accountNumber"].ToString();
                    txtAccountNumber.Text = accountNumber;
                }
                j = j + 1;
            }
            

            

            string sql = "select * from [SecurityHolding] where [SecurityHolding].[accountNumber] = '" + accountNumber + "' ";
            DataTable dtSecurities = myHKeInvestData.getData(sql);
            decimal totalValue = 0;
            decimal stockValue = 0;
            decimal bondValue  = 0;
            decimal unitTrustValue = 0;
            decimal freeBalance = 0;
            string fbSql = "select [initialAccountDepositAmount] from [Account] where [accountNumber] = '" + accountNumber + "'";
            DataTable dtDeposit = myHKeInvestData.getData(fbSql);
            if (dtDeposit == null || dtDeposit.Rows.Count == 0) { }
            else
            {
                foreach (DataRow row in dtDeposit.Rows)
                {
                    freeBalance = Convert.ToDecimal(row["initialAccountDepositAmount"]);
                }

            }
            foreach (DataRow row in dtSecurities.Rows)
            {
                string securityType = row["type"].ToString().Trim();
                decimal price = myExternalFunctions.getSecuritiesPrice(securityType, row["code"].ToString());
                totalValue += price * Convert.ToDecimal(row["shares"]);
                switch (securityType)
                {
                    case "stock": 
                        stockValue += price * Convert.ToDecimal(row["shares"]);
                        break;
                    case "bond":
                        bondValue += price * Convert.ToDecimal(row["shares"]);
                        break;
                    case "unit trust":
                        unitTrustValue += price * Convert.ToDecimal(row["shares"]);
                        break;
                    default:
                        break;
                }
                totalValue += freeBalance;
            }
            DataTable dtSummary = new DataTable();
            dtSummary.Columns.Add("totalValue", typeof(decimal));
            dtSummary.Columns.Add("convertedValue", typeof(decimal));
            dtSummary.Columns.Add("freeBalance", typeof(decimal));
            dtSummary.Columns.Add("stockValue", typeof(decimal));
            dtSummary.Columns.Add("bondValue", typeof(decimal));
            dtSummary.Columns.Add("unitTrustValue", typeof(decimal));
            dtSummary.Columns.Add("lastOrderDate", typeof(DateTime));
            dtSummary.Columns.Add("lastOrderValue", typeof(string));

            //for last order information
            
            sql = "select * from [Order]";
            DataTable dtOrder = myHKeInvestData.getData(sql);
            DateTime orderTime = new DateTime(1900, 12, 1);
            decimal orderValue = 0;
            string stockOrderType = null;
            string orderSecurityType = null;
            string amount = null;
            decimal shares = 0;
            decimal limitPrice = 0;
            decimal stopPrice = 0;
            
            foreach (DataRow rows in dtOrder.Rows)
            {
                DateTime temp = (DateTime)rows["dateSubmitted"];
                if (rows["status"].ToString().Trim() != "pending")
                {
                    if (DateTime.Compare(temp, orderTime) > 0)
                    {
                        orderTime = temp;
                        stockOrderType = rows["stockOrderType"].ToString().Trim();
                        orderSecurityType = rows["securityType"].ToString().Trim();
                        if (rows["amount"] != null) amount = rows["amount"].ToString().Trim();
                        else
                        {
                            shares = Convert.ToDecimal(rows["shares"]);
                            limitPrice = Convert.ToDecimal(rows["limitPrice"]);
                            stopPrice = Convert.ToDecimal(rows["stopPrice"]);
                        }
                    }
                }              
            }

            if (orderSecurityType == "bond" || orderSecurityType == "unit trust")
            {
                orderValue = Convert.ToDecimal(amount);
            }
            else
            {
                if (stockOrderType == "limit" || stockOrderType == "stop limit")
                {
                    orderValue = shares * limitPrice;
                }
                if (stockOrderType == "stop")
                {
                    orderValue = shares * stopPrice;
                }


                
            }

            



            dtSummary.Rows.Add(totalValue, 0, freeBalance, stockValue, bondValue, unitTrustValue, orderTime, orderValue.ToString());
            dtSummaryConvert = dtSummary.Clone();
            for (int i = 0; i < dtSummary.Rows.Count; i++)
            {
                dtSummaryConvert.ImportRow(dtSummary.Rows[i]);
            }
            gvSummary.DataSource = dtSummary;
            gvSummary.DataBind();

            //List of orders
            DataTable dtOrderList=new DataTable();
            dtOrderList.Columns.Add("referenceNumber", typeof(string));
            dtOrderList.Columns.Add("buyOrSell", typeof(string));
            dtOrderList.Columns.Add("securityType", typeof(string));
            dtOrderList.Columns.Add("securityCode", typeof(string));
            dtOrderList.Columns.Add("securityName", typeof(string));
            dtOrderList.Columns.Add("dateSubmitted", typeof(DateTime));
            dtOrderList.Columns.Add("status", typeof(string));
            dtOrderList.Columns.Add("amount", typeof(decimal));
            dtOrderList.Columns.Add("shares", typeof(decimal));
            dtOrderList.Columns.Add("limitPrice", typeof(decimal));
            dtOrderList.Columns.Add("stopPrice", typeof(decimal));
            dtOrderList.Columns.Add("expiryDay", typeof(string));

            sql = "select * from [Order] ";
            string securityName = null;
            DataTable dtOrderTable = myHKeInvestData.getData(sql);
            foreach(DataRow row in dtOrderTable.Rows)
            {
                DataTable name = myExternalFunctions.getSecuritiesByCode(row["securityType"].ToString().Trim(), row["securityCode"].ToString().Trim());
                
                if (name == null || name.Rows.Count == 0)
                {

                }
                else
                {
                    
                    foreach(DataRow rows in name.Rows)
                    {
                        securityName = rows["name"].ToString().Trim();
                    }

                }
                string referenceNumber = row["referenceNumber"].ToString().Trim();
                string buyOrSell = row["buyOrSell"].ToString().Trim();
                string securityType = row["securityType"].ToString().Trim();
                string securityCode = row["securityCode"].ToString().Trim();
                DateTime dateSubmitted = Convert.ToDateTime(row["dateSubmitted"].ToString().Trim());
                string status = row["status"].ToString().Trim();
                decimal Shares = 0;
                decimal StopPrice = 0;
                decimal LimitPrice = 0;
                decimal Amount = 0;
                if (securityType == "stock")
                {
                    Shares = Convert.ToDecimal(row["shares"]);
                    if (row["stockOrderType"].ToString().Trim() == "stop")
                    {
                        StopPrice = Convert.ToDecimal(row["stopPrice"]);
                    }
                    else
                    {
                        LimitPrice = Convert.ToDecimal(row["limitPrice"]);
                    }
                    
                }
                if (securityType == "bond" || securityType == "unit trust")
                {
                    if (buyOrSell == "buy")
                    {
                        Amount = Convert.ToDecimal(row["amount"]);
                    }
                    else
                    {
                        shares = Convert.ToDecimal(row["shares"]);
                    }
                    

                }
                
                string expiryDay = row["expiryDay"].ToString().Trim();
                

                dtOrderList.Rows.Add(referenceNumber, buyOrSell, securityType, securityCode, securityName, dateSubmitted, status, Amount, Shares, LimitPrice, StopPrice, expiryDay);

                dtSortOrder = dtOrderList.Clone();
                for (int i = 0; i < dtOrderList.Rows.Count; i++)
                {
                    dtSortOrder.ImportRow(dtOrderList.Rows[i]);
                }
                //
                DataView sortedView = new DataView(dtSortOrder);
                sortedView.Sort = "dateSubmitted" + " " + "Asc";
                gvOrder.DataSource = sortedView;

                gvOrder.DataBind();
            }
            

        }

        

        string accountNumber = "";


        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset visbility of controls and initialize values.
            //BUG FIXED HERE: There was a rea line under 'lblResultMessage', which means no such variable specified: typo in the aspx page!
            lblResultMessage.Visible = false;
            ddlCurrency.Visible = false;
            gvSecurityHolding.Visible = false;
            ddlCurrency.SelectedIndex = 0;
            string sql = "";

            // *******************************************************************
            // TODO: Set the account number and security type from the web page. *
            // *******************************************************************



            // Set the account number from a web form control!
            string securityType = ddlSecurityType.SelectedValue.Trim(); // Set the securityType from a web form control!

            // Check if an account number has been specified.
            if (accountNumber == "")
            {
                lblResultMessage.Text = "Please specify an account number.";
                lblResultMessage.Visible = true;
                ddlSecurityType.SelectedIndex = 0;
                return;
            }

            // No action when the first item in the DropDownList is selected.
            if (securityType == "0") { return; }

            // *****************************************************************************************
            // TODO: Construct the SQL statement to retrieve the first and last name of the client(s). *
            // *****************************************************************************************
            sql = "select [firstName], [lastName] from [ClientTemp] where [accountNumber]='" + accountNumber + "'"; // Complete the SQL statement.
            //Explanation for this task:
            //1. An account can be held by many clients.
            //2. An account can invest in multiple ways.
            //3. An account can invest in stocks from multiple companies.
            //4. No relationship between client and investment(can not specify which investment is done by which of clients holding one account."

            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                lblResultMessage.Text = "No such account number.";
                lblResultMessage.Visible = true;
                lblClientName.Visible = false;
                gvSecurityHolding.Visible = false;
                return;
            }

            // Show the client name(s) on the web page.
            string clientName = "Client(s): ";
            int i = 1;
            foreach (DataRow row in dtClient.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (dtClient.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i = i + 1;
            }
            lblClientName.Text = clientName;
            lblClientName.Visible = true;

            // *****************************************************************************************************************************
            // TODO: Construct the SQL select statement to get the code, name, shares and base of the security holdings of a specific type *
            //       in an account. The select statement should also return three additonal columns -- price, value and convertedValue --  *
            //       whose values are not actually in the database, but are set to the constant 0.00 by the select statement. (HINT: see   *
            //       http://stackoverflow.com/questions/2504163/include-in-select-a-column-that-isnt-actually-in-the-database.)            *   
            // *****************************************************************************************************************************
            sql = "select [code], [name], [shares], [base], '0.00' AS [price], '0.00' AS [value], '0.00' AS [convertedValue] from [SecurityHolding] where [SecurityHolding].[accountNumber]='" + accountNumber + "' and [type]='" + securityType + "'"; // Complete the SQL statement.

            DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
            if (dtSecurityHolding == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned, then display a message that the account does not hold this type of security.
            if (dtSecurityHolding.Rows.Count == 0)
            {
                lblResultMessage.Text = "No " + securityType + "s held in this account.";
                lblResultMessage.Visible = true;
                gvSecurityHolding.Visible = false;
                return;
            }

            // For each security in the result, get its current price from an external system, calculate the total value
            // of the security and change the current price and total value columns of the security in the result.
            int dtRow = 0;
            foreach (DataRow row in dtSecurityHolding.Rows)
            {
                string securityCode = row["code"].ToString();
                decimal shares = Convert.ToDecimal(row["shares"]);
                decimal price = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
                decimal value = Math.Round(shares * price - (decimal).005, 2);
                dtSecurityHolding.Rows[dtRow]["price"] = price;
                dtSecurityHolding.Rows[dtRow]["value"] = value;
                dtRow = dtRow + 1;
            }

            // Set the initial sort expression and sort direction for sorting the GridView in ViewState.
            ViewState["SortExpression"] = "name";
            ViewState["SortDirection"] = "ASC";

            // Bind the GridView to the DataTable.
            gvSecurityHolding.DataSource = dtSecurityHolding;
            gvSecurityHolding.DataBind();

            // Set the visibility of controls and GridView data.
            gvSecurityHolding.Visible = true;
            ddlCurrency.Visible = true;
            gvSecurityHolding.Columns[myHKeInvestCode.getColumnIndexByName(gvSecurityHolding, "convertedValue")].Visible = false;
        }

        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the index value of the convertedValue column in the GridView using the helper method "getColumnIndexByName".
            int convertedValueIndex = myHKeInvestCode.getColumnIndexByName(gvSummary, "convertedValue");

            // Get the currency to convert to from the ddlCurrency dropdownlist.
            // Hide the converted currency column if no currency is selected.
            string toCurrency = ddlCurrency.SelectedValue;
            if (toCurrency == "0")
            {
                gvSummary.Columns[convertedValueIndex].Visible = false;
                return;
            }
            string toCurrencyRate = myExternalFunctions.getCurrencyRate(toCurrency).ToString();
            // Make the convertedValue column visible and create a DataTable from the GridView.
            // Since a GridView cannot be updated directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            gvSummary.Columns[convertedValueIndex].Visible = true;
            

            // ***********************************************************************************************************
            // TODO: For each row in the DataTable, get the base currency of the security, convert the current value to  *
            //       the selected currency and assign the converted value to the convertedValue column in the DataTable. *
            // ***********************************************************************************************************
            int dtRow = 0;
            foreach (DataRow row in dtSummaryConvert.Rows)
            {
                // Add your code here!
                string base_currency = "HKD";
                string baseCurrencyRate = myExternalFunctions.getCurrencyRate(base_currency).ToString();
                decimal valueToConvert = Convert.ToDecimal(row["totalValue"]);
                decimal convertedV = myHKeInvestCode.convertCurrency(base_currency, baseCurrencyRate, toCurrency, toCurrencyRate, valueToConvert);
                dtSummaryConvert.Rows[dtRow]["convertedValue"] = convertedV;
                dtRow = dtRow + 1;
            }

            // Change the header text of the convertedValue column to indicate the currency. 
            gvSecurityHolding.Columns[convertedValueIndex].HeaderText = "Value in " + toCurrency;

            // Bind the DataTable to the GridView.
            gvSummary.DataSource = dtSummaryConvert;
            gvSummary.DataBind();
            string type = ddlSecurityType.SelectedValue;
            if (type != "0")
            {
                // Get the index value of the convertedValue column in the GridView using the helper method "getColumnIndexByName".
                convertedValueIndex = myHKeInvestCode.getColumnIndexByName(gvSecurityHolding, "convertedValue");

                // Get the currency to convert to from the ddlCurrency dropdownlist.
                // Hide the converted currency column if no currency is selected.
                
                if (toCurrency == "0")
                {
                    gvSecurityHolding.Columns[convertedValueIndex].Visible = false;
                    return;
                }
                
                // Make the convertedValue column visible and create a DataTable from the GridView.
                // Since a GridView cannot be updated directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
                gvSecurityHolding.Columns[convertedValueIndex].Visible = true;
                DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(gvSecurityHolding);

                // ***********************************************************************************************************
                // TODO: For each row in the DataTable, get the base currency of the security, convert the current value to  *
                //       the selected currency and assign the converted value to the convertedValue column in the DataTable. *
                // ***********************************************************************************************************
                dtRow = 0;
                foreach (DataRow row in dtSecurityHolding.Rows)
                {
                    // Add your code here!
                    string base_currency = row["base"].ToString();
                    string baseCurrencyRate = myExternalFunctions.getCurrencyRate(base_currency).ToString();
                    decimal valueToConvert = Convert.ToDecimal(row["shares"]) * Convert.ToDecimal(row["price"]);
                    decimal convertedV = myHKeInvestCode.convertCurrency(base_currency, baseCurrencyRate, toCurrency, toCurrencyRate, valueToConvert);
                    dtSecurityHolding.Rows[dtRow]["convertedValue"] = convertedV;
                    dtRow = dtRow + 1;
                }

                // Change the header text of the convertedValue column to indicate the currency. 
                gvSecurityHolding.Columns[convertedValueIndex].HeaderText = "Value in " + toCurrency;

                // Bind the DataTable to the GridView.
                gvSecurityHolding.DataSource = dtSecurityHolding;
                gvSecurityHolding.DataBind();
            }



            
        }

        protected void gvSecurityHolding_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Since a GridView cannot be sorted directly, it is first loaded into a DataTable using the helper method 'unloadGridView'.
            // Create a DataTable from the GridView.
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(gvSecurityHolding);

            // Set the sort expression in ViewState for correct toggling of sort direction,
            // Sort the DataTable and bind it to the GridView.
            string sortExpression = e.SortExpression.ToLower();
            ViewState["SortExpression"] = sortExpression;
            dtSecurityHolding.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtSecurityHolding.AcceptChanges();

            // Bind the DataTable to the GridView.
            gvSecurityHolding.DataSource = dtSecurityHolding.DefaultView;
            gvSecurityHolding.DataBind();
        }

        protected void gvOrder_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortedView = new DataView(dtSortOrder);
            sortedView.Sort = e.SortExpression + " " + "Asc";
            gvOrder.DataSource = sortedView;
            gvOrder.DataBind();
        }

        protected void gvHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView sortedView = new DataView(dtSortHistory);
            sortedView.Sort = e.SortExpression + " " + "Asc";
            gvHistory.DataSource = sortedView;
            gvHistory.DataBind();
        }

        



        protected void cvDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime StartDate = startDate.SelectedDate;
            DateTime EndDate = endDate.SelectedDate;
            DateTime CurrentDate = DateTime.Today;
            if (DateTime.Compare(StartDate, CurrentDate) > 0)
            {
                args.IsValid = false;
                dateChecked = false;
                cvDate.ErrorMessage = "Start date must be earlier than current date";
            }
            if (DateTime.Compare(StartDate, EndDate) > 0)
            {
                args.IsValid = false;
                dateChecked = false;
                cvDate.ErrorMessage = "Start date must be earlier than end date";
            }
        }

        protected void searchHistory_Click(object sender, EventArgs e)
        {
            if (IsValid == false)
            {
                return;
            }

            DateTime StartDate = startDate.SelectedDate;
            DateTime EndDate = endDate.SelectedDate;
            DataTable dtOrderList = new DataTable();
            dtOrderList.Columns.Add("referenceNumber", typeof(string));
            dtOrderList.Columns.Add("buyOrSell", typeof(string));
            dtOrderList.Columns.Add("securityType", typeof(string));
            dtOrderList.Columns.Add("securityCode", typeof(string));
            dtOrderList.Columns.Add("securityName", typeof(string));
            dtOrderList.Columns.Add("dateSubmitted", typeof(string));
            dtOrderList.Columns.Add("status", typeof(string));
            dtOrderList.Columns.Add("shares", typeof(decimal));
            dtOrderList.Columns.Add("executedAmount", typeof(decimal));
            dtOrderList.Columns.Add("fee", typeof(decimal));
            

            string sql = "select * from [Order] ";
            string securityName = null;
            DataTable dtOrders = myHKeInvestData.getData(sql);
            if (dtOrders == null || dtOrders.Rows.Count == 0)
            {

            }
            else
            {
                foreach(DataRow row in dtOrders.Rows)
                {
                    DateTime thisStartDate = Convert.ToDateTime(row["dateSubmitted"]);
                    if (dateChecked==false||(DateTime.Compare(StartDate, thisStartDate) < 0 && DateTime.Compare(EndDate, thisStartDate)>0))
                    {
                        DataTable name = myExternalFunctions.getSecuritiesByCode(row["securityType"].ToString().Trim(), row["securityCode"].ToString().Trim());

                        if (name == null || name.Rows.Count == 0)
                        {

                        }
                        else
                        {

                            foreach (DataRow rows in name.Rows)
                            {
                                securityName = rows["name"].ToString().Trim();
                            }

                        }


                        string referenceNumber = row["referenceNumber"].ToString().Trim();
                        string buyOrSell = row["buyOrSell"].ToString().Trim();
                        string securityType = row["securityType"].ToString().Trim();
                        string securityCode = row["securityCode"].ToString().Trim();
                        string dateSubmitted = row["dateSubmitted"].ToString().Trim();
                        string status = row["status"].ToString().Trim();
                        decimal eAmount = 0;
                        decimal shares = 0;
                        decimal fee = 0;
                        //decimal fee = Convert.ToDecimal(row["fee"]);/////////////////////////////////////////////////////////////////////////////////////////////////////

                        string sqlOrderTransaction = "select * from [Transaction] where [referenceNumber] = '"+referenceNumber+"'";
                       
                        DataTable dtOrderTransaction = myHKeInvestData.getData(sqlOrderTransaction);
                        if (dtOrderTransaction == null || dtOrderTransaction.Rows.Count == 0)
                        {

                        }
                        else
                        {
                            foreach (DataRow rows in dtOrderTransaction.Rows)
                            {
                                if (rows["executeShares"] == null)
                                {
                                    eAmount = 0;
                                    shares = Convert.ToDecimal(rows["executeShares"]);
                                }
                                else
                                {
                                    eAmount += Convert.ToDecimal(rows["executeShares"]) * Convert.ToDecimal(rows["executePrice"]);
                                    shares = Convert.ToDecimal(rows["executeShares"]);
                                }
                                    
                                    
                                

                                
                            }
                            
                        }
                        dtOrderList.Rows.Add(referenceNumber, buyOrSell, securityType, securityCode, securityName, dateSubmitted, status, shares, eAmount, fee);


                    }
                }






                
                

            }

            dtSortHistory = dtOrderList.Clone();
            for (int i = 0; i < dtOrderList.Rows.Count; i++)
            {
                dtSortHistory.ImportRow(dtOrderList.Rows[i]);
            }
            //
            DataView sortedView = new DataView(dtSortHistory);
            sortedView.Sort = "securityType" + " " + "Asc";
            gvHistory.DataSource = sortedView;

            gvHistory.DataBind();




            /////
            DataTable dtGVTransaction = new DataTable();
            dtGVTransaction.Columns.Add("transactionNumber", typeof(string));
            dtGVTransaction.Columns.Add("executedDate", typeof(string));
            dtGVTransaction.Columns.Add("executedShares", typeof(string));
            dtGVTransaction.Columns.Add("price", typeof(string));
            decimal Shares = 0;
            DateTime executedDate;
            decimal price = 0;
            string transactionNumber;
            string sqlTransaction = "select * from [Transaction] ";
            DataTable dtTransaction = myHKeInvestData.getData(sqlTransaction);
            if (dtTransaction == null || dtTransaction.Rows.Count == 0)
            {

            }
            else
            {
                foreach (DataRow rows in dtTransaction.Rows)
                {
                    DateTime thisStartDate = Convert.ToDateTime(rows["executeDate"]);
                    if (dateChecked == false || (DateTime.Compare(StartDate, thisStartDate) < 0 && DateTime.Compare(EndDate, thisStartDate) > 0))
                    {
                        Shares = Convert.ToDecimal(rows["executeShares"]);
                        executedDate = Convert.ToDateTime(rows["executeDate"]);
                        price = Convert.ToDecimal(rows["executePrice"]);
                        transactionNumber = rows["transactionNumber"].ToString().Trim();
                        dtGVTransaction.Rows.Add(transactionNumber, executedDate, Shares, price);
                    }
                    
                }
                dtSortTransaction = dtGVTransaction.Clone();
                for (int i = 0; i < dtGVTransaction.Rows.Count; i++)
                {
                    dtSortTransaction.ImportRow(dtGVTransaction.Rows[i]);
                }
                //
                DataView sortedView1 = new DataView(dtSortTransaction);
                sortedView1.Sort = "executedDate" + " " + "Asc";
                gvTransaction.DataSource = sortedView1;

                gvTransaction.DataBind();

            }
        }

        protected void cbCalendar_CheckedChanged(object sender, EventArgs e)
        {
            //required validation missing!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (cbCalendar.Checked == true)
            {
                startDate.Visible = true;
                endDate.Visible = true;
                cvDate.Enabled = true;
                dateChecked = true;
                startDate.SelectedDate = DateTime.Today;
                endDate.SelectedDate = DateTime.Today;
            }
            else
            {
                startDate.Visible = false;
                endDate.Visible = false;
                cvDate.Enabled = false;
                dateChecked = false;
                
            }
            
        }

        
    }
}