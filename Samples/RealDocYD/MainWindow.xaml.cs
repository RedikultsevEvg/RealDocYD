using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Disk.SDK;
using Disk.SDK.Provider;

namespace RealDocYD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public class IssueModel
    {
        public int SiteNum { get; set; }
        public int IssueNum { get; set; }
    }
    public partial class MainWindow : Window
    {
        private const string CLIENT_ID = "f75503b1cc734370a0a0e1f6e1acabe5";
        private const string constAccessToken = "AgAAAAACuFN0AAW808LC4Pxn90nevCsZgQSS4Oo";

        private string currentPath;

        private IDiskSdkClient sdk;
        private ObservableCollection<DiskItemInfo> folderItems;
        private readonly ICollection<DiskItemInfo> selectedItems = new Collection<DiskItemInfo>();
        private readonly ICollection<DiskItemInfo> cutItems = new Collection<DiskItemInfo>();
        private readonly ICollection<DiskItemInfo> copyItems = new Collection<DiskItemInfo>();
        private bool isLaunch;

        public static string AccessToken { get; set; }

        public IssueModel Issue { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Issue = new IssueModel();
            this.DataContext = Issue;
            this.CreateSdkClient();
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            string s;
            string siteNum = Convert.ToString(this.Issue.SiteNum);
            string issueNum = Convert.ToString(this.Issue.IssueNum);

            try
            {
                tblPath.Text = "Определение ссылки...";
                this.ShowProgress();
                currentPath = "wk/IP_R/" + siteNum + "/ISSUEDS/" + issueNum + "/";
                this.sdk.PublishAsync(this.currentPath);
                s = "Объект: "+ siteNum + ", Выпуск: " + issueNum;
            }
            catch
            {
                s = "Ошибка публикации";     
            }
            tblPath.Text = s;

        }

        private void CreateSdkClient()
        {
            this.sdk = new DiskSdkClient();
            sdk.AccessToken = constAccessToken;
            this.AddCompletedHandlers();
        }

        private void unPublish_Click(object sender, RoutedEventArgs e)
        {
            //this.ShowProgress();
            //this.sdk.UnpublishAsync(this.selectedItems.First().OriginalFullPath);
        }

        private void SdkOnPublishCompleted(object sender, GenericSdkEventArgs<string> e)
        {
            if (e.Error == null)
            {
                this.Dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            //this.popupLink.IsOpen = true;
                            if (e.Result == null || e.Result == "") tblLink.Text = "Ошибка поиска папки!";
                            else
                            {
                                tblLink.Text = "Ссылка на папку скопирована в буфер обмена";
                                Clipboard.SetText(e.Result);
                            }
                            
                        }));
                //this.InitFolder(this.currentPath);
            }
            else
            {
                //this.ProcessError(e.Error);
            }

            this.ChangeVisibilityOfProgressBar(Visibility.Collapsed);
        }

        private void AddCompletedHandlers()
        {
            //this.sdk.CopyCompleted += this.SdkOnCopyCompleted;
            //this.sdk.GetListCompleted += this.SdkOnGetListCompleted;
            //this.sdk.MakeFolderCompleted += this.SdkOnMakeFolderCompleted;
            //this.sdk.MoveCompleted += this.SdkOnMoveCompleted;
            this.sdk.PublishCompleted += this.SdkOnPublishCompleted;
            //this.sdk.RemoveCompleted += this.SdkOnRemoveCompleted;
            //this.sdk.TrashCompleted += this.SdkOnTrashCompleted;
            //this.sdk.UnpublishCompleted += this.SdkOnUnpublishCompleted;
        }

        private void ChangeVisibilityOfProgressBar(Visibility visibility, bool isIndeterminate = true)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.pbWaiting.Value = 0;
                this.pbWaiting.Visibility = visibility;
                this.pbWaiting.IsIndeterminate = isIndeterminate;
            }));
        }

        private void ShowProgress(bool isIndeterminate = true)
        {
            this.pbWaiting.Visibility = Visibility.Visible;
            this.pbWaiting.IsIndeterminate = isIndeterminate;
        }

        private void UpdateProgress(ulong current, ulong total)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.pbWaiting.Value = current;
                this.pbWaiting.Maximum = total;
            }));
        }
    }
}
