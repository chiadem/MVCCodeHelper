using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewModelGenerator
{
    public partial class ViewModelGen : Form
    {
        public ViewModelGen()
        {
            InitializeComponent();
        }

        private void ViewModelGen_Load(object sender, EventArgs e)
        {
  
        }

        private void TestB_Click(object sender, EventArgs e)
        {
            SqlConnection myConnection = new SqlConnection("user id=" + UserTB.Text + ";" +
                                      "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                      "Trusted_Connection=false;" +
                                      "database=" + DatabaseCB.SelectedText + "; " +
                                      "connection timeout=30");
            try
            {
                myConnection.Open();
                MessageBox.Show("Success!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed!");
            }
            finally
            {
                myConnection.Close();
            }

        }

        private void DatabaseCB_Click(object sender, EventArgs e)
        {
            DatabaseCB.Items.Clear();

            List<String> databases = new List<String>();

            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();

            connection.DataSource = ServeTB.Text;
            connection.UserID = UserTB.Text;
            connection.Password = PassTB.Text;
            connection.IntegratedSecurity = false;

            String strConn = connection.ToString();

            //create connection
            SqlConnection sqlConn = new SqlConnection(strConn);

            //open connection
            sqlConn.Open();

            //get databases
            DataTable tblDatabases = sqlConn.GetSchema("Databases");

            //close connection
            sqlConn.Close();

            //add to list
            foreach (DataRow row in tblDatabases.Rows)
            {
                String strDatabaseName = row["database_name"].ToString();

                databases.Add(strDatabaseName);

                DatabaseCB.Items.Add(strDatabaseName);
            }
        }

        private void TableCB_Click(object sender, EventArgs e)
        {
            TableCB.Items.Clear();
            SqlConnection conn = new SqlConnection("user id=" + UserTB.Text + ";" +
                                     "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                     "Trusted_Connection=false;" +
                                     "database=" + DatabaseCB.SelectedText + "; " +
                                     "connection timeout=30");
            conn.Open();
            List<string> tables = new List<string>();

            DataTable dt = conn.GetSchema("Tables");
            foreach (DataRow row in dt.Rows)
            {
                if (((string)row[3]).Equals("BASE TABLE") && ((string)row[1]).Equals("dbo"))
                {
                    string tablename = (string)row[2];
                    tables.Add(tablename);
                }
            }
            tables.Sort();
            foreach (var item in tables)
            {
                TableCB.Items.Add(item);
            }
            conn.Close();

        }

        private void GenB_Click(object sender, EventArgs e)
        {

            string viewModelName = ViewModelNameTB.Text;
            string tableName = TableCB.SelectedItem.ToString();
            string tableNameplural = ToPlural(tableName);

            string[,] columns = new string[100, 4];
            string n = "\n";

            string code = "public  class " + viewModelName + n + "{" + n + n;

            string toEntity = @"public " + tableName + @" ToEntity()" + n + @"
            {" + n + @"
            var entity = new " + tableName + @"" + n + @"
            {" + n;
            string toEntityP = "public " + tableName + " ToEntity(" + tableName + " entity)" + "{" + n;
            string toModel = " public static " + viewModelName + " ToModel(" + tableName + " entity)" + "{" + n
                + @" var model = new " + viewModelName + "();"
                + n + " if (entity != null)" + n + "{" + n;




            SqlConnection cn = new SqlConnection("user id=" + UserTB.Text + ";" +
                                    "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                    "Trusted_Connection=false;" +
                                    "database=" + DatabaseCB.SelectedText + "; " +
                                    "connection timeout=30");

            SqlCommand cmd = new SqlCommand();
            DataTable schemaTable;
            SqlDataReader myReader;

            cn.Open();

            //Retrieve records from the Employees table into a DataReader.
            cmd.Connection = cn;
            cmd.CommandText = "SELECT top 1 * FROM " + tableName;
            myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);

            //Retrieve column schema into a DataTable.
            schemaTable = myReader.GetSchemaTable();
            int i = 0;
            //For each field in the table...
            foreach (DataRow myField in schemaTable.Rows)
            {


                columns[i, 0] = myField["ColumnName"].ToString();
                columns[i, 1] = myField["DataType"].ToString();
                columns[i, 2] = myField["ColumnSize"].ToString();
                columns[i, 3] = myField["AllowDBNull"].ToString();

                string columnName = myField["ColumnName"].ToString();

                if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                    (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                    (columnName.Equals("createdBy") && !createdBy.Checked) ||
                    (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                    (columnName.Equals("rowVersion") && !rowVersion.Checked)
                    )
                {
                    continue;
                }


                string columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                columnNameCamel = columnNameCamel.Replace("ID", "Id");

                string displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                string dataType = myField["DataType"].ToString().Replace("System.", "").Replace("Int16", "short").Replace("Int32", "int").Replace("Int", "int").Replace("String", "string").Replace("Boolean", "bool");
                int size = Convert.ToInt32(myField["ColumnSize"]);
                bool isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                bool IsKey = Convert.ToBoolean(myField["IsKey"]);
                if (isNullable)
                {
                    if (!dataType.Equals("string"))
                    {
                        dataType = dataType + "?";
                    }
                }
                else if (RequiredAnn.Checked && !IsKey && !(columnName.Equals("createdDate") ||
                                                            columnName.Equals("modifiedDate") ||
                                                            columnName.Equals("createdBy") ||
                                                            columnName.Equals("modifiedBy") ||
                                                            columnName.Equals("rowVersion")))
                {
                    code = code + @"[Required(ErrorMessage = ""{0} value is empty"")]" + n;
                }

                if (IsKey & HiddenInputAnn.Checked)
                {
                    code = code + "[HiddenInput(DisplayValue = false)]" + n;
                }
                else if (DisplayAnn.Checked)
                {
                    code = code + @"[Display(Name = """ + displayName + @""")]" + n;
                }
                if (dataType.Equals("string") & StringLengthAnn.Checked)
                {
                    code = code + @"[StringLength(" + size + @", ErrorMessage = ""The length of {0} exceeds the limit of {1} characters!"")]" + n;
                }

                code = code + "public " + dataType + " " + columnNameCamel + " { get; set; }" + n + n;

                toEntity = toEntity + columnName + " = " + columnNameCamel + "," + n;
                toEntityP = toEntityP + "entity." + columnName + " = " + columnNameCamel + ";" + n;
                toModel = toModel + "model." + columnNameCamel + " = entity." + columnName + ";" + n;
                i++;
            }

            //Always close the DataReader and connection.
            myReader.Close();

            cn.Close();

            PK(tableName);

            foreach (DataGridViewRow itemm in PKGV.Rows)
            {
                if (itemm.Cells[0].Value == null)
                {
                    continue;
                }

                string FKTableName = "";
                string FKTableNamePlurar = "";
                string FKTableNameVM = "";

                if (itemm.Cells[6].Value != null)
                {
                    FKTableName = itemm.Cells[6].Value.ToString();
                    FKTableNamePlurar = FKTableName + "s";
                    if (FKTableName.Last().Equals('y'))
                    {
                        //agency agenc ies
                        FKTableNamePlurar = FKTableName.Remove(FKTableName.Length - 1) + "ies";
                    }
                    FKTableNameVM = FKTableName + "ViewModel";
                    code = code + n + "public List<" + FKTableNameVM + "> " + FKTableNamePlurar + " { get; set; }" + n;

                }
            }



            code = code + toEntity.TrimEnd(',') + n + @"};" + n + @"return entity;" + n + @"}" + n + n;
            code = code + toEntityP + n + @"return entity;" + n + @"}" + n + n;
            code = code + toModel + n + "}" + n + " return model;" + n + "}" + n + n;
            code = code + n + "}" + n;
            CodeText.Text = code;
            CodeText.Focus();
            Clipboard.SetText(CodeText.Text);
        }

        private void TableCB_SelectedValueChanged(object sender, EventArgs e)
        {
            ViewModelNameTB.Text = TableCB.SelectedItem + "ViewModel";

        }

        private void RepoBtn_Click(object sender, EventArgs e)
        {


            SqlConnection cn = new SqlConnection("user id=" + UserTB.Text + ";" +
                                    "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                    "Trusted_Connection=false;" +
                                    "database=" + DatabaseCB.SelectedText + "; " +
                                    "connection timeout=30");

            SqlCommand cmd = new SqlCommand();
            DataTable schemaTable;
            SqlDataReader myReader;

            cn.Open();
            string code = "";
            foreach (DataGridViewRow item in TableGrid.Rows)
            {
                if (item.Cells[0].Value == null)
                {
                    continue;
                }

                string tableName = item.Cells[0].Value.ToString();
                string regionName = item.Cells[1].Value.ToString();
                string viewModelName = tableName + "ViewModel";

                string tableNameplural = ToPlural(tableName);

                //Retrieve records from the Employees table into a DataReader.
                cmd.Connection = cn;
                cmd.CommandText = "SELECT top 1 * FROM " + tableName;
                myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //Retrieve column schema into a DataTable.
                schemaTable = myReader.GetSchemaTable();

                string[,] columns = new string[100, 4];
                string n = "\n";

                code = code + " #region " + regionName + n + n;

                string primaryKey = "";
                string primaryKeyCamel = "";
                string parameterPK = "";

                //string FK = "";
                //string FKIdFixed = ""; string FKCamel = "";


                //if (!string.IsNullOrEmpty(item.Cells[1].Value.ToString()))
                //{
                //    FKIdFixed = FK.Replace("ID", "Id");
                //    FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                //}

                int i = 0;
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {

                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    string columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                        (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                        (columnName.Equals("createdBy") && !createdBy.Checked) ||
                        (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                        (columnName.Equals("rowVersion") && !rowVersion.Checked)
                        )
                    {
                        continue;
                    }


                    string columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                    columnNameCamel = columnNameCamel.Replace("ID", "Id");

                    string displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                    string dataType = myField["DataType"].ToString().Replace("System.", "").Replace("Int16", "short").Replace("Int32", "int").Replace("Int", "int").Replace("String", "string").Replace("Boolean", "bool");
                    int size = Convert.ToInt32(myField["ColumnSize"]);
                    bool isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    bool IsKey = Convert.ToBoolean(myField["IsKey"]);

                    if (IsKey)
                    {
                        primaryKey = columnName;
                        parameterPK = columnName.Replace("ID", "Id");
                        primaryKeyCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                        primaryKeyCamel = columnNameCamel.Replace("ID", "Id");
                    }

                    i++;
                }
                #region GetTableName

                code = code + "public async Task<" + viewModelName + "> Get" + tableName + " (int " + parameterPK + ")" + n + "{";
                code = code + "var result = await db." + tableNameplural + ".SingleOrDefaultAsync(e => e." + primaryKey + " == " +
                       parameterPK + ");" + n;
                code = code + @"return result != null ? " + viewModelName + ".ToModel(result) : null;" + n + "}" + n + n;

                #endregion

                #region GetTableNameList

                code = code + @"
                    public List<" + viewModelName + @"> Get" + tableNameplural + @"List()
                    {
                        return db." + tableNameplural + @".Select(" + viewModelName + @".ToModel).ToList();
                    }";
                #endregion

                #region GetTableNameByForeignKey
                FK(tableName);
                PK(tableName);
                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    string FKColumn = "";
                    string FKIdFixed = ""; string FKCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        FKColumn = item_.Cells[3].Value.ToString();
                        FKIdFixed = FKColumn.Replace("ID", "Id");
                        FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                    }

                    code = code + "public List<" + viewModelName + "> Get" + tableNameplural + "By" + FKCamel + "(int " +
                           FKIdFixed + ")" + n + "{";
                    code = code + " var result = db." + tableNameplural + ".Where(a => a." + FKColumn + " == " + FKIdFixed +
                           ");" + n;
                    code = code + @" return result.Select(" + viewModelName + ".ToModel).ToList();" + n + "}" + n + n;
                }

                #endregion



                #region AddTableNameAsync

                code = code + @"

            public async Task<bool> Add" + tableName + @"Async(" + viewModelName + @" model)
            {
            try
            {
                db." + tableNameplural + @".Add(model.ToEntity());
                await db.SaveChangesAsync();
                Log.Info(""" + tableName + @" added with id  "" + model." + primaryKeyCamel + @");
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorException(""Failed to add " + tableName + @" with id  "" + model." + primaryKeyCamel + @", ex);
                return false;
            }" + n + "}" + n + n;

                #endregion

                #region UpdateTableNameAsync

                code = code + @" public async Task<bool> Update" + tableName + @"Async(" + viewModelName + @" model)
        {
            try
            {
                var entity = await db." + tableNameplural + @".FirstOrDefaultAsync(e => e." + primaryKey + @" == model." + primaryKeyCamel + @");
                entity = model.ToEntity(entity);
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Log.Info(""" + tableName + @" updated with id "" + model." + primaryKeyCamel + @");
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorException(""Failed to update " + tableName + @" with id "" + model." + primaryKeyCamel + @", ex);
                return false;
            }
        }" + n + n;

                #endregion



                #region DeleteTableNameAsync

                code = code + @"public async Task<bool> Delete" + tableName + @"Async(int id)
        {
            try
            {
                var result = db." + tableNameplural + @".FirstOrDefault(e => e." + primaryKey + @" == id);
                if (result != null)
                {
                    db." + tableNameplural + @".Remove(result);
                    await db.SaveChangesAsync();

                    Log.Info(""" + tableName + @" deleted with id "" + id);
                    return true;
                }
                Log.Info(""" + tableName + @" not found with id "" + id);
                return false;
                }
                catch (Exception ex)
                {
                    Log.ErrorException(""Failed to delete " + tableName + @" with id "" + id, ex);
                    return false;
                }
              }
            " + n + n;

                #endregion

                code = code + "#endregion" + n + n;
                myReader.Close();
            }
            //Always close the DataReader and connection.
            cn.Close();
            RepoCodeText.Text = code;
            RepoCodeText.Focus();
            RepoCodeText.SelectAll();
            ActionGen_Click();
        }

        private void TableNameRepo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TableNameRepo.SelectedItem != null)
            {
                RegionTB.Text = TableNameRepo.SelectedItem.ToString();
            }
        }

        private void TableCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TableNameRepo_Click(object sender, EventArgs e)
        {
        }

        private void QueueBtn_Click(object sender, EventArgs e)
        {

            if (TableGrid.Rows.Count > 1)
            {

                if (!TableGrid.Rows
                  .Cast<DataGridViewRow>()
                  .Any(r => r.Cells["CTableName"].Value != null && r.Cells["CTableName"].Value.ToString().Equals(TableNameRepo.Text)))
                    TableGrid.Rows.Add(TableNameRepo.Text, RegionTB.Text, RepoNameTB.Text, ControllerNameTB.Text, isPartial.Checked);

            }
            else
                TableGrid.Rows.Add(TableNameRepo.Text, RegionTB.Text, RepoNameTB.Text, ControllerNameTB.Text,isPartial.Checked);
            //foreach (var item in TableNameRepo.Items)
            //{
            //    TableGrid.Rows.Add(item.ToString(), FKTB.Text, item.ToString());
            //}


        }

        private void FK(string tableName)
        {
            SqlConnection sqlConnection1 = new SqlConnection("user id=" + UserTB.Text + ";" +
                                                             "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                                             "Trusted_Connection=false;" +
                                                             "database=" + DatabaseCB.SelectedText + "; " +
                                                             "connection timeout=30");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            DataSet ds = new DataSet("TimeRanges");

            cmd.CommandText = "sp_fkeys";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fktable_name", SqlDbType.VarChar, 100).Value = tableName;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);

            FKGV.AutoGenerateColumns = true;
            FKGV.DataSource = ds;
            FKGV.DataMember = ds.Tables[0].TableName;
            // Data is accessible through the DataReader object here.

            sqlConnection1.Close();
        }

        private void PK(string tableName)
        {
            SqlConnection sqlConnection1 = new SqlConnection("user id=" + UserTB.Text + ";" +
             "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
             "Trusted_Connection=false;" +
             "database=" + DatabaseCB.SelectedText + "; " +
             "connection timeout=30");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            DataSet ds = new DataSet("TimeRanges");

            cmd.CommandText = "sp_fkeys";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pktable_name", SqlDbType.VarChar, 100).Value = tableName;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();


            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);

            PKGV.AutoGenerateColumns = true;
            PKGV.DataSource = ds;
            PKGV.DataMember = ds.Tables[0].TableName;
            // Data is accessible through the DataReader object here.

            sqlConnection1.Close();
        }


        public void ActionGen_Click()
        {
            SqlConnection cn = new SqlConnection("user id=" + UserTB.Text + ";" +
                                    "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                    "Trusted_Connection=false;" +
                                    "database=" + DatabaseCB.SelectedText + "; " +
                                    "connection timeout=30");

            SqlCommand cmd = new SqlCommand();
            DataTable schemaTable;
            SqlDataReader myReader;

            cn.Open();
            string code = "";
            foreach (DataGridViewRow item in TableGrid.Rows)
            {
                if (item.Cells[0].Value == null)
                {
                    continue;
                }

                string tableName = item.Cells[0].Value.ToString();
                string repoName = item.Cells[2].Value.ToString();
                string controllerName = item.Cells[3].Value.ToString();
                bool isPartial = Convert.ToBoolean(item.Cells[4].Value);
                string viewModelName = tableName + "ViewModel";

                string tableNameplural = ToPlural(tableName);
                //Retrieve records from the Employees table into a DataReader.
                cmd.Connection = cn;
                cmd.CommandText = "SELECT top 1 * FROM " + tableName;
                myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //Retrieve column schema into a DataTable.
                schemaTable = myReader.GetSchemaTable();

                string[,] columns = new string[100, 4];
                string n = "\n";

                code = code + " #region " + tableName + n + n;

                string primaryKey = "";
                string primaryKeyCamel = "";
                string parameterPK = "";

                //string FK = "";
                //string FKIdFixed = ""; string FKCamel = "";


                //if (!string.IsNullOrEmpty(item.Cells[1].Value.ToString()))
                //{
                //    FKIdFixed = FK.Replace("ID", "Id");
                //    FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                //}

                int i = 0;
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {

                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    string columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                        (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                        (columnName.Equals("createdBy") && !createdBy.Checked) ||
                        (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                        (columnName.Equals("rowVersion") && !rowVersion.Checked)
                        )
                    {
                        continue;
                    }


                    string columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                    columnNameCamel = columnNameCamel.Replace("ID", "Id");

                    string displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                    string dataType = myField["DataType"].ToString().Replace("System.", "").Replace("Int16", "short").Replace("Int32", "int").Replace("Int", "int").Replace("String", "string").Replace("Boolean", "bool");
                    int size = Convert.ToInt32(myField["ColumnSize"]);
                    bool isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    bool IsKey = Convert.ToBoolean(myField["IsKey"]);

                    if (IsKey)
                    {
                        primaryKey = columnName;
                        parameterPK = columnName.Replace("ID", "Id");
                        primaryKeyCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                        primaryKeyCamel = columnNameCamel.Replace("ID", "Id");
                    }

                    i++;
                }
                if (!isPartial)
                {

                    #region TableNameDetails

                    string actionName = tableName + @"Details";
                    code = code + @"
                // GET: /" + controllerName + "/" + actionName + @"/5
                public async Task<ActionResult> " + actionName + @"(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                        Log.Warn(""" + tableName + @" is not found id=: "" + id);
                        return HttpNotFound();
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return View(model);
                }" + n + n;

                    #endregion

                    #region AddTableName

                    code = code + @"
                // GET: /""" + controllerName + @"/Add" + tableName + @"
                public ActionResult Add" + tableName + @"()
                {
                    var model = new " + viewModelName + @"();
                    return View(model);
                }

                // POST:  /""" + controllerName + @"/Add" + tableName + @"
                    [HttpPost]
                public async Task<ActionResult> Add" + tableName + @"(" + viewModelName + @" model)
                {
                    if (ModelState.IsValid)
                    {
                        if (await " + repoName + @".Add" + tableName + @"Async(model))
                        {
                            ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel + @" + "" added"";
                            Log.Info(string.Format(""New " + tableName + @" {0} added"", model." + primaryKeyCamel + @"));
                            return RedirectToAction(""List" + tableName + @""");
                        }
                        ModelState.AddModelError(""" + tableName + @"AddingError"", ""There was a problem adding the " + tableName + @" "" + model." + primaryKeyCamel + @");
                        return View(model);
                    }
                    return View(model);
                 }" + n + n;

                    #endregion

                    #region EditTableName

                    code = code + @" 
                // GET: /" + controllerName + @"/Edit" + tableName + @"
                public async Task<ActionResult> Edit" + tableName + @"(int? id)
                {
                    var model = new " + viewModelName + @"();
                    //get the " + tableName + @" from the DB
                    if (id.HasValue)
                    {
                        model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    }
                    return View(""Edit" + tableName + @""", model);
                }


                    // POST: /" + controllerName + @"/Edit" + tableName + @"
                    [HttpPost]
                    public async Task<ActionResult> Edit" + tableName + @"(" + viewModelName + @" model)
                    {
                        if (ModelState.IsValid)
                        {
                            if (await " + repoName + @".Update" + tableName + @"Async(model))
                            {
                                ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel + @" + "" saved"";
                                Log.Info(string.Format(""New " + tableName + @" {0} edited"", model." + primaryKeyCamel + @"));
                                return RedirectToAction(""List" + tableNameplural + @""");
                            }
                            ModelState.AddModelError(""" + tableName + @"EditError"", ""There was a problem editing the " + tableName + @" "" + model." + primaryKeyCamel + @");
                            return View(model);
                        }
                        return View(model);

                    }" + n + n;
                    #endregion

                    #region ListTableName

                    code = code + @"  
                // GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"()
                {
                    Log.Info(""" + tableName + @" called"");
                    return View(" + repoName + @".Get" + tableNameplural + @"List());
                }" + n + n;
                    #endregion

                    #region DeleteTableName

                    code = code + @" 
                // GET: /" + controllerName + @"/Delete" + tableName + @"/5
                public async Task<ActionResult> Delete" + tableName + @"(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                        Log.Warn(""" + tableName + @" is not found id=: "" + id);
                        return HttpNotFound();
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return View(model);
                }
                " + n;

                    code = code + @"   
                // POST: /" + controllerName + @"/Delete" + tableName + @"/5
                [HttpPost, ActionName(""Delete" + tableName + @""")]
                public async Task<ActionResult> DeleteConfirmed" + tableName + @"(int id)
                {
                    var model = await " + repoName + @".Get" + tableName + @"(id);
                    await Task.Run(() =>
                    {
                        " + repoName + @".Delete" + tableName + @"Async(id);
                        Log.Info(string.Format(""" + tableName + @" {0} deleted"", id));
                    });
                    return RedirectToAction(""List" + tableNameplural + @""");
                }" + n + n;
                    #endregion

                    #region GetTableNameByForeignKey

                    FK(tableName);
                    PK(tableName);
                    foreach (DataGridViewRow item_ in FKGV.Rows)
                    {
                        if (item_.Cells[0].Value == null)
                        {
                            continue;
                        }

                        string FKColumn = "";
                        string FKIdFixed = ""; string FKCamel = "";


                        if (item_.Cells[3].Value != null)
                        {
                            FKColumn = item_.Cells[3].Value.ToString();
                            FKIdFixed = FKColumn.Replace("ID", "Id");
                            FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                        }

                        code = code + @"
                            // GET: /" + controllerName + @"/" + tableNameplural + @"By" + FKCamel + @"
                            public ActionResult " + tableNameplural + @"By" + FKCamel + @"(int? id)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                Log.Info(""" + tableNameplural + @"By" + FKCamel + @" called with id: ""+id.Value);
                                return View(" + repoName + @".Get" + tableNameplural + "By" + FKCamel + @"(id.Value));
                            }" + n + n;


                        //code = code + "public List<" + viewModelName + "> Get" + tableNameplural + "By" + FKCamel + "(int " +
                        //       FKIdFixed + ")" + n + "{";
                        //code = code + " var result = db." + tableNameplural + ".Where(a => a." + FKColumn + " == " + FKIdFixed +
                        //       ");" + n;
                        //code = code + @" return result.Select(" + viewModelName + ".ToModel).ToList();" + n + "}" + n + n;
                    }

                    #endregion


                }
                else
                {

                    #region TableNameDetails

                    string actionName = tableName + @"Details";
                    code = code + @"
                // GET: /" + controllerName + "/" + actionName + @"/5
                public async Task<ActionResult> " + actionName + @"(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                        Log.Warn(""" + tableName + @" is not found id=: "" + id);
                        return HttpNotFound();
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return PartialView(""_" + actionName + @""", model);

                }" + n + n;

                    #endregion

                    #region AddTableName

                    code = code + @"
                // GET: /""" + controllerName + @"/Add" + tableName + @"
                public ActionResult Add" + tableName + @"()
                {
                    var model = new " + viewModelName + @"();
                   return PartialView(""_Add" + tableName + @""", model);
                }

                // POST:  /""" + controllerName + @"/Add" + tableName + @"
                    [HttpPost]
                public async Task<ActionResult> Add" + tableName + @"(" + viewModelName + @" model)
                {
                    if (ModelState.IsValid)
                    {
                        if (await " + repoName + @".Add" + tableName + @"Async(model))
                        {
                            ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel + @" + "" added"";
                            Log.Info(string.Format(""New " + tableName + @" {0} added"", model." + primaryKeyCamel + @"));
                            return RedirectToAction(""List" + tableName + @""");
                        }
                        ModelState.AddModelError(""" + tableName + @"AddingError"", ""There was a problem adding the " + tableName + @" "" + model." + primaryKeyCamel + @");
                   return PartialView(""_Add" + tableName + @""", model);
                    }
                   return PartialView(""_Add" + tableName + @""", model);
                 }" + n + n;

                    #endregion

                    #region EditTableName

                    code = code + @" 
                // GET: /" + controllerName + @"/Edit" + tableName + @"
                public async Task<ActionResult> Edit" + tableName + @"(int? id)
                {
                    var model = new " + viewModelName + @"();
                    //get the " + tableName + @" from the DB
                    if (id.HasValue)
                    {
                        model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    }
                    return PartialView(""_Edit" + tableName + @""", model);
                }


                    // POST: /" + controllerName + @"/Edit" + tableName + @"
                    [HttpPost]
                    public async Task<ActionResult> Edit" + tableName + @"(" + viewModelName + @" model)
                    {
                        if (ModelState.IsValid)
                        {
                            if (await " + repoName + @".Update" + tableName + @"Async(model))
                            {
                                ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel + @" + "" saved"";
                                Log.Info(string.Format(""New " + tableName + @" {0} edited"", model." + primaryKeyCamel + @"));
                                return RedirectToAction(""List" + tableNameplural + @""");
                            }
                            ModelState.AddModelError(""" + tableName + @"EditError"", ""There was a problem editing the " + tableName + @" "" + model." + primaryKeyCamel + @");
                    return PartialView(""_Edit" + tableName + @""", model);
                        }
                    return PartialView(""_Edit" + tableName + @""", model);

                    }" + n + n;
                    #endregion

                    #region ListTableName

                    code = code + @"  
                // GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"()
                {
                    Log.Info(""" + tableName + @" called"");
                    return PartialView(""_Edit" + tableName + @""", " + repoName + @".Get" + tableNameplural + @"List());
                }" + n + n;
                    #endregion

                    #region DeleteTableName

                    code = code + @" 
                // GET: /" + controllerName + @"/Delete" + tableName + @"/5
                public async Task<ActionResult> Delete" + tableName + @"(int? id)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                        Log.Warn(""" + tableName + @" is not found id=: "" + id);
                        return HttpNotFound();
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return PartialView(""_Delete" + tableName + @""", model);

                }
                " + n;

                    code = code + @"   
                // POST: /" + controllerName + @"/Delete" + tableName + @"/5
                [HttpPost, ActionName(""Delete" + tableName + @""")]
                public async Task<ActionResult> DeleteConfirmed" + tableName + @"(int id)
                {
                    var model = await " + repoName + @".Get" + tableName + @"(id);
                    await Task.Run(() =>
                    {
                        " + repoName + @".Delete" + tableName + @"Async(id);
                        Log.Info(string.Format(""" + tableName + @" {0} deleted"", id));
                    });
                    return RedirectToAction(""List" + tableNameplural + @""");
                }" + n + n;
                    #endregion

                    #region GetTableNameByForeignKey

                    FK(tableName);
                    PK(tableName);
                    foreach (DataGridViewRow item_ in FKGV.Rows)
                    {
                        if (item_.Cells[0].Value == null)
                        {
                            continue;
                        }

                        string FKColumn = "";
                        string FKIdFixed = ""; string FKCamel = "";


                        if (item_.Cells[3].Value != null)
                        {
                            FKColumn = item_.Cells[3].Value.ToString();
                            FKIdFixed = FKColumn.Replace("ID", "Id");
                            FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                        }

                        code = code + @"
                            // GET: /" + controllerName + @"/" + tableNameplural + @"By" + FKCamel + @"
                            public ActionResult " + tableNameplural + @"By" + FKCamel + @"(int? id)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                Log.Info(""" + tableNameplural + @"By" + FKCamel + @" called with id: ""+id.Value);
                                return View(" + repoName + @".Get" + tableNameplural + "By" + FKCamel + @"(id.Value));
                            }" + n + n;


                        //code = code + "public List<" + viewModelName + "> Get" + tableNameplural + "By" + FKCamel + "(int " +
                        //       FKIdFixed + ")" + n + "{";
                        //code = code + " var result = db." + tableNameplural + ".Where(a => a." + FKColumn + " == " + FKIdFixed +
                        //       ");" + n;
                        //code = code + @" return result.Select(" + viewModelName + ".ToModel).ToList();" + n + "}" + n + n;
                    }

                    #endregion
                }

                #region JSON list

                code = code + @"
                #region Json
  
                // Json GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"Json()
                {
                    Log.Info(""" + tableName + @" called"");
                    return Json(""_Edit" + tableName + @""", " + repoName + @".Get" + tableNameplural + @"List(), JsonRequestBehavior.AllowGet);
                }" + n + n;


                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    string FKColumn = "";
                    string FKIdFixed = ""; string FKCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        FKColumn = item_.Cells[3].Value.ToString();
                        FKIdFixed = FKColumn.Replace("ID", "Id");
                        FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                    }

                    code = code + @"
                            // Json GET: /" + controllerName + @"/" + tableNameplural + @"By" + FKCamel + @"Json
                            public ActionResult " + tableNameplural + @"By" + FKCamel + @"Json(int? id)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                return Json(" + repoName + @".Get" + tableNameplural + "By" + FKCamel + @"(id.Value), JsonRequestBehavior.AllowGet);
                            }" + n + n;
                }

                code = code + @"
                #endregion";
                #endregion

                #region Kendo Grid

                code = code + @"
                
                #region Kendo Grid
      
                // Json GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"Kendo([DataSourceRequest] DataSourceRequest request)
                {
                    Log.Info(""" + tableName + @" called"");
                    return Json(""_Edit" + tableName + @""", " + repoName + @".Get" + tableNameplural + @"List().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }" + n + n;


                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    string FKColumn = "";
                    string FKIdFixed = ""; string FKCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        FKColumn = item_.Cells[3].Value.ToString();
                        FKIdFixed = FKColumn.Replace("ID", "Id");
                        FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                    }

                    code = code + @"
                            // Json GET: /" + controllerName + @"/" + tableNameplural + @"By" + FKCamel + @"Json
                            public ActionResult " + tableNameplural + @"By" + FKCamel + @"Kendo(int? id,[DataSourceRequest] DataSourceRequest request)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                return Json(" + repoName + @".Get" + tableNameplural + "By" + FKCamel + @"(id.Value).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                            }" + n + n;
                }
                code = code + @"
                #endregion";

                #endregion
                code = code + "#endregion" + n + n;
                myReader.Close();
            }
            //Always close the DataReader and connection.
            cn.Close();
            ActionsResult.Text = code;
            ActionsResult.Focus();
            ActionsResult.SelectAll();
        }


        public string ToPlural(string text)
        {

            string tableNameplural = text + "s";
            if (text.Last().Equals('y'))
            {
                //agency agenc ies
                tableNameplural = text.Remove(text.Length - 1) + "ies";
            }
            if (tableNameplural.Substring(tableNameplural.Length - 2).Equals("ss"))
            {
                tableNameplural = tableNameplural.Remove(tableNameplural.Length - 1);
            }

            if (tableNameplural.Substring(tableNameplural.Length - 3).Equals("sss"))
            {
                tableNameplural = tableNameplural.Remove(tableNameplural.Length - 2) + "es";
            }
            return tableNameplural;
        }

        private void RepoCodeText_Click(object sender, EventArgs e)
        {
            RepoCodeText.SelectAll();
            if (!string.IsNullOrEmpty(RepoCodeText.Text))
                Clipboard.SetText(RepoCodeText.Text);
        }

        private void ActionsResult_Click(object sender, EventArgs e)
        {
            ActionsResult.SelectAll();
            if (!string.IsNullOrEmpty(ActionsResult.Text))
                Clipboard.SetText(ActionsResult.Text);

        }

        private void DBSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ServeTB.Text.Length > 0 && UserTB.Text.Length > 0 && PassTB.Text.Length > 0)
            {
                TableNameRepo.Items.Clear();
                SqlConnection conn = new SqlConnection("user id=" + UserTB.Text + ";" +
                                         "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                         "Trusted_Connection=false;" +
                                         "database=" + DatabaseCB.SelectedText + "; " +
                                         "connection timeout=30");
                conn.Open();
                List<string> tables = new List<string>();

                DataTable dt = conn.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    if (((string)row[3]).Equals("BASE TABLE") && ((string)row[1]).Equals("dbo"))
                    {
                        string tablename = (string)row[2];
                        tables.Add(tablename);
                    }
                }
                tables.Sort();
                foreach (var item in tables)
                {
                    TableNameRepo.Items.Add(item);
                }
                conn.Close();
            }
        }

        private void ServeTB_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
