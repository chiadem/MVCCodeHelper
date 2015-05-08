#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

namespace CHI_MVCCodeHelper
{
    public partial class MVCCodeHelperMain : Form
    {
        //private readonly Dictionary<int, string> _columns = new Dictionary<int, string>();

        private List<Property> _propertyList = new List<Property>();

        private SqlConnection _dbConnection;
        private SoundPlayer butters, nice, help;
        public MVCCodeHelperMain()
        {
            InitializeComponent();
            listBox1.AllowDrop = true;

             butters = new SoundPlayer(Properties.Resources.butters);
             nice = new SoundPlayer(Properties.Resources.nice);
             help = new SoundPlayer(Properties.Resources.helpaudio);
        }

        private void ViewModelGen_Load(object sender, EventArgs e)
        {
            var user = Settings.Get("User", "");
            var pass = Settings.Get("Password", "");
            var server = Settings.Get("Server", "");
            var db = Settings.Get("DBName", "");
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(server) ||
                string.IsNullOrEmpty(db))
            {
                DBSettings.SelectedTab = tabPage1;
                MessageBox.Show("Set your DB settings before you start");
            }
            else
            {
                OpenDBConnection(user, pass, server, db);
                FillTableNameListBoxes();
            }
        }

        private void OpenDBConnection(string user, string pass, string server, string db)
        {
            UserTB.Text = user;
            PassTB.Text = pass;
            ServeTB.Text = server;
            DatabaseCB.Text = db;
            _dbConnection = new SqlConnection("user id=" + user + ";" +
                                              "password=" + pass + ";server=" + server + ";" +
                                              "Trusted_Connection=false;" +
                                              "database=" + db + "; " +
                                              "connection timeout=30");
            _dbConnection.Open();
        }

        private void TestB_Click(object sender, EventArgs e)
        {
            _dbConnection = new SqlConnection("user id=" + UserTB.Text + ";" +
                                              "password=" + PassTB.Text + ";server=" + ServeTB.Text + ";" +
                                              "Trusted_Connection=false;" +
                                              "database=" + DatabaseCB.Text + "; " +
                                              "connection timeout=30");
            try
            {
                _dbConnection.Open();
                nice.Play();
                MessageBox.Show("Succesful! Setting are saved for databse:" + DatabaseCB.Text + ".");
                Settings.Set("User", UserTB.Text);
                Settings.Set("Password", PassTB.Text);
                Settings.Set("Server", ServeTB.Text);
                Settings.Set("DBName", DatabaseCB.Text);
                FillTableNameListBoxes();

            }
            catch (Exception)
            {
                butters.Play();
                MessageBox.Show("Failed!");
            }
        }

        private void DatabaseCB_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseCB.Items.Clear();

                //get databases
                var tblDatabases = _dbConnection.GetSchema("Databases");

