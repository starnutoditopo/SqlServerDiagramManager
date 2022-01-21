using Microsoft.Win32;
using System;
using System.Windows;
using System.Data.SqlClient;

namespace SqlServerDiagramManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainWindowDataContext mainWindowDataContext = LoadSettings();
        mainWindowDataContext.PropertyChanged += MainWindowDataContext_PropertyChanged;
        this.DataContext = mainWindowDataContext;
    }

    private void MainWindowDataContext_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.SaveSettings(sender as MainWindowDataContext);
    }
    private MainWindowDataContext LoadSettings()
    {
        MainWindowDataContext mainWindowDataContext = new MainWindowDataContext();

        mainWindowDataContext.InputFilePath = Properties.Settings.Default.InputFilePath;
        mainWindowDataContext.OutputFilePath = Properties.Settings.Default.OutputFilePath;
        mainWindowDataContext.InputConnectionstring = Properties.Settings.Default.InputConnectionstring;
        mainWindowDataContext.OutputConnectionstring = Properties.Settings.Default.OutputConnectionstring;
        mainWindowDataContext.InputDiagramName = Properties.Settings.Default.InputDiagramName;
        mainWindowDataContext.OutputDiagramName = Properties.Settings.Default.OutputDiagramName;

        return mainWindowDataContext;
    }
    private void SaveSettings(MainWindowDataContext mainWindowDataContext)
    {
        Properties.Settings.Default.InputFilePath = mainWindowDataContext.InputFilePath;
        Properties.Settings.Default.OutputFilePath = mainWindowDataContext.OutputFilePath;
        Properties.Settings.Default.InputConnectionstring = mainWindowDataContext.InputConnectionstring;
        Properties.Settings.Default.OutputConnectionstring = mainWindowDataContext.OutputConnectionstring;
        Properties.Settings.Default.InputDiagramName = mainWindowDataContext.InputDiagramName;
        Properties.Settings.Default.OutputDiagramName = mainWindowDataContext.OutputDiagramName;
        Properties.Settings.Default.Save();
    }

    public new MainWindowDataContext DataContext
    {
        get
        {
            return base.DataContext as MainWindowDataContext;
        }
        private set
        {
            base.DataContext = value;
        }
    }

    private void ChooseInputFile_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Binary files|*.bin|All files|*.*";
        openFileDialog.DefaultExt = "*.bin";
        openFileDialog.Title = "Select input file";
        openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.DataContext.InputFilePath);
        openFileDialog.FileName = System.IO.Path.GetFileName(this.DataContext.InputFilePath);
        if (openFileDialog.ShowDialog() == true)
        {
            this.DataContext.InputFilePath = openFileDialog.FileName;
        }
    }
    private void ChooseOutputFile_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Binary files|*.bin|All files|*.*";
        saveFileDialog.DefaultExt = "*.bin";
        saveFileDialog.Title = "Select output file";
        saveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.DataContext.OutputFilePath);
        saveFileDialog.FileName = System.IO.Path.GetFileName(this.DataContext.OutputFilePath);
        if (saveFileDialog.ShowDialog() == true)
        {
            this.DataContext.OutputFilePath = saveFileDialog.FileName;
        }
    }

    private void On_UploadDiagramClick(object sender, RoutedEventArgs e)
    {
        byte[] data = System.IO.File.ReadAllBytes(this.DataContext.InputFilePath);

        using (SqlConnection connection = new SqlConnection(this.DataContext.OutputConnectionstring))
        {
            var queryString = string.Format("insert into sysdiagrams (name, principal_id, version, definition) values (@diagramName, 1, 1, 0x{0})", BitConverter.ToString(data).Replace("-", "").ToLower());

            SqlCommand command = new SqlCommand(queryString, connection);
            SqlParameter parameter = new SqlParameter("diagramName", this.DataContext.OutputDiagramName);
            command.Parameters.Add(parameter);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    private void On_DownloadDiagramClick(object sender, RoutedEventArgs e)
    {
        string queryString = $@"select TOP 1 [definition] as Data from sysdiagrams where name=@diagramName";

        using (SqlConnection connection = new SqlConnection(this.DataContext.InputConnectionstring))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            SqlParameter parameter = new SqlParameter("diagramName", this.DataContext.InputDiagramName);
            command.Parameters.Add(parameter);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    byte[] data = (byte[])reader["Data"];
                    System.IO.File.WriteAllBytes(this.DataContext.OutputFilePath, data);
                    break;
                }
            }
        }
    }
}
