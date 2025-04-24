public Form4()
{
    InitializeComponent();
    string curDir = Directory.GetCurrentDirectory();
    webBrowser1.Url = new Uri($"file:///{curDir}/MapView.html");
}