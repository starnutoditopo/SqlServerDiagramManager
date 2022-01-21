namespace SqlServerDiagramManager;

public class MainWindowDataContext : NotifyPropertyChangedBase
{
    private string inputConnectionstring = string.Empty;
    public string InputConnectionstring
    {
        get
        {
            return this.inputConnectionstring;
        }
        set
        {
            if (SetProperty(ref this.inputConnectionstring, value))
            {
                this.OnPropertyChanged();
            }
        }
    }
    private string outputConnectionstring = string.Empty;
    public string OutputConnectionstring
    {
        get
        {
            return this.outputConnectionstring;
        }
        set
        {
            if (SetProperty(ref this.outputConnectionstring, value))
            {
                this.OnPropertyChanged();
            }
        }
    }
    private string outputDiagramName = string.Empty;
    public string OutputDiagramName
    {
        get
        {
            return this.outputDiagramName;
        }
        set
        {
            if (SetProperty(ref this.outputDiagramName, value))
            {
                this.OnPropertyChanged();
            }
        }
    }
    private string inputDiagramName = string.Empty;
    public string InputDiagramName
    {
        get
        {
            return this.inputDiagramName;
        }
        set
        {
            if (SetProperty(ref this.inputDiagramName, value))
            {
                this.OnPropertyChanged();
            }
        }
    }
    private string inputFileName = string.Empty;
    public string InputFilePath
    {
        get
        {
            return this.inputFileName;
        }
        set
        {
            if (SetProperty(ref this.inputFileName, value))
            {
                this.OnPropertyChanged();
            }
        }
    }

    private string outputFileName = string.Empty;
    public string OutputFilePath
    {
        get
        {
            return this.outputFileName;
        }
        set
        {
            if (SetProperty(ref this.outputFileName, value))
            {
                this.OnPropertyChanged();
            }
        }
    }
}