                //add to list
                foreach (DataRow row in tblDatabases.Rows)
                {
                    var strDatabaseName = row["database_name"].ToString();


                    DatabaseCB.Items.Add(strDatabaseName);
                }
            }
            catch (Exception)
            {
                _dbConnection.Close();
            }

        }

        private void TableCB_Click(object sender, EventArgs e)
        {

        }

        private void GenerateViewModel_Click(object sender, EventArgs e)
        {
            if (VMTableCB.SelectedIndex == -1)
            {

                butters.Play();
                MessageBox.Show("Select a table first bro!");
                return;
            }
            else
            {
                var viewModelName = ViewModelNameTB.Text;
                var tableName = VMTableCB.SelectedItem.ToString();
                var tableNameplural = ToPlural(tableName);

                var columns = new string[100, 4];
                const string n = "\n";

                var vmCode = "public  class " + viewModelName + " : ViewModel" + n + "{" + n + n;

                var toEntity = @"public " + tableName + @" ToEntity()" + n + @"
            {" + n + @"
            var entity = new " + tableName + @"" + n + @"
            {" + n;
                var toEntityP = "public " + tableName + " ToEntity(" + tableName + " entity)" + "{" + n;
                var toModel = " public static " + viewModelName + " ToModel(" + tableName + " entity)" + "{" + n
                              + @" var model = new " + viewModelName + "();"
                              + n + " if (entity != null)" + n + "{" + n;
                var toModelNested = " public static " + viewModelName + " ToModelWithNestedModels(" + tableName +
                                    " entity, int levels = 0)" + "{" + n
                                    + @" var model = new " + viewModelName + "();"
                                    + n + " if (entity != null)" + n + "{" + n;
                var constructor = "public " + viewModelName + "()" + n + "{" + n;
                var primaryKey = "";
                var primaryKeyCamel = "";

                var cmd = new SqlCommand
                {
                    Connection = _dbConnection,
                    CommandText = "SELECT top 1 * FROM " + tableName
                };


                //Retrieve records from the Employees table into a DataReader.
                var myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);

                //Retrieve column schema into a DataTable.
                var schemaTable = myReader.GetSchemaTable();
                var i = 0;
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {
                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    var columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                        (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                        (columnName.Equals("createdBy") && !createdBy.Checked) ||
                        (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                        (columnName.Equals("rowVersion") && !rowVersion.Checked)
                        )
                    {
                        continue;
                    }


                    var columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                    columnNameCamel = columnNameCamel.Replace("ID", "Id");

                    var displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                    var dataType =
                        myField["DataType"].ToString()
                            .Replace("System.", "")
                            .Replace("Int16", "short")
                            .Replace("Int32", "int")
                            .Replace("Int", "int")
                            .Replace("String", "string")
                            .Replace("Boolean", "bool");
                    var size = Convert.ToInt32(myField["ColumnSize"]);
                    var isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    var isKey = Convert.ToBoolean(myField["IsKey"]);
                    if (isNullable)
                    {
                        if (!dataType.Equals("string"))
                        {
                            dataType = dataType + "?";
                        }
                    }
                    else if (RequiredAnn.Checked && !isKey && !(columnName.Equals("createdDate") ||
                                                                columnName.Equals("modifiedDate") ||
                                                                columnName.Equals("createdBy") ||
                                                                columnName.Equals("modifiedBy") ||
                                                                columnName.Equals("rowVersion")))
                    {
                        vmCode = vmCode + @"[Required(ErrorMessage = ""{0} value is empty"")]" + n;
                    }

                    if (isKey)
                    {
                        vmCode = HiddenInputAnn.Checked ? vmCode + "[HiddenInput(DisplayValue = false)]" : vmCode + n;
                        primaryKey = columnName;
                        primaryKeyCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                        primaryKeyCamel = columnNameCamel.Replace("ID", "Id");
                    }
                    else if (DisplayAnn.Checked)
                    {
                        vmCode = vmCode + @"[Display(Name = """ + displayName + @""")]" + n;
                    }
                    if (dataType.Equals("string") & StringLengthAnn.Checked && size < 2147483647)
                    {
                        vmCode = vmCode + @"[StringLength(" + size +
                                 @", ErrorMessage = ""The length of {0} exceeds the limit of {1} characters!"")]" + n;
                    }

                    vmCode = vmCode + "public " + dataType + " " + columnNameCamel + " { get; set; }" + n + n;

                    toEntity = toEntity + columnName + " = " + columnNameCamel + "," + n;
                    toEntityP = toEntityP + "entity." + columnName + " = " + columnNameCamel + ";" + n;
                    toModel = toModel + "model." + columnNameCamel + " = entity." + columnName + ";" + n;
                    toModelNested = toModelNested + "model." + columnNameCamel + " = entity." + columnName + ";" + n;

                    i++;
                }

                //Always close the DataReader and connection.
                myReader.Close();

                Pk(tableName);
                toModelNested += "using (var repo = new WTRepository())" + n + "{";
                foreach (DataGridViewRow itemm in PKGV.Rows)
                {
                    if (itemm.Cells[0].Value == null)
                    {
                        continue;
                    }

                    if (itemm.Cells[6].Value != null)
                    {
                        var fkTableName = itemm.Cells[6].Value.ToString();
                        var fkTableNamePlurar = fkTableName + "s";
                        if (fkTableName.Last().Equals('y'))
                        {
                            //agency agenc ies
                            fkTableNamePlurar = fkTableName.Remove(fkTableName.Length - 1) + "ies";
                        }
                        var fkTableNameVm = fkTableName + "VM";
                        vmCode = vmCode + n + "public List<" + fkTableNameVm + "> " + fkTableNamePlurar +
                                 " { get; set; }" + n;
                        toModelNested += " model." + fkTableNamePlurar + " = repo.Get" + fkTableNamePlurar + "By" +
                                         primaryKeyCamel + "(entity." + primaryKey + ",levels); " + n;
                        constructor += fkTableNamePlurar + " =  new List<" + fkTableNameVm + ">();" + n;
                    }
                }


                vmCode += constructor + "}" + n;
                vmCode = vmCode + toEntity.TrimEnd(',') + n + @"};" + n + @"return entity;" + n + @"}" + n + n;
                vmCode = vmCode + toEntityP + n + @"return entity;" + n + @"}" + n + n;
                vmCode = vmCode + toModel + n + "}" + n + " return model;" + n + "}" + n + n;
                vmCode = vmCode + toModelNested + "}" + n + "}" + n + " return model;" + n + "}" + n + n;
                vmCode = vmCode + n + "}" + n;
                CodeText.Text = vmCode;
                CodeText.Focus();
                //  Clipboard.SetText(CodeText.Text);
                nice.Play();

            }
        }

        private void TableCB_SelectedValueChanged(object sender, EventArgs e)
        {
            ViewModelNameTB.Text = VMTableCB.SelectedItem + "VM";
        }


        private void GenerateRepositoryBtn_Click(object sender, EventArgs e)
        {
            if (TableGrid.Rows.Count == 0 || TableGrid.Rows[0].Cells[0].Value == null)
                MessageBox.Show(
                    "Add at least one table to the queue! And make sure all table names are set in the queue.");


            var cmd = new SqlCommand();

            var repoCode = "";
            foreach (DataGridViewRow item in TableGrid.Rows)
            {
                if (item.Cells[0].Value == null)
                {
                    continue;
                }

                var tableName = item.Cells[0].Value.ToString();
                var regionName = item.Cells[1].Value.ToString();
                var viewModelName = tableName + "VM";

                var tableNameplural = ToPlural(tableName);

                //Retrieve records from the Employees table into a DataReader.
                cmd.Connection = _dbConnection;
                cmd.CommandText = "SELECT top 1 * FROM " + tableName;
                var myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //Retrieve column schema into a DataTable.
                var schemaTable = myReader.GetSchemaTable();

                var columns = new string[100, 4];
                const string n = "\n";

                repoCode = repoCode + " #region " + regionName + n + n;

                var primaryKey = "";
                var primaryKeyCamel = "";
                var parameterPk = "";

                //string FK = "";
                //string FKIdFixed = ""; string FKCamel = "";


                //if (!string.IsNullOrEmpty(item.Cells[1].Value.ToString()))
                //{
                //    FKIdFixed = FK.Replace("ID", "Id");
                //    FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                //}

                var i = 0;
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {
                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    var columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                        (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                        (columnName.Equals("createdBy") && !createdBy.Checked) ||
                        (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                        (columnName.Equals("rowVersion") && !rowVersion.Checked)
                        )
                    {
                        continue;
                    }


                    var columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                    columnNameCamel = columnNameCamel.Replace("ID", "Id");

                    var displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                    var dataType =
                        myField["DataType"].ToString()
                            .Replace("System.", "")
                            .Replace("Int16", "short")
                            .Replace("Int32", "int")
                            .Replace("Int", "int")
                            .Replace("String", "string")
                            .Replace("Boolean", "bool");
                    var size = Convert.ToInt32(myField["ColumnSize"]);
                    var isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    var isKey = Convert.ToBoolean(myField["IsKey"]);

                    if (isKey)
                    {
                        primaryKey = columnName;
                        parameterPk = columnName.Replace("ID", "Id");
                        primaryKeyCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                        primaryKeyCamel = columnNameCamel.Replace("ID", "Id");
                    }

                    i++;
                }

                #region GetTableName

                repoCode = repoCode + "public async Task<" + viewModelName + "> Get" + tableName + " (int " +
                           parameterPk + ", int levels = 0)" + n + "{";
                repoCode = repoCode + "var result = await _db." + tableName + ".SingleOrDefaultAsync(e => e." +
                           primaryKey + " == " +
                           parameterPk + ");" + n;

                repoCode = repoCode +
                           "if (result != null){ " + n + " return levels > 0 ? " + viewModelName +
                           @".ToModelWithNestedModels(result,levels - 1) :" + viewModelName + @".ToModel(result);" + n +
                           @"}
                    Log.Warn(""" + tableName + @" is not found id: "" + " + parameterPk + @");
                    return null;
                }";

                #endregion

                #region GetTableNameList

                repoCode = repoCode + @"
                    public List<" + viewModelName + @"> Get" + tableNameplural + @"List(int levels = 0)
                    {
                        return levels > 0 ? _db." + tableName + @".Select(x => " + viewModelName +
                           @".ToModelWithNestedModels(x,levels - 1)).ToList() : _db." + tableName + @".Select(" +
                           viewModelName + @".ToModel).ToList();
                    }" + n + n;

                #endregion

                myReader.Close();

                #region GetTableNameByForeignKey

                Fk(tableName);
                Pk(tableName);
                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    var fkColumn = "";
                    var fkIdFixed = "";
                    var fkCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        fkColumn = item_.Cells[3].Value.ToString();
                        fkIdFixed = fkColumn.Replace("ID", "Id");
                        fkCamel = fkIdFixed.First().ToString().ToUpper() + String.Join("", fkIdFixed.Skip(1));
                    }

                    repoCode = repoCode + "public List<" + viewModelName + "> Get" + tableNameplural + "By" + fkCamel +
                               "(int " +
                               fkIdFixed + ", int levels = 0)" + n + "{";
                    repoCode = repoCode + " var result = _db." + tableName + ".Where(a => a." + fkColumn + " == " +
                               fkIdFixed +
                               ").ToList();" + n;
                    repoCode = repoCode + "levels -= 1; " + n + @" return (levels + 1) > 0 ? result.Select(x => " +
                               viewModelName + ".ToModelWithNestedModels(x,levels)).ToList() : result.Select(" +
                               viewModelName + ".ToModel).ToList();" + n + "}" + n + n;
                }

                #endregion

                #region AddTableNameAsync

                var recursiveAdd = "";
                var recursiveUpdate = "";
                var recursivecheck = Convert.ToBoolean(item.Cells[5].Value);

                if (recursivecheck)
                {
                    foreach (DataGridViewRow itemm in PKGV.Rows)
                    {
                        if (itemm.Cells[0].Value == null)
                        {
                            continue;
                        }
                        if (itemm.Cells[6].Value == null) continue;
                        var fkTableName = itemm.Cells[6].Value.ToString();
                        var fkTableNamePlurar = fkTableName + "s";
                        if (fkTableName.Last().Equals('y'))
                        {
                            //agency agenc ies
                            fkTableNamePlurar = fkTableName.Remove(fkTableName.Length - 1) + "ies";
                        }
                        var fkTableNameVm = fkTableName + "VM";

                        recursiveAdd = recursiveAdd + "foreach (var " + fkTableName.ToLower() + " in  model." +
                                       fkTableNamePlurar + " ?? Enumerable.Empty<" + fkTableNameVm + ">())" + n +
                                       "{" + n +
                                       fkTableName.ToLower() + "." + primaryKeyCamel + " =model." + primaryKeyCamel +
                                       ";" + n +
                                       "await Add" + fkTableName + @"Async(" + fkTableName.ToLower() + ");" +
                                       n + "}" + n;

                        recursiveUpdate = recursiveUpdate + "foreach (var " + fkTableName.ToLower() + " in  model." +
                                          fkTableNamePlurar + " ?? Enumerable.Empty<" + fkTableNameVm + ">())" + n +
                                          "{" + n +
                                          fkTableName.ToLower() + "." + primaryKeyCamel + " =model." +
                                          primaryKeyCamel +
                                          ";" + n +
                                          "await AddOrUpdate" + fkTableName + @"Async(" + fkTableName.ToLower() +
                                          ");" +
                                          n + "}" + n;
                    }
                    item.Cells[6].Value = recursiveAdd;
                    item.Cells[7].Value = recursiveUpdate;
                }
                repoCode = repoCode + @"

            public async Task<int?> Add" + tableName + @"Async(" + viewModelName + @" model)
            {
            try
            {
                var entity = _db." + tableName + @".Add(model.ToEntity());
                await _db.SaveChangesAsync();
                model." + primaryKeyCamel + @"=entity." + primaryKey + @";
                Log.Info(""" + tableName + @" added with id  "" + model." + primaryKeyCamel + @");" + n + n +
                           recursiveAdd + n + @"return entity." + primaryKey + @";
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    errors.Add(""Entity of type "" + eve.Entry.Entity.GetType().Name + "" in state "" + eve.Entry.State + "" has the following validation errors:"");
                    errors.AddRange(eve.ValidationErrors.Select(ve => "" Property: '"" + ve.PropertyName + ""'""+ "" Error: '"" + ve.ErrorMessage + ""'""));
                }

                Log.Error(""Failed to add " + tableName +
                           @""" + string.Join("","", errors.ToArray()) +"". Json model: "" + JsonConvert.SerializeObject(model, Formatting.Indented), ex);
                return null;

            }
            catch (Exception ex)
            {
                Log.Error(""Failed to add " + tableName +
                           @". Json model: "" + JsonConvert.SerializeObject(model, Formatting.Indented), ex);
                return null;
            }" + n + "}" + n + n;

                #endregion

                #region UpdateTableNameAsync

                repoCode = repoCode + @" public async Task<bool> Update" + tableName + @"Async(" + viewModelName +
                           @" model)
        {
            try
            {
                var entity = await _db." + tableName + @".SingleOrDefaultAsync(e => e." + primaryKey + @" == model." +
                           primaryKeyCamel + @");
                entity = model.ToEntity(entity);
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                model." + primaryKeyCamel + @"=entity." + primaryKey + @";
                Log.Info(""" + tableName + @" updated with id "" + model." + primaryKeyCamel + @");" + n +
                           recursiveUpdate + n + @"
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    errors.Add(""Entity of type "" + eve.Entry.Entity.GetType().Name + "" in state "" + eve.Entry.State + "" has the following validation errors:"");
                    errors.AddRange(eve.ValidationErrors.Select(ve => "" Property: '"" + ve.PropertyName + ""'""+ "" Error: '"" + ve.ErrorMessage + ""'""));
                }

                Log.Error(""Failed to update " + tableName + @" with id "" + model." + primaryKeyCamel +
                           @" + "" - "" + string.Join("","", errors.ToArray()));
                return false;

            }
            catch (Exception ex)
            {
                Log.Error(""Failed to update " + tableName + @" with id "" + model." + primaryKeyCamel + @", ex);
                return false;
            }
        }" + n + n;

                #endregion

                repoCode = repoCode + @" public async Task<bool> AddOrUpdate" + tableName + @"Async(" + viewModelName +
                           @" model)
        {
            bool exist = await _db." + tableName + @".AnyAsync(e => e." + primaryKey + @" == model." + primaryKeyCamel +
                           @");
            if (exist)
            {
                return await Update" + tableName + @"Async(model);
            }
            var result = await Add" + tableName + @"Async(model);
            return result != null;
        }" + n + n;

                #region UpdateTableNameAsync

                #endregion

                #region DeleteTableNameAsync

                repoCode = repoCode + @"public async Task<bool> Delete" + tableName + @"Async(int id)
        {
            try
            {
                var result = await _db." + tableName + @".SingleOrDefaultAsync(e => e." + primaryKey + @" == id);
                if (result != null)
                {
                    _db." + tableName + @".Remove(result);
                    await _db.SaveChangesAsync();

                    Log.Info(""" + tableName + @" deleted with id "" + id);
                    return true;
                }
                Log.Info(""" + tableName + @" not found with id "" + id);
                return false;
                }
            catch (DbEntityValidationException ex)
            {
                var errors = new List<string>();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    errors.Add(""Entity of type "" + eve.Entry.Entity.GetType().Name + "" in state "" + eve.Entry.State + "" has the following validation errors:"");
                    errors.AddRange(eve.ValidationErrors.Select(ve => "" Property: '"" + ve.PropertyName + ""'""+ "" Error: '"" + ve.ErrorMessage + ""'""));
                }

                Log.Error(""Failed to delete " + tableName +
                           @" with id "" + id + "" - "" + string.Join("","", errors.ToArray()));
                return false;

            }
                catch (Exception ex)
                {
                    Log.Error(""Failed to delete " + tableName + @" with id "" + id, ex);
                    return false;
                }
              }
            " + n + n;

                #endregion

                repoCode = repoCode + "#endregion" + n + n;
            }
            RepoCodeText.Text = repoCode;
            RepoCodeText.Focus();
            RepoCodeText.SelectAll();
            GenerateController();
            nice.Play();
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

        private void RepoControllerQueueBtn_Click(object sender, EventArgs e)
        {
            if (TableNameRepo.SelectedIndex == -1)
            {
                
                butters.Play();

                MessageBox.Show(
                    "Which table do you want to add to the queue? Select a table first then click this button again!");
            }
            else if (TableGrid.Rows.Count > 1)
            {
                if (!TableGrid.Rows
                    .Cast<DataGridViewRow>()
                    .Any(
                        r =>
                            r.Cells["CTableName"].Value != null &&
                            r.Cells["CTableName"].Value.ToString().Equals(TableNameRepo.Text)))
                    TableGrid.Rows.Add(TableNameRepo.Text, RegionTB.Text, RepoNameTB.Text, ControllerNameTB.Text,
                        isPartial.Checked, recursiveAdd.Checked);
            }
            else
                TableGrid.Rows.Add(TableNameRepo.Text, RegionTB.Text, RepoNameTB.Text, ControllerNameTB.Text,
                    isPartial.Checked, recursiveAdd.Checked);
            //foreach (var item in TableNameRepo.Items)
            //{
            //    TableGrid.Rows.Add(item.ToString(), FKTB.Text, item.ToString());
            //}
        }

        private void Fk(string tableName)
        {
            var cmd = new SqlCommand();
            var ds = new DataSet("TimeRanges");

            cmd.CommandText = "sp_fkeys";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@fktable_name", SqlDbType.VarChar, 100).Value = tableName;
            cmd.Connection = _dbConnection;

            var da = new SqlDataAdapter { SelectCommand = cmd };

            da.Fill(ds);

            FKGV.AutoGenerateColumns = true;
            FKGV.DataSource = ds;
            FKGV.DataMember = ds.Tables[0].TableName;
            // Data is accessible through the DataReader object here.
        }

        private void Pk(string tableName)
        {
            var cmd = new SqlCommand();
            var ds = new DataSet("TimeRanges");

            cmd.CommandText = "sp_fkeys";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pktable_name", SqlDbType.VarChar, 100).Value = tableName;
            cmd.Connection = _dbConnection;

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);

            PKGV.AutoGenerateColumns = true;
            PKGV.DataSource = ds;
            PKGV.DataMember = ds.Tables[0].TableName;
        }


        public void GenerateController()
        {
            var cmd = new SqlCommand();

            var contCode = "";
            foreach (DataGridViewRow item in TableGrid.Rows)
            {
                if (item.Cells[0].Value == null)
                {
                    continue;
                }

                var tableName = item.Cells[0].Value.ToString();
                var repoName = item.Cells[2].Value.ToString();
                var controllerName = item.Cells[3].Value.ToString().Replace("Controller", "");
                var isPartial = Convert.ToBoolean(item.Cells[4].Value);
                var viewModelName = tableName + "VM";

                var tableNameplural = ToPlural(tableName);
                //Retrieve records from the Employees table into a DataReader.
                cmd.Connection = _dbConnection;
                cmd.CommandText = "SELECT top 1 * FROM " + tableName;
                var myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //Retrieve column schema into a DataTable.
                var schemaTable = myReader.GetSchemaTable();

                var columns = new string[100, 4];
                const string n = "\n";

                contCode = contCode + " #region " + tableName + n + n;

                var primaryKeyCamel = "";
                var parameterPk = "";

                //string FK = "";
                //string FKIdFixed = ""; string FKCamel = "";


                //if (!string.IsNullOrEmpty(item.Cells[1].Value.ToString()))
                //{
                //    FKIdFixed = FK.Replace("ID", "Id");
                //    FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                //}

                var i = 0;
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {
                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    var columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !createdDate.Checked) ||
                        (columnName.Equals("modifiedDate") && !modifiedDate.Checked) ||
                        (columnName.Equals("createdBy") && !createdBy.Checked) ||
                        (columnName.Equals("modifiedBy") && !modifiedBy.Checked) ||
                        (columnName.Equals("rowVersion") && !rowVersion.Checked)
                        )
                    {
                        continue;
                    }


                    var columnNameCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                    columnNameCamel = columnNameCamel.Replace("ID", "Id");

                    //  var displayName = Regex.Replace(columnNameCamel, "(\\B[A-Z])", " $1");
                    ////   var dataType =
                    //       myField["DataType"].ToString()
                    //           .Replace("System.", "")
                    //           .Replace("Int16", "short")
                    //           .Replace("Int32", "int")
                    //           .Replace("Int", "int")
                    //           .Replace("String", "string")
                    //           .Replace("Boolean", "bool");
                    // var size = Convert.ToInt32(myField["ColumnSize"]);
                    // var isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    var isKey = Convert.ToBoolean(myField["IsKey"]);

                    if (isKey)
                    {
                        parameterPk = columnName.Replace("ID", "Id");
                        primaryKeyCamel = columnName.First().ToString().ToUpper() + String.Join("", columnName.Skip(1));
                        primaryKeyCamel = columnNameCamel.Replace("ID", "Id");
                    }

                    i++;
                }
                if (!isPartial)
                {
                    #region TableNameDetails

                    var actionName = tableName + @"Details";
                    contCode = contCode + @"
                // GET: /" + controllerName + "/" + actionName + @"/5
                public async Task<ActionResult> " + actionName + @"(int? id)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                        return HttpNotFound();
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return View(model);
                }" + n;

                    #endregion

                    #region AddTableName

                    contCode = contCode + @"
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
                        var " + parameterPk + @" = await " + repoName + @".Add" + tableName + @"Async(model);
                        if (" + parameterPk + @".HasValue)
                        {
                            ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel +
                               @" + "" added"";
                            return RedirectToAction(""List" + tableName + @""");
                        }
                        ModelState.AddModelError(""" + tableName + @"AddingError"", ""There was a problem adding the " +
                               tableName + @" "" + model." + primaryKeyCamel + @");
                        return View(model);
                    }
                    return View(model);
                 }" + n + n;

                    #endregion

                    #region EditTableName

                    contCode = contCode + @" 
                // GET: /" + controllerName + @"/Edit" + tableName + @"
                public async Task<ActionResult> Edit" + tableName + @"(int? id)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                  
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model!=null)
                    { 
                       return View(""Edit" + tableName + @""", model);
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                    // POST: /" + controllerName + @"/Edit" + tableName + @"
                    [HttpPost]
                    public async Task<ActionResult> Edit" + tableName + @"(" + viewModelName + @" model)
                    {
                        if (ModelState.IsValid)
                        {
                            if (await " + repoName + @".Update" + tableName + @"Async(model))
                            {
                                ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel +
                               @" + "" saved"";
                                return RedirectToAction(""List" + tableNameplural + @""");
                            }
                            ModelState.AddModelError(""" + tableName +
                               @"EditError"", ""There was a problem editing the " + tableName + @" "" + model." +
                               primaryKeyCamel + @");
                            return View(model);
                        }
                        return View(model);

                    }" + n;

                    #endregion

                    #region ListTableName

                    contCode = contCode + @"  
                // GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"()
                {
                    //Log.Info(""" + tableName + @" called"");
                    return View(" + repoName + @".Get" + tableNameplural + @"List());
                }" + n + n;

                    #endregion

                    #region DeleteTableName

                    contCode = contCode + @" 
                // GET: /" + controllerName + @"/Delete" + tableName + @"/5
                public async Task<ActionResult> Delete" + tableName + @"(int? id)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    
                    var model = await " + repoName + @".Get" + tableName + @"(id.Value);
                    if (model == null)
                    {
                     return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    }
                    Log.Info(""" + actionName + @" is called id: "" + id);
                    return View(model);
                }
                " + n;

                    contCode = contCode + @"   
                // POST: /" + controllerName + @"/Delete" + tableName + @"/5
                [HttpPost, ActionName(""Delete" + tableName + @""")]
                public async Task<ActionResult> DeleteConfirmed" + tableName + @"(int id)
                {
                    //var model = await " + repoName + @".Get" + tableName + @"(id);
                    await " + repoName + @".Delete" + tableName + @"Async(id);
                    return RedirectToAction(""List" + tableNameplural + @""");
                }" + n + n;

                    #endregion

                    #region GetTableNameByForeignKey

                    myReader.Close();
                    Fk(tableName);
                    Pk(tableName);
                    foreach (DataGridViewRow item_ in FKGV.Rows)
                    {
                        if (item_.Cells[0].Value == null)
                        {
                            continue;
                        }

                        var FKCamel = "";


                        if (item_.Cells[3].Value != null)
                        {
                            var FKColumn = item_.Cells[3].Value.ToString();
                            var FKIdFixed = FKColumn.Replace("ID", "Id");
                            FKCamel = FKIdFixed.First().ToString().ToUpper() + String.Join("", FKIdFixed.Skip(1));
                        }

                        contCode = contCode + @"
                            // GET: /" + controllerName + @"/" + tableNameplural + @"By" + FKCamel + @"
                            public ActionResult " + tableNameplural + @"By" + FKCamel + @"(int? id)
                            {
                                if (id == null)
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                
                                Log.Info(""" + tableNameplural + @"By" + FKCamel + @" called with id: ""+id.Value);
                                return View(" + repoName + @".Get" + tableNameplural + "By" + FKCamel + @"(id.Value));
                            }" + n;


                        //code = code + "public List<" + viewModelName + "> Get" + tableNameplural + "By" + FKCamel + "(int " +
                        //       FKIdFixed + ")" + n + "{";
                        //code = code + " var result = _db." + tableName + ".Where(a => a." + FKColumn + " == " + FKIdFixed +
                        //       ");" + n;
                        //code = code + @" return result.Select(" + viewModelName + ".ToModel).ToList();" + n + "}" + n + n;
                    }

                    #endregion
                }
                else
                {
                    #region TableNameDetails

                    var actionName = tableName + @"Details";
                    contCode = contCode + @"
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

                    contCode = contCode + @"
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
                            ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel +
                               @" + "" added"";
                            return RedirectToAction(""List" + tableName + @""");
                        }
                        ModelState.AddModelError(""" + tableName + @"AddingError"", ""There was a problem adding the " +
                               tableName + @" "" + model." + primaryKeyCamel + @");
                   return PartialView(""_Add" + tableName + @""", model);
                    }
                   return PartialView(""_Add" + tableName + @""", model);
                 }" + n + n;

                    #endregion

                    #region EditTableName

                    contCode = contCode + @" 
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
                                ViewBag.SuccessMessage = """ + tableName + @" "" + model." + primaryKeyCamel +
                               @" + "" saved"";
                                return RedirectToAction(""List" + tableNameplural + @""");
                            }
                            ModelState.AddModelError(""" + tableName +
                               @"EditError"", ""There was a problem editing the " + tableName + @" "" + model." +
                               primaryKeyCamel + @");
                    return PartialView(""_Edit" + tableName + @""", model);
                        }
                    return PartialView(""_Edit" + tableName + @""", model);

                    }" + n + n;

                    #endregion

                    #region ListTableName

                    contCode = contCode + @"  
                // GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"()
                {
                    Log.Info(""" + tableName + @" called"");
                    return PartialView(""_Edit" + tableName + @""", " + repoName + @".Get" + tableNameplural +
                               @"List());
                }" + n + n;

                    #endregion

                    #region DeleteTableName

                    contCode = contCode + @" 
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

                    contCode = contCode + @"   
                // POST: /" + controllerName + @"/Delete" + tableName + @"/5
                [HttpPost, ActionName(""Delete" + tableName + @""")]
                public async Task<ActionResult> DeleteConfirmed" + tableName + @"(int id)
                {
                    var model = await " + repoName + @".Get" + tableName + @"(id);
                    await Task.Run(() =>
                    {
                        " + repoName + @".Delete" + tableName + @"Async(id);
                    });
                    return RedirectToAction(""List" + tableNameplural + @""");
                }" + n + n;

                    #endregion

                    #region GetTableNameByForeignKey

                    Fk(tableName);
                    Pk(tableName);
                    foreach (DataGridViewRow item_ in FKGV.Rows)
                    {
                        if (item_.Cells[0].Value == null)
                        {
                            continue;
                        }

                        var fkCamel = "";


                        if (item_.Cells[3].Value != null)
                        {
                            var fkColumn = item_.Cells[3].Value.ToString();
                            var fkIdFixed = fkColumn.Replace("ID", "Id");
                            fkCamel = fkIdFixed.First().ToString().ToUpper() + String.Join("", fkIdFixed.Skip(1));
                        }

                        contCode = contCode + @"
                            // GET: /" + controllerName + @"/" + tableNameplural + @"By" + fkCamel + @"
                            public ActionResult " + tableNameplural + @"By" + fkCamel + @"(int? id)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                Log.Info(""" + tableNameplural + @"By" + fkCamel + @" called with id: ""+id.Value);
                                return View(" + repoName + @".Get" + tableNameplural + "By" + fkCamel + @"(id.Value));
                            }" + n + n;
                    }

                    #endregion
                }

                #region JSON list

                contCode = contCode + @"
                #region Json
  
                // Json GET: /" + controllerName + @"/List" + tableNameplural + @"
                public ActionResult List" + tableNameplural + @"Json()
                {
                    //Log.Info(""" + tableName + @" called"");
                    return Json(" + repoName + @".Get" + tableNameplural + @"List(), JsonRequestBehavior.AllowGet);
                }" + n + n;


                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    var fkCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        var fkColumn = item_.Cells[3].Value.ToString();
                        var fkIdFixed = fkColumn.Replace("ID", "Id");
                        fkCamel = fkIdFixed.First().ToString().ToUpper() + String.Join("", fkIdFixed.Skip(1));
                    }

                    contCode = contCode + @"
                            // Json GET: /" + controllerName + @"/" + tableNameplural + @"By" + fkCamel + @"Json
                            public ActionResult " + tableNameplural + @"By" + fkCamel + @"Json(int? id)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                return Json(" + repoName + @".Get" + tableNameplural + "By" + fkCamel +
                               @"(id.Value), JsonRequestBehavior.AllowGet);
                            }" + n + n;
                }

                contCode = contCode + @"
                #endregion";

                #endregion

                #region Kendo Grid

                contCode = contCode + @"
                
                #region Kendo Grid
      
                // Json GET: /" + controllerName + @"/List" + tableNameplural + @"Kendo
                public ActionResult List" + tableNameplural + @"Kendo([DataSourceRequest] DataSourceRequest request)
                {
                    //Log.Info(""" + tableName + @" called"");
                    return Json(" + repoName + @".Get" + tableNameplural +
                           @"List().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }" + n + n;


                foreach (DataGridViewRow item_ in FKGV.Rows)
                {
                    if (item_.Cells[0].Value == null)
                    {
                        continue;
                    }

                    var fkCamel = "";


                    if (item_.Cells[3].Value != null)
                    {
                        var fkColumn = item_.Cells[3].Value.ToString();
                        var fkIdFixed = fkColumn.Replace("ID", "Id");
                        fkCamel = fkIdFixed.First().ToString().ToUpper() + String.Join("", fkIdFixed.Skip(1));
                    }

                    contCode = contCode + @"
                            // Json GET: /" + controllerName + @"/" + tableNameplural + @"By" + fkCamel + @"Json
                            public ActionResult " + tableNameplural + @"By" + fkCamel +
                               @"Kendo(int? id,[DataSourceRequest] DataSourceRequest request)
                            {
                                if (id == null)
                                {
                                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                                }
                                return Json(" + repoName + @".Get" + tableNameplural + "By" + fkCamel +
                               @"(id.Value).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                            }" + n + n;
                }
                contCode = contCode + @"
                #endregion" + n + n;

                #endregion

                contCode = contCode + "#endregion" + n + n;
            }
            //Always close the DataReader and connection.
            ActionsResult.Text = contCode;
            ActionsResult.Focus();
            ActionsResult.SelectAll();
        }


        private static string ToPlural(string text)
        {
            var tableNameplural = text + "s";
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
            //FillTableNameListBoxes();
            if (DBSettings.SelectedIndex == 4)
            {
               
                help.Play();
            }

        }

        private void FillTableNameListBoxes()
        {
            if (ServeTB.Text.Length > 0 && UserTB.Text.Length > 0 && PassTB.Text.Length > 0)
            {
                TableNameRepo.Items.Clear();
                VMTableCB.Items.Clear();
                ViewHelperTableCB.Items.Clear();

                var dt = _dbConnection.GetSchema("Tables");
                var tables = (from DataRow row in dt.Rows where ((string)row[3]).Equals("BASE TABLE") && ((string)row[1]).Equals("dbo") select (string)row[2]).ToList();
                tables.Sort();
                foreach (var item in tables)
                {
                    TableNameRepo.Items.Add(item);
                    VMTableCB.Items.Add(item);
                    ViewHelperTableCB.Items.Add(item);

                }
            }
        }

        private void ServeTB_TextChanged(object sender, EventArgs e)
        {
        }

        private void ViewModelGen_FormClosing(object sender, FormClosingEventArgs e)
        {
            _dbConnection.Close();
        }

        private void DisplayAnn_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RepoNameTB_TextChanged(object sender, EventArgs e)
        {
        }

        private void TableNameRepo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                QueueBtn.PerformClick();
            }
        }

        private void TableCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                VMGenB.PerformClick();
            }
        }

        private void CodeText_Click(object sender, EventArgs e)
        {
            CodeText.SelectAll();
            if (!string.IsNullOrEmpty(CodeText.Text))
                Clipboard.SetText(CodeText.Text);
        }

        private void ViewHelperTableCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewHelperTableCB.SelectedIndex == -1) return;
            listBox1.Items.Clear();
            _propertyList.Clear();
            classfilename.Text = "";
            var tableName = ViewHelperTableCB.SelectedItem.ToString();

            var columns = new string[100, 4];
            const string n = "\n";
            try
            {


                var cmd = new SqlCommand
                {
                    Connection = _dbConnection,
                    CommandText = "SELECT top 1 * FROM " + tableName
                };

                //Retrieve records from the Employees table into a DataReader.
                var myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);

                //Retrieve column schema into a DataTable.
                var schemaTable = myReader.GetSchemaTable();
                var i = 0;
                //For each field in the table...
                if (_propertyList == null)
                    _propertyList = new List<Property>();
                foreach (DataRow myField in schemaTable.Rows)
                {
                    columns[i, 0] = myField["ColumnName"].ToString();
                    columns[i, 1] = myField["DataType"].ToString();
                    columns[i, 2] = myField["ColumnSize"].ToString();
                    columns[i, 3] = myField["AllowDBNull"].ToString();

                    var columnName = myField["ColumnName"].ToString();

                    if ((columnName.Equals("createdDate") && !DefaultscheckBox.Checked) ||
                        (columnName.Equals("modifiedDate") && !DefaultscheckBox.Checked) ||
                        (columnName.Equals("createdBy") && !DefaultscheckBox.Checked) ||
                        (columnName.Equals("modifiedBy") && !DefaultscheckBox.Checked) ||
                        (columnName.Equals("rowVersion") && !DefaultscheckBox.Checked)
                        )
                    {
                        continue;
                    }

                    var dataType =
                        myField["DataType"].ToString()
                            .Replace("System.", "")
                            .Replace("Int16", "short")
                            .Replace("Int32", "int")
                            .Replace("Int", "int")
                            .Replace("String", "string")
                            .Replace("Boolean", "bool");
                    var size = Convert.ToInt32(myField["ColumnSize"]);
                    var isNullable = Convert.ToBoolean(myField["AllowDBNull"]);
                    var isKey = Convert.ToBoolean(myField["IsKey"]);

                    var property = new Property { Name = columnName, Type = dataType, Size = size, IsKey = isKey, IsNullable = isNullable };

                    _propertyList.Add(property);
                    listBox1.Items.Add(property.Name);
                    i++;
                }

                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not read the table. Pelase restart! \n Error: " + ex.ToString());

            }
        }

        private void ViewHelperTableCB_Click(object sender, EventArgs e)
        {
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (_propertyList.Count == 0)
            {

                butters.Play();
                MessageBox.Show("Select a table or a file, Bro!");
                return;
            }
            var labelPost = labelPostfix.Text.Length > 0 ? @",""" + labelPostfix.Text + @"""" : "";
            var tableName = ViewHelperTableCB.SelectedItem == null ? classfilename.Text.Replace(".cs", "") : ViewHelperTableCB.SelectedItem.ToString();
            const string n = "\n";
            var code = @"<!--#region " + tableName + @"  -->" + n;

            if (radioButtonLabel.Checked)
            {
                var i = 0;
                foreach (var item in listBox1.Items)
                {
                    var property = _propertyList.Single(p => p.Name == (string)item);

                    if (radioButtonBoth.Checked || radioButtonLabel.Checked)
                    {
                        code = property.Type != "bool"
                            ? code + @"@Html.WTLabelFor(a => a." + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                              @", new { @class = ""control-label col-md-12" + @""" }" + labelPost + @")" + n
                            : code + @"@Html.WTLabelFor(a => a." + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                              @", new { @class = ""control-label col-md-12" + @""" })" + n + n;
                    }

                    if (StaticControlsCheck.Checked)
                        code = code + @"<p data-bind=""display: @Html.NameFor(a => a." + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                               @").ToString()""></p>" + n + n;
                    else
                        code = code + GenerateControl(property.Type, (AutoCamelCase.Checked ? property.CamelName : property.Name));



                    //vmCode = vmCode + "public " + dataType + " " + columnNameCamel + " { get; set; }" + n + n;

                    //toEntity = toEntity + columnName + " = " + columnNameCamel + "," + n;
                    //toEntityP = toEntityP + "entity." + columnName + " = " + columnNameCamel + ";" + n;
                    //toModel = toModel + "model." + columnNameCamel + " = entity." + columnName + ";" + n;
                    i++;
                }
            }
            else if (radioButtonBoth.Checked)
            {
                code = code + @"<div class=""row"">";
                foreach (var item in listBox1.Items)
                {
                    var property = _propertyList.Single(p => p.Name == (string)item);

                    var control = StaticControlsCheck.Checked
                        ? @"<p class=""form-control-static"" data-bind=""display: @Html.NameFor(a => a." +
                          nestedPrefix.Text +
                           property.CamelName + @").ToString()""></p>" + n
                        : GenerateControl(property.Type, property.CamelName, property.Size);


                    code = property.Type != "bool"
                        ? code + n + @"<div class=""" + textBoxFormclass.Text + @""">
                                                @Html.WTLabelFor(a => a." + nestedPrefix.Text + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                          @", new { @class = ""control-label col-md-12"" }" + labelPost + @")
                                                <div class=""col-md-12"">" +
                          control
                          + @"</div>" + n + @"   </div>"
                        : code + n + @"<div class=""" + textBoxFormclass.Text + @""">
                                                @Html.WTLabelFor(a => a." + nestedPrefix.Text + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                          @", new { @class = ""control-label col-md-12"" },""" + @""")
                                                <div class=""col-md-12"">" +
                          control
                          + @"</div>" + n + @"   </div>";
                }
                code = code + @"</div>" + n;
            }
            //Old lucky checkbox code
            else
            {
                var listIndex = 0;
                for (var k = 0; k <= listBox1.Items.Count / rowCount.Value; k++)
                {
                    if (listIndex == listBox1.Items.Count) continue;

                    code = code + @"<div class=""row"">";
                    foreach (var item in listBox1.Items)
                    {
                        var property = _propertyList.Single(p => p.Name == (string)item);

                        var control = StaticControlsCheck.Checked
                            ? @"<p class=""form-control-static"" data-bind=""display: @Html.NameFor(a => a." +
                               (AutoCamelCase.Checked ? property.CamelName : property.Name) + @").ToString()""></p>" + n
                            : GenerateControl(property.Type, (AutoCamelCase.Checked ? property.CamelName : property.Name), property.Size);


                        code = property.Type != "bool"
                            ? code + n + @"<div class=""form-group col-md-" + GroupMd.Value + @""">
                                                @Html.WTLabelFor(a => a." + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                              @", new { @class = ""control-label col-md-" + ControlLabelMd.Value + @""" }" + labelPost + @")
                                                <div class=""col-md-" + ControllMD.Value + @""">" +
                              control
                              + @"</div>" + n + @"   </div>"
                            : code + n + @"<div class=""form-group col-md-" + GroupMd.Value + @""">
                                                @Html.WTLabelFor(a => a." + (AutoCamelCase.Checked ? property.CamelName : property.Name) +
                              @", new { @class = ""control-label col-md-" + ControlLabelMd.Value +
                              @""" })                                                
                                                <div class=""col-md-" + ControllMD.Value + @""">" +
                              control
                              + @"</div>" + n + @"   </div>"
                            ;

                        listIndex++;
                    }
                    code = code + @"</div>" + n;
                }
            }

            code = code + n + @" <!--#endregion -->";
            ViewCode.Text = code;
            nice.Play();
        }

        private string GenerateControl(string dataType, string controlName, int? size = null)
        {
            var c = "";
            const string n = "\n";
            switch (dataType)
            {
                case "string":
                case "String":
                    c = size != null && size.Value < 200
                        ? @"@Html.TextBoxFor(a => a." + nestedPrefix.Text + controlName +
                          @", new { @class = ""form-control input-sm"", @data_bind = ""razorNaming: true, value: "" + Html.NameFor(a => a." +
                          nestedPrefix.Text + controlName + @") })" + n + @"@Html.ValidationMessageFor(a => a." +
                          nestedPrefix.Text + controlName + @", null, new { @class = ""help-block"" })" + n
                        : @"@Html.TextAreaFor(a => a." + nestedPrefix.Text + controlName +
                          @", new { @class = ""form-control input-sm"", @data_bind = ""razorNaming: true, value: "" + Html.NameFor(a => a." +
                          nestedPrefix.Text + controlName + @") })" + n + @"@Html.ValidationMessageFor(a => a." +
                          nestedPrefix.Text + controlName + @", null, new { @class = ""help-block"" })" + n;
                    break;

                case "bool":
                case "bool?":
                case "Nullable<bool>":
                    c = @"@Html.CheckBox(""" + Regex.Replace(controlName, "(\\B[A-Z])", " $1") + @""", Model." +
                        nestedPrefix.Text + controlName +
                        @".GetValueOrDefault(), new { @class = ""form-control"", @data_bind = ""razorNaming: true, checkedUniform: "" + Html.NameFor(a => a." +
                        nestedPrefix.Text + controlName + @") })" + n + @"@Html.ValidationMessageFor(a => a." +
                        nestedPrefix.Text + controlName + @", null, new { @class = ""help-block""})" + n;
                    break;
                case "int":
                case "int?":
                case "Nullable<int>":
                case "tinyint":
                case "smallint":
                case "bigint":
                case "short":
                case "short?":
                case "Nullable<short>":
                    c = @"@Html.TextBoxFor(a => a." + nestedPrefix.Text + controlName +
                        @", new { @class = ""form-control input-sm"", @type=""number"", @data_bind = ""razorNaming: true, value: "" + Html.NameFor(a => a." +
                        nestedPrefix.Text +
                        controlName + @") })" + n + @"@Html.ValidationMessageFor(a => a." + nestedPrefix.Text +
                        controlName +
                        @", null, new { @class = ""help-block"" })" + n;
                    break;
            }
            return c;
        }

        private void luckyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (luckyCheckBox.Checked)
            //{
            //    LuckyGroup.Show();
            //    generateLabelsCheck.Checked = true;
            //    generateLabelsCheck.Enabled = false;
            //}
            //else
            //{
            //    generateLabelsCheck.Enabled = true;

            //    LuckyGroup.Hide();
            //}
        }

        private void columnList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listBox1.IndexFromPoint(e.Location);
                if (item >= 0)
                {
                    listBox1.SelectedIndex = item;
                    listBox1.Items.RemoveAt(item);
                }
            }
            if (listBox1.SelectedItem == null) return;
            listBox1.DoDragDrop(listBox1.SelectedItem, DragDropEffects.Move);
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            var point = listBox1.PointToClient(new Point(e.X, e.Y));
            var index = listBox1.IndexFromPoint(point);
            if (index < 0) index = listBox1.Items.Count - 1;
            var data = e.Data.GetData(typeof(string));
            listBox1.Items.Remove(data);
            listBox1.Items.Insert(index, data);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void rowCount_ValueChanged(object sender, EventArgs e)
        {
            if (rowCount.Value < 7 && rowCount.Value > 0 && 12 % rowCount.Value == 0)
            {
                GroupMd.Value = 12 / rowCount.Value;
            }
        }

        private void radioButtonBoth_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFormclass.Enabled = radioButtonBoth.Checked;
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (classFile.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
                _propertyList.Clear();
                classfilename.Text = classFile.SafeFileName;
                button1.Text = classFile.SafeFileName;
                try
                {
                    var pattern = new Regex(@"public (.*) { get; set; }");

                    var lines = File.ReadAllLines(classFile.FileName);
                    foreach (var line in lines)
                    {
                        var match = pattern.Match(line);
                        if (!match.Success) continue;
                        var prop = match.Value.Replace("{ get; set; }", "").Replace("public", "").Replace("virtual", "").Trim();
                        var array = prop.Split(null);
                        var property = new Property { Name = array[1], Type = array[0] };
                        if (_propertyList == null)
                            _propertyList = new List<Property>();
                        _propertyList.Add(property);
                        listBox1.Items.Add(property.Name);

                    }

                    ViewHelperTableCB.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void DBSettings_Selected(object sender, TabControlEventArgs e)
        {

        }
    }
}